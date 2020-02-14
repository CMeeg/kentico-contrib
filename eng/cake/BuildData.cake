public class BuildData
{
    private readonly ICakeContext context;
    private List<Package> packages;

    public string Configuration { get; }
    public string RepositoryUrl { get; }
    public ConvertableDirectoryPath DistDirectory { get; set; }
    public IEnumerable<FilePath> SolutionFiles { get; set; }
    public IEnumerable<FilePath> TestAssemblies { get; set; }
    public IEnumerable<Package> Packages => packages;

    public BuildData(ICakeContext context, string configuration, string repositoryUrl)
    {
        this.context = context;
        packages = new List<Package>();

        Configuration = configuration;
        RepositoryUrl = repositoryUrl;
        DistDirectory = context.Directory("./.dist");
        SolutionFiles = context.GetFiles("./src/**/*.sln");

        TestAssemblies = context.GetFiles(
            "./src/**/bin/" + configuration + "/*.Tests.dll",
            new GlobberSettings
            {
                FilePredicate = file => file.Path.GetFilename().FullPath.ToUpper() != "CMS.TESTS.DLL"
            }
        );
    }

    public BuildData AddPackages(string packagesPath)
    {
        var packagesDir = context.Directory(packagesPath);
        var packageDirs = context.GetSubDirectories(packagesDir);

        foreach (var packageDir in packageDirs)
        {
            var package = CreatePackage(ToConvertable(packageDir));

            if (package == null)
            {
                continue;
            }

            packages.Add(package);
        }

        return this;
    }

    private Package CreatePackage(ConvertableDirectoryPath packageDir)
    {
        var nuspec = CreatePackageNuSpec(packageDir);

        if (nuspec == null)
        {
            // Not a package

            return null;
        }

        var project = CreatePackageProject(packageDir);

        var cmsModule = CreatePackageCmsModule(packageDir);

        return new Package
        {
            PackageDirectory = packageDir,
            NuSpec = nuspec,
            Project = project,
            CmsModule = cmsModule
        };
    }

    private PackageNuSpec CreatePackageNuSpec(ConvertableDirectoryPath packageDir)
    {
        var nuspecFiles = context.GetFiles(packageDir.Path + "/*.nuspec");

        if (nuspecFiles.Count == 0)
        {
            return null;
        }

        var nuspecFile = ToConvertable(nuspecFiles.First());

        string packageId = context.XmlPeek(
            nuspecFile,
            "/n:package/n:metadata/n:id",
            new XmlPeekSettings {
                Namespaces = new Dictionary<string, string> {{ "n", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" }}
            }
        );

        return new PackageNuSpec
        {
            NuSpecFile = nuspecFile,
            PackageId = packageId
        };
    }

    private PackageProject CreatePackageProject(ConvertableDirectoryPath packageDir)
    {
        var csprojFiles = context.GetFiles(packageDir.Path + "/*.csproj");

        if (csprojFiles.Count == 0)
        {
            return null;
        }

        var assemblyInfoFilePath = packageDir + context.Directory("Properties") + context.File("AssemblyInfo.cs");

        return new PackageProject
        {
            ProjectFile = ToConvertable(csprojFiles.First()),
            BinDirectory = packageDir + context.Directory("bin") + context.Directory(Configuration),
            AssemblyInfo = context.ParseAssemblyInfo(assemblyInfoFilePath)
        };
    }

    private PackageCmsModule CreatePackageCmsModule(ConvertableDirectoryPath packageDir)
    {
        var modulePropsFiles = context.GetFiles(packageDir.Path + "/Meeg.Kentico.Cms.Module.props");

        if (modulePropsFiles.Count == 0)
        {
            return null;
        }

        var modulePropsFile = ToConvertable(modulePropsFiles.First());

        string cmsPath = context.XmlPeek(
            modulePropsFile,
            "/Project/PropertyGroup/CmsPath"
        );

        string moduleName = context.XmlPeek(
            modulePropsFile,
            "/Project/PropertyGroup/CmsModuleName"
        );

        return new PackageCmsModule
        {
            PropsFile = modulePropsFile,
            CmsPath = cmsPath,
            ModuleName = moduleName
        };
    }

    private ConvertableFilePath ToConvertable(FilePath path)
    {
        return context.File(path.FullPath);
    }

    private IEnumerable<ConvertableFilePath> ToConvertable(IEnumerable<FilePath> paths)
    {
        return paths.Select(path => ToConvertable(path));
    }

    private ConvertableDirectoryPath ToConvertable(DirectoryPath path)
    {
        return context.Directory(path.FullPath);
    }
}

public class Package
{
    public ConvertableDirectoryPath PackageDirectory { get; set; }
    public string PackageName => GetPackageName();
    public PackageNuSpec NuSpec { get; set; }
    public PackageProject Project { get; set; }
    public PackageCmsModule CmsModule { get; set; }
    public GitVersion GitVersion { get; private set; }
    public string PackageVersion { get; private set; }
    public string PackageReleaseVersion { get; private set; }
    public bool IsPreRelease { get; private set; }

    public bool IsMetaPackage => Project == null;
    public bool IsCmsModule => CmsModule != null;

    private string GetPackageName()
    {
        if (!IsMetaPackage && NuSpec.PackageId.Equals("$id$", StringComparison.OrdinalIgnoreCase))
        {
            return Project.AssemblyInfo.Title;
        }

        return NuSpec.PackageId;
    }

    public void SetVersion(GitVersion gitVersion)
    {
        GitVersion = gitVersion ?? throw new ArgumentNullException(nameof(gitVersion));

        IsPreRelease = !string.IsNullOrEmpty(GitVersion.PreReleaseTag);

        PackageVersion = IsPreRelease
            ? $"{GitVersion.NuGetVersion}{GitVersion.BuildMetaDataPadded}"
            : GitVersion.NuGetVersion;

        PackageReleaseVersion = GitVersion.MajorMinorPatch;
    }
}

public class PackageNuSpec
{
    public ConvertableFilePath NuSpecFile { get; set; }
    public string PackageId { get; set; }
}

public class PackageProject
{
    public ConvertableFilePath ProjectFile { get; set; }
    public ConvertableDirectoryPath BinDirectory { get; set; }
    public AssemblyInfoParseResult AssemblyInfo { get; set; }
}

public class PackageCmsModule
{
    public ConvertableFilePath PropsFile { get; set; }
    public string CmsPath { get; set; }
    public string ModuleName { get; set; }
};

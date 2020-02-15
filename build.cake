//////////////////////////////////////////////////////////////////////
// Directives
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=GitVersion.CommandLine&version=5.1.3
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#addin "nuget:?package=Cake.Incubator&version=5.1.0"
#l "local:?path=eng/cake/BuildData.cake"
#l "local:?path=eng/cake/nuget/NuGet.cake"
#l "local:?path=eng/cake/kentico/Kentico.cake"

//////////////////////////////////////////////////////////////////////
// Arguments
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// Setup
//////////////////////////////////////////////////////////////////////

Setup<BuildData>(setupContext => {
    return new BuildData(setupContext, configuration, "https://github.com/CMeeg/kentico-contrib.git")
        .AddPackages("./src/Meeg.Kentico.ContentComponents");
});

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildData>(data =>
{
    Information("Cleaning: {0}", data.DistDirectory);
    CleanDirectory(data.DistDirectory);

    foreach (var package in data.Packages.Where(p => !p.IsMetaPackage))
    {
        Information("Cleaning: {0}", package.Project.BinDirectory);
        CleanDirectory(package.Project.BinDirectory);
    }
});

Task("Restore-NuGet-Packages")
    .DoesForEach<BuildData, FilePath>(data => data.SolutionFiles, (data, sln, context) =>
{
    Information("Restoring: {0}", sln);
    NuGetRestore(
        sln,
        new NuGetRestoreSettings
        {
            Verbosity = NuGetVerbosity.Quiet
        }
    );
});

Task("Version")
    .DoesForEach<BuildData, Package>(data => data.Packages, (data, package, context) =>
{
    var gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        WorkingDirectory = package.PackageDirectory
    });

    package.SetVersion(gitVersion);

    Information("Versioning: {0}", package.PackageDirectory);
    Information(package.GitVersion.Dump());
    Information("PackageVersion: {0}", package.PackageVersion);
    Information("PackageReleaseVersion: {0}", package.PackageReleaseVersion);
});

Task("Build")
    .DoesForEach<BuildData, FilePath>(data => data.SolutionFiles, (data, sln, context) =>
{
    Information("Building: {0}", sln);
    MSBuild(
        sln,
        settings => settings
            .SetConfiguration(data.Configuration)
            .SetVerbosity(Verbosity.Minimal)
    );
});

Task("Run-Unit-Tests")
    .Does<BuildData>(data =>
{

    var testAssemblies = GetFiles(
        "./src/**/bin/" + configuration + "/*.Tests.dll",
        new GlobberSettings
        {
            FilePredicate = file => file.Path.GetFilename().FullPath.ToUpper() != "CMS.TESTS.DLL"
        }
    );

    var settings = new NUnit3Settings
    {
        NoResults = true
    };

    if (!BuildSystem.IsLocalBuild)
    {
        settings.Where = "cat != Integration";
    }

    NUnit3(testAssemblies, settings);
});

Task("NuGet-Pack")
    .DoesForEach<BuildData, Package>(data => data.Packages, (data, package, context) =>
{
    // If the package already exists then don't package it

    string packageSource = FindPackageSource(package.PackageName, package.PackageVersion);

    if (!string.IsNullOrEmpty(packageSource))
    {
        Information("Skipping: {0} version {1} already exists on source {2}", package.PackageName, package.PackageVersion, packageSource);

        return;
    }

    if (string.IsNullOrEmpty(packageSource) && package.IsPreRelease)
    {
        // We also need to check that the we're not attempting to pre-release a package that has already been released

        packageSource = FindPackageSource(package.PackageName, package.PackageReleaseVersion);

        if (!string.IsNullOrEmpty(packageSource))
        {
            Information("Skipping: {0} cannot pre-release version {1} of package {2} that already exists on source {3}", package.PackageName, package.PackageVersion, package.PackageReleaseVersion, packageSource);

            return;
        }
    }

    // Create the NuGet package

    Information("Packaging: {0} version {1}", package.PackageName, package.PackageVersion);

    var properties = new Dictionary<string, string> {
        { "version", package.PackageVersion },
        { "releaseVersion", package.PackageReleaseVersion },
        { "configuration", data.Configuration }
    };

    if (!package.IsMetaPackage)
    {
        var assemblyInfo = package.Project.AssemblyInfo;

        properties.Add("id", assemblyInfo.Title);
        properties.Add("description", assemblyInfo.Description);
        properties.Add("author", assemblyInfo.Company);
        properties.Add("copyright", assemblyInfo.Copyright);
    }

    if (package.IsCmsModule)
    {
        properties.Add("cmsPath", package.CmsModule.CmsPath);
        properties.Add("cmsModuleName", package.CmsModule.ModuleName);

        WriteModuleMetadata(package);
    }

    var settings = new NuGetPackSettings {
        OutputDirectory = data.DistDirectory,
        Properties = properties,
        Repository = new NuGetRepository {
            Type = "git",
            Url = data.RepositoryUrl,
            Commit = package.GitVersion.Sha
        }
    };

    if (!package.IsMetaPackage && !BuildSystem.IsLocalBuild)
    {
        // Create a symbols package

        settings.Symbols = true;
        settings.ArgumentCustomization = args => args.Append("-SymbolPackageFormat snupkg");
    }

    NuGetPack(package.NuSpec.NuSpecFile, settings);
});

Task("NuGet-Publish")
    .Does<BuildData>(data =>
{
    if (!BuildSystem.IsLocalBuild)
    {
        Error("This script will only publish to a local NuGet feed.");

        return;
    }

    string nuGetFeedPath = EnvironmentVariable("NUGET_LOCAL_FEED_PATH");

    if (string.IsNullOrEmpty(nuGetFeedPath))
    {
        Error("NUGET_LOCAL_FEED_PATH environment variable not set.");

        return;
    }

    NuGetInit(
        data.DistDirectory,
        nuGetFeedPath
    );
});

Task("Cms-Module-Export")
    .DoesForEach<BuildData, Package>(data => data.Packages.Where(p => p.IsCmsModule), (data, package, context) =>
{
    Information("Exporting module: {0}", package.CmsModule.ModuleName);

    if (KentExport(package))
    {
        Information("Success {0}", ":)");
    }
    else
    {
        Error("Failure :(");
    }
});

//////////////////////////////////////////////////////////////////////
// Targets
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Version")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests");

Task("Package")
    .IsDependentOn("Default")
    .IsDependentOn("NuGet-Pack");

Task("Publish")
    .IsDependentOn("Package")
    .IsDependentOn("NuGet-Publish");

Task("Export")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Version")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Cms-Module-Export");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget(target);

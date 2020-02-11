//////////////////////////////////////////////////////////////////////
// Directives
//////////////////////////////////////////////////////////////////////

#tool nuget:?package=GitVersion.CommandLine&version=5.1.3
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
#addin "nuget:?package=Cake.Incubator&version=5.1.0"
#l "local:?path=eng/cake/BuildData.cake"
#l "local:?path=eng/cake/NuGet.cake"

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
    .IsDependentOn("Clean")
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
    .IsDependentOn("Restore-NuGet-Packages")
    .DoesForEach<BuildData, Package>(data => data.Packages, (data, package, context) =>
{
    var gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        WorkingDirectory = package.PackageDirectory
    });

    package.SetGitVersion(gitVersion);

    Information("Versioning: {0}", package.PackageDirectory);
    Information(package.GitVersion.Dump());
    Information($"PackageVersion: {package.PackageVersion}");
});

Task("Build")
    .IsDependentOn("Version")
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
    .IsDependentOn("Build")
    .Does<BuildData>(data =>
{
    NUnit3(
        data.TestAssemblies,
        new NUnit3Settings
        {
            NoResults = true
        }
    );
});

Task("NuGet-Pack")
    .IsDependentOn("Run-Unit-Tests")
    .DoesForEach<BuildData, Package>(data => data.Packages, (data, package, context) =>
{
    // If the package already exists then don't package it

    string packageName = package.PackageName;
    string packageVersion = package.PackageVersion;
    string releaseVersion = packageVersion;
    string packageSource = FindPackageSource(packageName, packageVersion);

    if (!string.IsNullOrEmpty(packageSource))
    {
        Information("Skipping: {0} version {1} already exists on source {2}", packageName, packageVersion, packageSource);

        return;
    }

    if (string.IsNullOrEmpty(packageSource) && package.IsPreRelease)
    {
        // We also need to check that the we're not attempting to pre-release a package that has already been released

        releaseVersion = package.GitVersion.MajorMinorPatch;
        packageSource = FindPackageSource(packageName, releaseVersion);

        if (!string.IsNullOrEmpty(packageSource))
        {
            Information("Skipping: {0} cannot pre-release version {1} of package {2} that already exists on source {3}", packageName, packageVersion, releaseVersion, packageSource);

            return;
        }
    }

    // Create the NuGet package

    Information("Packaging: {0} version {1}", package.PackageName, packageVersion);

    var properties = new Dictionary<string, string> {
        { "version", packageVersion },
        { "releaseVersion", releaseVersion },
        { "configuration", data.Configuration }
    };

    if (!package.IsMetaPackage)
    {
        var assemblyInfo = package.Project.AssemblyInfo;

        properties.Add("id", assemblyInfo.Title);
        properties.Add("description", assemblyInfo.Description);
        properties.Add("author", assemblyInfo.Company);
        properties.Add("copyright", assemblyInfo.Copyright);
        // TODO: Get these values from the props file
        properties.Add("cmsPath", @"..\..\CMS");
        properties.Add("cmsModuleName", assemblyInfo.Title);
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
    .IsDependentOn("NuGet-Pack")
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

//////////////////////////////////////////////////////////////////////
// Targets
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

Task("Package")
    .IsDependentOn("NuGet-Pack");

Task("Publish")
    .IsDependentOn("NuGet-Publish");

//////////////////////////////////////////////////////////////////////
// Execution
//////////////////////////////////////////////////////////////////////

RunTarget(target);

private bool KentExport(Package package)
{
    var outputDirectory = GetCmsModulePackageDirectory(package);

    CleanDirectory(outputDirectory);

    Information("Exporting: {0}", outputDirectory);

    int exitCode = StartProcess(
        "kent",
        new ProcessSettings()
            .UseWorkingDirectory(package.PackageDirectory)
            .SetRedirectStandardOutput(true)
            .SetRedirectStandardError(true)
            .WithArguments(args =>
            {
                args.Append("export")
                    .AppendSwitchQuoted("-v", package.PackageReleaseVersion)
                    .AppendSwitchQuoted("-out", outputDirectory);
            })
    );

    return exitCode == 0;
}

public void WriteModuleMetadata(Package package)
{
    var outputDirectory = GetCmsModulePackageDirectory(package);

    if (!DirectoryExists(outputDirectory))
    {
        Warning("Cannot find module package directory - do you need to export the module?: {0}", outputDirectory);

        return;
    }

    var sourceFile = File("./eng/cake/kentico/ModuleMetaData.xml");
    var outputFile = outputDirectory + File("ModuleMetaData.xml");

    TransformTextFile(sourceFile, "{", "}")
        .WithToken("ModuleName", package.CmsModule.ModuleName)
        .WithToken("Version", package.PackageVersion)
        .Save(outputFile);
}

public ConvertableDirectoryPath GetCmsModulePackageDirectory(Package package)
{
    return package.PackageDirectory + Directory("Package") + Directory(package.PackageReleaseVersion);
}

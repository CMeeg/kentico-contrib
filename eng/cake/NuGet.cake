private string FindPackageSource(string packageName, string packageVersion)
{
    // HACK: This is not ideal, but using StartProcess here because:
    // - the nuget cli list command doesn't find anything but the latest version (filtering doesn't work), and the -AllVersions arg doesn't work
    // - the cake Powershell add-in doesn't execute pwsh, and I kept getting problems with locating and executing the Find-Package cmdlet in Powershell

    int findPackageExitCode = StartProcess(
        "pwsh",
        new ProcessSettings()
            .SetRedirectStandardOutput(true)
            .SetRedirectStandardError(false)
            .WithArguments(args =>
            {
                args.Append("-NoProfile")
                    .AppendSwitch("-ExecutionPolicy", "Bypass")
                    .AppendSwitch("-File", "./eng/cake/Find-PackageSource.ps1")
                    .AppendSwitchQuoted("-PackageName", packageName)
                    .AppendSwitchQuoted("-PackageVersion", packageVersion);
            }),
        out IEnumerable<string> findPackageOutput
    );

    if (findPackageExitCode == 0)
    {
        string packageSource = findPackageOutput.LastOrDefault();

        return packageSource;
    }

    return null;
}

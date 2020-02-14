private bool KentExport(ConvertableDirectoryPath moduleDirectory, string version)
{
    int exitCode = StartProcess(
        "kent",
        new ProcessSettings()
            .UseWorkingDirectory(moduleDirectory)
            .SetRedirectStandardOutput(true)
            .SetRedirectStandardError(true)
            .WithArguments(args =>
            {
                args.Append("export")
                    .AppendSwitch("-v", version);
            })
    );

    return exitCode == 0;
}

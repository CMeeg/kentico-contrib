param
(

    [Parameter()]
    [ValidateNotNullOrEmpty()]
    [string] $PackageName,

    [Parameter()]
    [ValidateNotNullOrEmpty()]
	[string] $PackageVersion
)

$packages = Find-Package -AllowPrereleaseVersions -Name $PackageName -RequiredVersion $PackageVersion -ProviderName NuGet -ErrorAction SilentlyContinue

if ($packages.Count -eq 0)
{
    exit 1
}

$package = $packages[0];

$package.Source

exit 0

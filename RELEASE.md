# Release process

The release process differs slightly depending on if you are compiling a:

* Local release e.g. for development or alpha testing
* Pre-release e.g. for integration or beta testing
* General release e.g. a new version of the software for general consumption

## Pre-requisites

Before you run through a release process, please make sure you:

* Bump the version numbers in the `GitVersion.yml` files for all packages that have changed in this release
  * Following SemVer guidelines
  * Only update "major-minor-patch" - pre-release designations will be added automatically by the build script
* Execute the CMS Module export command:
  * `./build.ps1 -Target Export`
* Update the changelog(s) as appropriate
* Update any documentation as appropriate

## Local release

Local releases require that a local NuGet feed is available:

* Ensure that you have designated a directory to act as a [local NuGet feed](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds)
  * If you have not already done so, also set this local feed as a [NuGet source](https://docs.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-sources)
* Set an environment variable `NUGET_LOCAL_FEED_PATH` and use the full path to your local NuGet feed as its value

To perform a new local release:

* Execute the build script using Powershell:
  * `./build.ps1 -Target Publish`
* Or for a debug build:
  * `./build.ps1 -Target Publish -Configuration Debug`

This will place a new pre-release version of the software on to your local NuGet feed ready for consumption.

> The build script uses GitVersion to automatically generate the package version number for the release based on the state of the repo and any `GitVersion.yml` files, and it will generate the same version number if you run the build script multiple times on the same commit. If you need to re-release a particular version you can either delete the conflicting package version from your local feed; or commit your changes, which will generate a new version number next time you run the script.

## Pre-release

To perform a new pre-release:

* Ensure that any completed feature branches / pull requests ready for release have been merged into `develop`
* Create a new `release` branch from `develop`
  * The release branch name doesn't really matter when using `GitVersion.yml` files
  * If not using `GitVersion.yml`, name the release branch according to SemVer based on the work that has been completed since the last general release e.g. `release/1.0.0`
* Make sure you have followed the release [pre-requisites](#pre-requisites)
* Commit and push the release branch
* Approve the build to be published via Azure DevOps

If any changes are required to the pre-release (e.g. bug fixes, documentation changes):

* Make the required changes on the release branch
* Commit and push the release branch
* Approve the build to be published via Azure DevOps

## General release

> It is assumed that before creating a general release that you have first been through the process of creating a pre-release.

To perform a new general release:

* Merge the release branch into `develop`
* Push `develop`
* Merge the release branch into `master`
* Tag `master` with the version used as the release branch name
* Push `master` and the tag
* Approve the build to be published via Azure DevOps

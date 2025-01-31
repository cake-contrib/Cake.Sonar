#addin "Cake.Git&version=5.0.1"
#tool "dotnet:?package=GitVersion.Tool&version=6.1.0"
#tool "nuget:?package=xunit.runner.console&version=2.9.3"
#tool "nuget:?package=NuGet.CommandLine&version=6.12.2"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "./src/Cake.Sonar.sln";

///////////////////////////////////////////////////////////////////////////////
// WAZZUP
///////////////////////////////////////////////////////////////////////////////

var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var buildNumber = AppVeyor.Environment.Build.Number;

var branchName = isRunningOnAppVeyor ? EnvironmentVariable("APPVEYOR_REPO_BRANCH") : GitBranchCurrent(DirectoryPath.FromString(".")).FriendlyName;
var isMasterBranch = System.String.Equals("master", branchName, System.StringComparison.OrdinalIgnoreCase);
var outputDirNuGet = new DirectoryPath("./nuget/").MakeAbsolute(Context.Environment);

///////////////////////////////////////////////////////////////////////////////
// VERSION
///////////////////////////////////////////////////////////////////////////////

var gitVersion = GitVersion();

///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("PrintVersion")
    .Does(() =>
    {
        Information("Current version is " + gitVersion.FullSemVer + ", nuget version " + gitVersion.SemVer);
    });

Task("Clean")
    .Does(() =>
    {
        EnsureDirectoryDoesNotExist(outputDirNuGet, new DeleteDirectorySettings
        {
            Recursive = true,
            Force = true
        });
        CreateDirectory(outputDirNuGet);
    });

Task("Restore-Nuget-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetRestore(solution);
    });

//////////////////////////////////////////////////////////////////////////////
// Build
//////////////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("PrintVersion")
    .IsDependentOn("Restore-Nuget-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {

        var msBuildSettings = new DotNetMSBuildSettings()
        {
            Version = gitVersion.AssemblySemVer,
            InformationalVersion = gitVersion.InformationalVersion,
            PackageVersion = gitVersion.SemVer
        };

        msBuildSettings.WithProperty("PackageOutputPath", outputDirNuGet.FullPath);

        var settings = new DotNetBuildSettings
        {
            Configuration = configuration,
            MSBuildSettings = msBuildSettings
        };

        DotNetBuild(solution, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetTest("./src/Cake.Sonar.Test/Cake.Sonar.Test.csproj");
    });

//////////////////////////////////////////////////////////////////////////////
// Nuget
//////////////////////////////////////////////////////////////////////////////

Task("Publish")
    .IsDependentOn("Test")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isMasterBranch)
    .Does(() =>
    {

        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("Could not resolve Nuget API key.");

        var package = "./nuget/Cake.Sonar." + gitVersion.SemVer + ".nupkg";

        // Push the package.
        NuGetPush(package, new NuGetPushSettings
        {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = apiKey
        });
    });

///////////////////////////////////////////////////////////////////////////////
// APPVEYOR
///////////////////////////////////////////////////////////////////////////////

Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    //AppVeyor.UpdateBuildVersion(gitVersion.FullSemVer);
});

Task("AppVeyor")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Publish");


///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);

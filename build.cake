#addin "Cake.Git&version=5.0.1"
#tool "dotnet:?package=GitVersion.Tool&version=6.1.0"
#tool "nuget:?package=xunit.runner.console&version=2.9.3"
#tool "nuget:?package=NuGet.CommandLine&version=6.12.2"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("NUGET_API_KEY") ?? "");
var nugetSource = Argument("nugetSource", EnvironmentVariable("NUGET_SOURCE") ?? "https://api.nuget.org/v3/index.json");
var solution = "./src/Cake.Sonar.sln";

///////////////////////////////////////////////////////////////////////////////
// WAZZUP
///////////////////////////////////////////////////////////////////////////////

var isLocalBuild = BuildSystem.IsLocalBuild;
var isPullRequest = BuildSystem.GitHubActions.Environment.PullRequest.IsPullRequest;
var gitHubEvent = EnvironmentVariable("GITHUB_EVENT_NAME");
var isReleaseCreation = "release".Equals(gitHubEvent, StringComparison.OrdinalIgnoreCase);

var isMasterBranch = "refs/heads/master".Equals(BuildSystem.GitHubActions.Environment.Workflow.Ref, StringComparison.OrdinalIgnoreCase);
var outputNuGetDir = new DirectoryPath("./nuget/").MakeAbsolute(Context.Environment);

///////////////////////////////////////////////////////////////////////////////
// VERSION
///////////////////////////////////////////////////////////////////////////////

var gitVersion = GitVersion();

///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    Information($"Local build: {isLocalBuild}");
    Information($"Main branch: {isMasterBranch}");
    Information($"Pull request: {isPullRequest}");
    Information($"Ref: {BuildSystem.GitHubActions.Environment.Workflow.Ref}");
    Information($"Is release creation: {isReleaseCreation}");
});

Task("PrintVersion")
    .Does(() =>
    {
        Information("Current version is " + gitVersion.FullSemVer + ", NuGet version " + gitVersion.SemVer);
    });

Task("Clean")
    .Does(() =>
    {
        EnsureDirectoryDoesNotExist(outputNuGetDir, new DeleteDirectorySettings
        {
            Recursive = true,
            Force = true
        });
        CreateDirectory(outputNuGetDir);
    });

//////////////////////////////////////////////////////////////////////////////
// Build
//////////////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("PrintVersion")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var msBuildSettings = new DotNetMSBuildSettings()
        {
            Version = gitVersion.AssemblySemVer,
            InformationalVersion = gitVersion.InformationalVersion,
            PackageVersion = gitVersion.SemVer
        };

        msBuildSettings.WithProperty("PackageOutputPath", outputNuGetDir.FullPath);

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
        DotNetTest(solution);
    });

//////////////////////////////////////////////////////////////////////////////
// Nuget
//////////////////////////////////////////////////////////////////////////////

Task("Publish")
    .IsDependentOn("Test")
    .WithCriteria(() => isReleaseCreation)
    .Does(() =>
    {
        if (string.IsNullOrEmpty(nugetApiKey))
        {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
        }

        var package = "./nuget/Cake.Sonar." + gitVersion.SemVer + ".nupkg";

        NuGetPush(package, new NuGetPushSettings
        {
            ApiKey = nugetApiKey,
            Source = nugetSource,
            SkipDuplicate = true
        });
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Publish");

RunTarget(target);

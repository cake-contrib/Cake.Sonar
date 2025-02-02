#addin "Cake.Git&version=5.0.1"
#tool "dotnet:?package=GitVersion.Tool&version=6.1.0"
#tool "nuget:?package=xunit.runner.console&version=2.9.3"
#tool "nuget:?package=NuGet.CommandLine&version=6.12.2"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetApiKey = Argument("nugetApiKey", EnvironmentVariable("NUGET_API_KEY") ?? "");
var nugetPublishFeed = EnvironmentVariable("NUGET_SOURCE") ?? "https://api.nuget.org/v3/index.json";
var solution = "./src/Cake.Sonar.sln";

///////////////////////////////////////////////////////////////////////////////
// WAZZUP
///////////////////////////////////////////////////////////////////////////////

var isLocalBuild = BuildSystem.IsLocalBuild;
var isPullRequest = BuildSystem.GitHubActions.Environment.PullRequest.IsPullRequest;
var gitHubEvent = EnvironmentVariable("GITHUB_EVENT_NAME");
var isReleaseCreation = string.Equals(gitHubEvent, "release");

var isMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("refs/heads/master", BuildSystem.GitHubActions.Environment.Workflow.Ref);
var outputDirNuGet = new DirectoryPath("./nuget/").MakeAbsolute(Context.Environment);

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
    Information($"ref: {BuildSystem.GitHubActions.Environment.Workflow.Ref}");
    Information($"Is release creation: {isReleaseCreation}");
});


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
    .WithCriteria(() => isReleaseCreation)
    .Does(() =>
    {

        var apiKey = EnvironmentVariable("NUGET_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("Could not resolve Nuget API key.");

        var package = "./nuget/Cake.Sonar." + gitVersion.SemVer + ".nupkg";

        // Push the package.
        NuGetPush(package, new NuGetPushSettings
        {
            Source = nugetPublishFeed,
            ApiKey = nugetApiKey,
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

#tool "dotnet:?package=GitVersion.Tool&version=6.8.2"

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

Task("Restore")
    .Does(() =>
    {
        DotNetRestore(solution);
    });

Task("Build")
    .IsDependentOn("PrintVersion")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetBuild(solution, new DotNetBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
            MSBuildSettings = new DotNetMSBuildSettings
            {
                Version = gitVersion.AssemblySemVer,
                InformationalVersion = gitVersion.InformationalVersion,
                ContinuousIntegrationBuild = !isLocalBuild
            }
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetTest(solution, new DotNetTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true
        });
    });

//////////////////////////////////////////////////////////////////////////////
// Nuget
//////////////////////////////////////////////////////////////////////////////

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        DotNetPack(solution, new DotNetPackSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true,
            OutputDirectory = outputNuGetDir,
            MSBuildSettings = new DotNetMSBuildSettings
            {
                PackageVersion = gitVersion.SemVer
            }
        });
    });

Task("Publish")
    .IsDependentOn("Pack")
    .WithCriteria(() => isReleaseCreation)
    .Does(() =>
    {
        if (string.IsNullOrEmpty(nugetApiKey))
        {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
        }

        foreach (var package in GetFiles($"{outputNuGetDir}/*.nupkg"))
        {
            DotNetNuGetPush(package.FullPath, new DotNetNuGetPushSettings
            {
                ApiKey = nugetApiKey,
                Source = nugetSource,
                SkipDuplicate = true
            });
        }
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test")
    .IsDependentOn("Publish");

RunTarget(target);

var target = Argument("target", "Default");
var solution = "./src/Cake.Sonar.sln";

var version = "0.0.9";

Task("Restore-Nuget-Packages")
	.Does(() => {
		NuGetRestore(solution);
	});

Task("Default")
	.IsDependentOn("Build");
	
Task("Build")
	.IsDependentOn("Restore-Nuget-Packages")
	.Does(() => {
		MSBuild(solution,new MSBuildSettings {
    		Verbosity = Verbosity.Minimal,
    		ToolVersion = MSBuildToolVersion.VS2015,
    		Configuration = "Release",
    		PlatformTarget = PlatformTarget.MSIL
    	});
	});

Task("Pack")
	.IsDependentOn("Build")
	.Does(() => {
		
		CreateDirectory("nuget");
		CleanDirectory("nuget");
		
		var nuGetPackSettings   = new NuGetPackSettings {
                                Id                      = "Cake.Sonar",
                                Version                 = version,
                                Title                   = "Cake.Sonar",
                                Authors                 = new[] {"Tom Staijen"},
                                Owners                  = new[] {"Tom Staijen"},
                                Description             = "Run sonar for msbuild",
                                Summary                 = "Run sonar for msbuild",
                                ProjectUrl              = new Uri("https://github.com/AgileArchitect/Cake.Sonar"),
								LicenseUrl              = new Uri("https://github.com/AgileArchitect/Cake.Sonar/blob/master/LICENCE"),
                                Tags                    = new [] {"Cake", "Sonar", "MSBuild"},
                                RequireLicenseAcceptance= false,
                                Symbols                 = false,
                                NoPackageAnalysis       = true,
                                Files                   = new [] {
                                                                     new NuSpecContent {Source = "Cake.Sonar.dll" },
                                                                  },
                                BasePath                = "./src/Cake.Sonar/bin/release",
                                OutputDirectory         = "./nuget"
                            };
            
		NuGetPack(nuGetPackSettings);
	});

Task("Publish")
	.IsDependentOn("Pack")
	.Does(() => {
		var package = "./nuget/Cake.Sonar." + version + ".nupkg";
            
// Push the package.
		NuGetPush(package, new NuGetPushSettings {
    		Source = "http://teamcity.ncontrol.local:8222/nuget",
    		ApiKey = "abcdef"
		});
	});

RunTarget(target);
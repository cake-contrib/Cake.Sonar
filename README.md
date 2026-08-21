# Cake.Sonar

[![Build (and release)](https://github.com/cake-contrib/Cake.Sonar/actions/workflows/build.yml/badge.svg)](https://github.com/cake-contrib/Cake.Sonar/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Cake.Sonar.svg)](https://www.nuget.org/packages/Cake.Sonar/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Cake.Sonar.svg)](https://www.nuget.org/packages/Cake.Sonar/)

Addin used to execute the [MSBuild scanner for SonarQube](http://docs.sonarqube.org/display/SCAN/Analyzing+with+SonarQube+Scanner+for+MSBuild) using cake aliases.
Don't forget to include the tool package.

```csharp
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar

Task("Sonar")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("Build")
  .IsDependentOn("Unit")
  .IsDependentOn("SonarEnd");

Task("SonarBegin")
  .Does(() => {
     SonarBegin(new SonarBeginSettings{
        # Supported parameters
        Key = "MyProject",
        Url = "sonarcube.contoso.local",
        Token = "token",
        Verbose = true,
        # Custom parameters
        ArgumentCustomization = args => args
            .Append("/d:sonar.gitlab.project_id=XXXX")
            .Append("/d:sonar.gitlab.xxx=XXXX")
        });
     });
  });

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Token = "token"
     });
  });
```

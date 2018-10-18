# Cake.Sonar

[![Build status](https://ci.appveyor.com/api/projects/status/sm1h3u7jqgen7rac/branch/master?svg=true)](https://ci.appveyor.com/project/tomstaijen/cake-sonar/branch/master)

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
        Login = "admin",
        Password = "admin",
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
        Login = "admin",
        Password = "admin"
     });
  });

```



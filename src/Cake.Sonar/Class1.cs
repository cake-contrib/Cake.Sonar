using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Sonar
{
    [CakeAliasCategory("Sonar")]
    public static class SonarCakeAliases
    {
        [CakeMethodAlias]
        public static void SonarBegin(this ICakeContext context, SonarBeginSettings settings)
        {
            new SonarCake().Begin(settings, context.ProcessRunner, context.Log);
        }

        [CakeMethodAlias]
        public static void SonarEnd(this ICakeContext context, SonarEndSettings settings)
        {
            new SonarCake().End(settings, context.ProcessRunner, context.Log);
        }
    }

    public class SonarCake
    {
        public static string ToolPath = "Tools/MSBuild.SonarQube.Runner.Tool/tools/MSBuild.SonarQube.Runner.exe";

        public void Begin(SonarBeginSettings settings, IProcessRunner runner, ICakeLog log)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("begin");

            AppendArguments(settings, arguments);

            log.Information(arguments.Render());

            var proces = runner.Start(ToolPath, new ProcessSettings()
            {
                Arguments = arguments
            });

            if (settings.Verbose)
                arguments.Append("/d:sonar.verbuse=true");

            proces.WaitForExit();
            var exitCode = proces.GetExitCode();
            log.Information($"Exitcode {exitCode}");
        }

        public void End(SonarEndSettings settings, IProcessRunner runner, ICakeLog log)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("end");

            AppendArguments(settings, arguments);

            log.Information(arguments.Render());

            var proces = runner.Start(ToolPath, new ProcessSettings()
            {
                Arguments = arguments
            });

            proces.WaitForExit();
            var exitCode = proces.GetExitCode();
            log.Information($"Exitcode {exitCode}");
        }

        public void AppendArguments(object s, ProcessArgumentBuilder builder)
        {
            foreach (var pi in s.GetType().GetProperties())
            {
                var attr = pi.GetCustomAttributes<ArgumentAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    var value = pi.GetValue(s);
                    if( value != null )
                        builder.Append($"{attr.Name}{pi.GetValue(s)}");
                }
            }
        }
    }

    public class SonarBeginSettings
    {
        [Argument("/d:sonar.host.url=")]
        public string Url { get; set; }

        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        [Argument("/d:sonar.password=")]
        public string Password { get; set; }

        [Argument("/k:")]
        public string Key { get; set; }

        [Argument("/n:")]
        public string Name { get; set; }

        [Argument("/d:sonar.branch=")]
        public string Branch { get; set; }

        [Argument("/v:")]
        public string Version{ get; set; }

        public bool Verbose { get; set; }
    }

    public class SonarEndSettings
    {

        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        [Argument("/d:sonar.password=")]
        public string Password { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute 
    {
        public string Name { get; set; }

        public ArgumentAttribute(string name)
        {
            Name = name;
        }   
           
    }
}
 
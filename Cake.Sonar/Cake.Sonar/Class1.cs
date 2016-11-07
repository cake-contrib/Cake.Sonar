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
        public static void Begin(this ICakeContext context, SonarBeginSettings settings)
        {
            new SonarCake().Begin(settings, context.ProcessRunner, context.Log);
        }

        public static void End(this ICakeContext context, SonarEndSettings settings)
        {
            new SonarCake().End(settings, context.ProcessRunner, context.Log);
        }
    }

    public class SonarCake
    {
        public void Begin(SonarBeginSettings settings, IProcessRunner runner, ICakeLog log)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("begin");

            AppendArguments(settings, arguments);

            log.Information(arguments.Render());

            var proces = runner.Start("Tools/MSBuild.Sonar.Runner.Tools/tools/MSBUild.Sonar.Runner.exe", new ProcessSettings()
            {
                Arguments = arguments
            });

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

            var proces = runner.Start("Tools/MSBuild.Sonar.Runner.Tools/tools/MSBUild.Sonar.Runner.exe", new ProcessSettings()
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
                    builder.Append($"-D{attr.Name}={pi.GetValue(s)}");
                }
            }
        }
    }

    public class SonarBeginSettings
    {
        [Argument("sonar.url")]
        public string Url { get; set; }
    }

    public class SonarEndSettings
    {
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
 
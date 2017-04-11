﻿using System;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarCake
    {
        private readonly ICakeLog _log;
        private readonly IProcessRunner _runner;

        public SonarCake(ICakeContext context)
        {
            _log = context.Log;
            _runner = context.ProcessRunner;
        }

        private static readonly string ToolPath = "Tools/MSBuild.SonarQube.Runner.Tool/tools/SonarQube.Scanner.MSBuild.exe";

        public void Begin(SonarBeginSettings settings)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("begin");

            AppendArguments(settings, arguments);

            _log.Information(arguments.RenderSafe());

            if (settings.Verbose)
                arguments.Append("/d:sonar.verbuse=true");

            var proces = _runner.Start(ToolPath, new ProcessSettings()
            {
                Arguments = arguments,
                RedirectStandardOutput = settings.Silent
            });

            proces.WaitForExit();

            var lastLine = proces.GetStandardOutput().LastOrDefault();

            if (!string.IsNullOrEmpty(lastLine) && lastLine.Contains("Exit code: 1"))
            {
                var exitCode = proces.GetExitCode();
                _log.Information($"Exitcode {exitCode}");
                throw new Exception("Sonar failure. Check the logs.");
            }

        }

        public void End(SonarEndSettings settings)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("end");

            AppendArguments(settings, arguments);

            _log.Information(arguments.RenderSafe());

            var proces = _runner.Start(ToolPath, new ProcessSettings()
            {
                Arguments = arguments,
                RedirectStandardOutput = settings.Silent
            });

            proces.WaitForExit();
            var exitCode = proces.GetExitCode();

            if (exitCode > 0)
                throw new CakeException("SonarQube failure.");

        }

        public void AppendArguments(object s, ProcessArgumentBuilder builder)
        {
            foreach (var pi in s.GetType().GetProperties())
            {
                AppendArguments(s, builder, pi);
                AppendSecretArguments(s, builder, pi);
            }
        }

        private static void AppendArguments(object s, ProcessArgumentBuilder builder, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttributes<ArgumentAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var value = pi.GetValue(s);
                if (value != null)
                {
                    builder.Append($"{attr.Name}{pi.GetValue(s)}");
                }
            }
        }

        private static void AppendSecretArguments(object s, ProcessArgumentBuilder builder, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttributes<SecretArgumentAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var value = pi.GetValue(s);
                if (value != null)
                {
                    builder.AppendSwitchSecret(attr.Name, "", pi.GetValue(s).ToString());
                }
            }
        }
    }
}
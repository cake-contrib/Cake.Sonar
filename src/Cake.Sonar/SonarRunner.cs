using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Collections.Generic;

namespace Cake.Sonar
{
    public class SonarRunner : Tool<SonarSettings> 
    {
        private readonly ICakeLog _log;
        private readonly ICakeEnvironment _environment;

        public SonarRunner(ICakeLog log, IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _log = log;
            _environment = environment;
        }

        protected override string GetToolName()
        {
            return "SonarQube";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] {"SonarQube.Scanner.MSBuild.exe"};
        }

        public void Run(SonarSettings settings)
        {
            var arguments = settings.GetArguments(_environment);
            _log.Information(arguments.RenderSafe());

            Run(settings, arguments, new ProcessSettings { RedirectStandardOutput = settings.Silent }, null);
        }
    }
}

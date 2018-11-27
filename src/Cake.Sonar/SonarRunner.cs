using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;

namespace Cake.Sonar
{
    public class SonarRunner : Tool<SonarSettings> 
    {
		public static string CORECLR_TOOL_NAME = "SonarScanner.MSBuild.dll";

        private readonly ICakeLog _log;

		private readonly ICakeEnvironment _environment;
        
		private readonly DotNetCoreToolRunner _coreRunner;

		private readonly IToolLocator _toolsLocator;

		public SonarRunner(ICakeLog log, IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
		{
			_log = log;
			_environment = environment;
			_toolsLocator = tools;
			_coreRunner = new DotNetCoreToolRunner(fileSystem, environment, processRunner, tools);
		}

        protected override string GetToolName()
        {
            return "SonarQube";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] {"SonarScanner.MSBuild.exe", "SonarQube.Scanner.MSBuild.exe"};
        }

        public void Run(SonarSettings settings)
        {
            Prepare(settings);

            var arguments = settings.GetArguments(_environment);
            _log.Information(arguments.RenderSafe());

			if(_environment.Runtime.IsCoreClr || settings.UseCoreClr) {
				var tool = _toolsLocator.Resolve(CORECLR_TOOL_NAME);
				if( tool == null ) {
					throw new Exception($"No CoreCLR executable found ({CORECLR_TOOL_NAME})");
				}
				_log.Debug("We're launching for CoreCLR with executable " + tool.FullPath);
				_coreRunner.Execute(new FilePath("dummy.file").MakeAbsolute(_environment), tool.FullPath, arguments, new DotNetCoreToolSettings { });
			}
			else {
				_log.Debug("We're launching for CLR.");
				Run(settings, arguments, new ProcessSettings { RedirectStandardOutput = settings.Silent }, null);
			}
        }

        private void Prepare(SonarSettings settings) {
            var beginSettings = settings as SonarBeginSettings;
            if( beginSettings != null && beginSettings.VersionResult == null ) {
                beginSettings.VersionResult = GetVersion(beginSettings);
            }
        }

        private VersionResult GetVersion(SonarBeginSettings settings)
        {
            try
            {
                var version = new SonarServer().GetVersion(settings.Url).Result;
                return new VersionResult
                {
                    Url = settings.Url,
                    Version = version
                };
            }
            catch (Exception e)
            {
                return new VersionResult
                {
                    Exception = e,
                    Url = settings.Url
                };
            }
        }
    }
}

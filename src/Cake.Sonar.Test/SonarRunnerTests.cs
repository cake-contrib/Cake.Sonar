using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cake.Sonar.Test
{
    public class SonarRunnerTests
    {
        private Mock<IToolLocator> _toolLocator;
        private SonarRunner _runner;

        public SonarRunnerTests()
        {
            var env = new FakeEnvironment(Core.PlatformFamily.Windows);
            env.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            _toolLocator = new Mock<IToolLocator>();
            _runner = new SonarRunner(new FakeLog(), new FakeFileSystem(env), env, new Mock<IProcessRunner>().Object, _toolLocator.Object);
        }

        [Fact]
        public void Executes_CoreClr_Build_When_UseCoreClrSetting_IsSet()
        {
            var settings = new SonarBeginSettings
            {
                UseCoreClr = true
            };

            try
            {
                _runner.Run(settings);
            }
            catch
            {
                // We cannot mock FileSystemAccess in the internals of the SonarRunner 
                // Let´s ignore IOExceptions, just verify that Resolve() was called with the expected (.NET Core tool) arguments
            }

            _toolLocator.Verify(t => t.Resolve(SonarRunner.CORECLR_TOOL_NAME));
        }
    }
}

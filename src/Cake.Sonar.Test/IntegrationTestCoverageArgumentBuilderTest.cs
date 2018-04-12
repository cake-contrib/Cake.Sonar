using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class IntegrationTestCoverageArgumentBuilderTest
    {
        [Fact]
        public void TestBeginSettingsWithDotCoverIntegrationReportsPath()
        {
            var beginSettings = new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
                DotCoverIntegrationReportsPath = "./out/dotCover.html"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.dotcover.it.reportsPaths=""./out/dotCover.html"" /d:sonar.login=""tom"" /d:sonar.password=""god""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.dotcover.it.reportsPaths=""./out/dotCover.html"" /d:sonar.login=""[REDACTED]"" /d:sonar.password=""[REDACTED]""", s);
        }

        [Fact]
        public void TestBeginSettingsWithOpenCoverIntegrationReportsPath()
        {
            var beginSettings = new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
                OpenCoverIntegrationReportsPath = "./out/opencover.xml"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.opencover.it.reportsPaths=""./out/opencover.xml"" /d:sonar.login=""tom"" /d:sonar.password=""god""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.opencover.it.reportsPaths=""./out/opencover.xml"" /d:sonar.login=""[REDACTED]"" /d:sonar.password=""[REDACTED]""", s);
        }

        [Fact]
        public void TestBeginSettingsWithVsCoverageIntegrationReportsPath()
        {
            var beginSettings = new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
                VsCoverageIntegrationReportsPath = "./out/VisualStudio.coverage"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.vscoveragexml.it.reportsPaths=""./out/VisualStudio.coverage"" /d:sonar.login=""tom"" /d:sonar.password=""god""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.vscoveragexml.it.reportsPaths=""./out/VisualStudio.coverage"" /d:sonar.login=""[REDACTED]"" /d:sonar.password=""[REDACTED]""", s);
        }

        [Fact]
        public void TestBeginSettingsWithNCover3IntegrationReportsPath()
        {
            var beginSettings = new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
                NCover3IntegrationReportsPath = "./out/coverage.nccov"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.ncover3.it.reportsPaths=""./out/coverage.nccov"" /d:sonar.login=""tom"" /d:sonar.password=""god""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.ncover3.it.reportsPaths=""./out/coverage.nccov"" /d:sonar.login=""[REDACTED]"" /d:sonar.password=""[REDACTED]""", s);
        }
    }
}

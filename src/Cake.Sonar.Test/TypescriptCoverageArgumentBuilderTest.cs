using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class TypescriptCoverageArgumentBuilderTest
    {
        [Fact]
        public void TypescriptCoverageTest()
        {
            var beginSettings = new SonarBeginSettings
            {
                Token = "token",
                Url = "http://sonarqube.com:9000",
                TypescriptCoverageReportsPath = "coverage1.lcov,coverage2.lcov"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.typescript.lcov.reportPaths=""coverage1.lcov,coverage2.lcov"" /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.typescript.lcov.reportPaths=""coverage1.lcov,coverage2.lcov"" /d:sonar.token=""[REDACTED]""", s);
        }
    }
}

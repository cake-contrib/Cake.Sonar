using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class JavascriptCoverageArgumentBuilderTest
    {
        [Fact]
        public void CoverageExclussionsTest()
        {
            var beginSettings = new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
                JavascriptCoverageReportsPath = "coverage1.lcov,coverage2.lcov"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.javascript.lcov.reportPaths=""coverage1.lcov,coverage2.lcov"" /d:sonar.login=""tom"" /d:sonar.password=""god""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.javascript.lcov.reportPaths=""coverage1.lcov,coverage2.lcov"" /d:sonar.login=""[REDACTED]"" /d:sonar.password=""[REDACTED]""", s);
        }
    }
}

namespace Cake.Sonar.Test
{
    using System;
    using Xunit;

    public class TSLintReportArgumentBuilderTest
    {
        [Fact]
        public void TSLintReportTest()
        {
            var beginSettings = new SonarBeginSettings
            {
                Token = "token",
                Url = "http://sonarqube.com:9000",
                TSLintReportPaths = "tslint-report1.json,tslint-report2.json"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.typescript.tslint.reportPaths=""tslint-report1.json,tslint-report2.json"" /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.typescript.tslint.reportPaths=""tslint-report1.json,tslint-report2.json"" /d:sonar.token=""[REDACTED]""", s);
        }
    }
}

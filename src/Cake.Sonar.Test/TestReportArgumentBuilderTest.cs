using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class TestReportArgumentBuilderTest
    {
        [Fact]
        public void TestReportTest()
        {
            var beginSettings = new SonarBeginSettings
            {
                Token = "token",
                Url = "http://sonarqube.com:9000",
                TestReportPaths = "test-report1.xml,test-report2.xml"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:testExecutionReportPaths=""test-report1.xml,test-report2.xml"" /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:testExecutionReportPaths=""test-report1.xml,test-report2.xml"" /d:sonar.token=""[REDACTED]""", s);
        }
    }
}

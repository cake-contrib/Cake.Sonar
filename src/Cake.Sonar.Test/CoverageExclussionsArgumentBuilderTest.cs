using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class CoverageExclussionsArgumentBuilderTest
    {
        [Fact]
        public void CoverageExclussionsTest()
        {
            var beginSettings = new SonarBeginSettings
            {
                Token = "token",
                Url = "http://sonarqube.com:9000",
                CoverageExclusions = "SomeClass1.cs,SomeClass2.cs"
            };

            var builder = beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.coverage.exclusions=""SomeClass1.cs,SomeClass2.cs"" /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.coverage.exclusions=""SomeClass1.cs,SomeClass2.cs"" /d:sonar.token=""[REDACTED]""", s);
        }
    }
}

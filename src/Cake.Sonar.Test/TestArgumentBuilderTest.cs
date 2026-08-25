using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class TestArgumentBuilderTest
    {
        private readonly SonarBeginSettings _beginSettings;

        public TestArgumentBuilderTest()
        {
            _beginSettings = new SonarBeginSettings
            {
                Token = "token",
                Url = "http://sonarqube.com:9000",
                NUnitReportsPath = "./out/nunit.xml"
            };
        }


        [Fact]
        public void TestGetEndSettingsFromBeginSettings()
        {
            var s = new SonarBeginSettings
            {
                Token = "token",
                Silent = true
            };

            var endSettings = s.GetEndSettings();

            Assert.Equal("token", endSettings.Token);
            Assert.True(endSettings.Silent);
        }

        [Fact]
        public void TestBeginSettings()
        {



            var builder = _beginSettings.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.nunit.reportsPaths=""./out/nunit.xml"" /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.host.url=""http://sonarqube.com:9000"" /d:sonar.cs.nunit.reportsPaths=""./out/nunit.xml"" /d:sonar.token=""[REDACTED]""", s);

        }

        [Fact]
        public void TestToken()
        {
            var builder = new SonarBeginSettings
            {
                Token = "token"
            }.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.token=""token""", r);
            Assert.Equal(@"begin /d:sonar.token=""[REDACTED]""", s);
        }

        [Fact]
        public void TestEndSettings()
        {
            var builder = _beginSettings.GetEndSettings().GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"end /d:sonar.token=""token""", r);
            Assert.Equal(@"end /d:sonar.token=""[REDACTED]""", s);
        }
    }
}

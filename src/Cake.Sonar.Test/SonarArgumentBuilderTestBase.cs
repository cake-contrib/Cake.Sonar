using System;
using Cake.Core.IO;
using Xunit;

namespace Cake.Sonar.Test
{
    public abstract class SonarArgumentBuilderTestBase
    {
        protected static SonarBeginSettings CeateBeginSettings()
        {
            return new SonarBeginSettings
            {
                Login = "tom",
                Password = "god",
                Url = "http://sonarqube.com:9000",
            };
        }

        protected void AssertRenderedSafe(string expected, ProcessArgumentBuilder builder)
        {
            var s = builder.RenderSafe();
            Console.WriteLine($"Rendered Safe: {s}");
            Assert.Equal($"begin /d:sonar.host.url=\"http://sonarqube.com:9000\" {expected} /d:sonar.login=\"[REDACTED]\" /d:sonar.password=\"[REDACTED]\"", s);
        }

        protected static void AssertRendered(string expected, ProcessArgumentBuilder builder)
        {
            var r = builder.Render();
            Console.WriteLine($"Rendered: {r}");
            Assert.Equal($"begin /d:sonar.host.url=\"http://sonarqube.com:9000\" {expected} /d:sonar.login=\"tom\" /d:sonar.password=\"god\"", r);
        }

        protected void AssertResult(string expected, ProcessArgumentBuilder builder)
        {
            AssertRendered(expected, builder);
            AssertRenderedSafe(expected, builder);
        }
    }
}

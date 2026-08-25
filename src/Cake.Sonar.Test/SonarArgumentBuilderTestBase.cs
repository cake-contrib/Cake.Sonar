using System;
using Cake.Core.IO;
using Xunit;

namespace Cake.Sonar.Test
{
    public abstract class SonarArgumentBuilderTestBase
    {
        protected static SonarBeginSettings CreateBeginSettings()
        {
            return new SonarBeginSettings
            {
                Token = "abc",
                Url = "http://sonarqube.com:9000",
            };
        }

        protected static void AssertRenderedSafe(string expected, ProcessArgumentBuilder builder)
        {
            var s = builder.RenderSafe();
            Console.WriteLine($"Rendered Safe: {s}");
            Assert.Equal($"begin /d:sonar.host.url=\"http://sonarqube.com:9000\" {expected} /d:sonar.token=\"[REDACTED]\"", s);
        }

        protected static void AssertRendered(string expected, ProcessArgumentBuilder builder)
        {
            var r = builder.Render();
            Console.WriteLine($"Rendered: {r}");
            Assert.Equal($"begin /d:sonar.host.url=\"http://sonarqube.com:9000\" {expected} /d:sonar.token=\"abc\"", r);
        }

        protected static void AssertResult(string expected, ProcessArgumentBuilder builder)
        {
            AssertRendered(expected, builder);
            AssertRenderedSafe(expected, builder);
        }
    }
}

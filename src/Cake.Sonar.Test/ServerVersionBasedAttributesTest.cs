using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class ServerVersionBasedAttributesTest
    {
        [Fact(Skip = "depends on running sonarqube instance at localhost:9000")]
        public void TestVersion()
        {
            var server = new SonarServer();
            var version = server.GetVersion().Result;
            Console.WriteLine($"Got version {version}");
            Assert.True(version.Major >= 6);
        }
    }
}

using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class BranchArgumentTest
    {

        [Fact]
        public void TestBranchBefore7()
        {
            var builder = new SonarBeginSettings
            {
                VersionResult = new VersionResult
                {
                    Version = Version.Parse("6.7")
                },
                Branch = "master"
            }.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.branch=""master""", r);
            Assert.Equal(@"begin /d:sonar.branch=""master""", s);
        }

        [Fact]
        public void TestBranchDefault()
        {
            var builder = new SonarBeginSettings
            {
                VersionResult = new VersionResult(),
                Branch = "master"
            }.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.branch.name=""master""", r);
            Assert.Equal(@"begin /d:sonar.branch.name=""master""", s);
        }

        [Fact]
        public void TestBranchFromVersion7()
        {
            var builder = new SonarBeginSettings
            {
                Branch = "master",
                VersionResult = new VersionResult
                {
                    Version = Version.Parse("7.0")
                }

            }.GetArguments(null);

            var r = builder.Render();
            var s = builder.RenderSafe();

            Console.WriteLine($"Rendered: {r}");
            Console.WriteLine($"Rendered Safe: {s}");

            Assert.Equal(@"begin /d:sonar.branch.name=""master""", r);
            Assert.Equal(@"begin /d:sonar.branch.name=""master""", s);
        }
    }
}

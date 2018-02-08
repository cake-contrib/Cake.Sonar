using System;
using Cake.Sonar.Attributes;
using Xunit;

namespace Cake.Sonar.Test
{
    public class VersionAttributeTest
    {

        [Theory]
        [InlineData("7.0", null, "7.0", true)]
        [InlineData("7.0", null, "6.9", false)]
        [InlineData("7.0", null, "7.1", true)]
        [InlineData(null, "7.0", "7.1", false)]
        [InlineData(null, "7.0", "7.0", false)]
        [InlineData(null, "7.0", "6.9", true)]

        [InlineData("7.0", "8.0", "6.9", false)]
        [InlineData("7.0", "8.0", "7.0", true)]
        [InlineData("7.0", "8.0", "7.9", true)]
        [InlineData("7.0", "8.0", "8.0", false)]
        [InlineData("7.0", "8.0", "8.1", false)]
        public void Match(string from, string toExcluding, string version, bool match)
        {

            var attribute = new ArgumentAttribute("hi")
            {
                From = from,
                ToExcluding = toExcluding
            };

            var result = attribute.Match(Version.Parse(version));

            Assert.Equal(match, result);

        }
    }
}

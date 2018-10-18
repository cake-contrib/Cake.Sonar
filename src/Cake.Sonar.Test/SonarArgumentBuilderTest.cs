using Xunit;

namespace Cake.Sonar.Test
{
    public class SonarArgumentBuilderTest : SonarArgumentBuilderTestBase {
        [Fact]
        public void TestAnalysisMode() {
            var beginSettings = CeateBeginSettings();
            beginSettings.AnalysisMode = "preview";

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.analysis.mode=""preview""", builder);
        }

        [Fact]
        public void UseWsCacheTest()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.UseWsCache = true;

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.useWsCache=""true""", builder);
        }

        [Fact]
        public void IssuesReportHtmlEnableTest()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.IssuesReportHtmlEnable = true;

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.issuesReport.html.enable=""true""", builder);
        }

        [Fact]
        public void IssuesReportConsoleEnableTest()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.IssuesReportConsoleEnable = true;

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.issuesReport.console.enable=""true""", builder);
        }

        [Fact]
        public void LanguageTest()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.Language = "cs";

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.language=""cs""", builder);
        }
    }
}

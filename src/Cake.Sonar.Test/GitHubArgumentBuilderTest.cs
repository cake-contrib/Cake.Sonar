using Xunit;

namespace Cake.Sonar.Test
{
    public class GitHubArgumentBuilderTest : SonarArgumentBuilderTestBase {

        [Fact]
        public void TestGitHubPullRequest()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubPullRequest = "2";

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.github.pullRequest=""2""", builder);
        }

        [Fact]
        public void TestGitHubRepository()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubRepository = "myOrganisation/myProject";

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.github.repository=""myOrganisation/myProject""", builder);
        }

        [Fact]
        public void TestGitHubLogin()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubLogin = "githubuser";

            var builder = beginSettings.GetArguments(null);

            AssertRendered(@"/d:sonar.github.login=""githubuser""", builder);
            AssertRenderedSafe(@"/d:sonar.github.login=""[REDACTED]""", builder);
        }

        [Fact]
        public void TestGitHubOAuth()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubOAuth = "githuboauth";

            var builder = beginSettings.GetArguments(null);

            AssertRendered(@"/d:sonar.github.oauth=""githuboauth""", builder);
            AssertRenderedSafe(@"/d:sonar.github.oauth=""[REDACTED]""", builder);
        }

        [Fact]
        public void TestGitHubEndpoint()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubEndpoint = @"https://mygithub.com/api/v3";

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.github.endpoint=""https://mygithub.com/api/v3""", builder);
        }

        [Fact]
        public void TestGitHubDeleteOldComments()
        {
            var beginSettings = CeateBeginSettings();
            beginSettings.GitHubDeleteOldComments = true;

            var builder = beginSettings.GetArguments(null);

            AssertResult(@"/d:sonar.github.deleteOldComments=""true""", builder);
        }
    }
}

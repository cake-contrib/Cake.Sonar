using System;
using Xunit;

namespace Cake.Sonar.Test
{
    public class PullRequestArgumentTest
    {
        private readonly SonarBeginSettings _arguments;

        public PullRequestArgumentTest()
        {
            _arguments = new SonarBeginSettings();
        }

        [Fact]
        public void RenderedArguments_Do_Not_Contain_PullRequest_Arguments_If_Not_Set()
        {
            var argumentString = _arguments.GetArguments(null).Render();

            Assert.DoesNotContain("sonar.pullrequest.provider", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.branch", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.key", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.base", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.github.repository", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.github.endpoint", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.vsts.instanceUrl", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.vsts.project", argumentString);
            Assert.DoesNotContain("sonar.pullrequest.vsts.repository", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_Provider_If_Specified()
        {
            _arguments.PullRequestProvider = "vsts";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.provider=\"vsts\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_Branch_If_Specified()
        {
            _arguments.PullRequestBranch = "feature/some-feature-branch";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.branch=\"feature/some-feature-branch\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_Key_If_Specified()
        {
            _arguments.PullRequestKey = 1655;

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.key=\"1655\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_Base_If_Specified()
        {
            _arguments.PullRequestBase = "master";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.base=\"master\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_GithubRepository_If_Specified()
        {
            _arguments.PullRequestGithubRepository = "my-company/my-repo";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.github.repository=\"my-company/my-repo\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_GithubEndpoint_If_Specified()
        {
            _arguments.PullRequestGithubEndpoint = "https://api.github.com";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.github.endpoint=\"https://api.github.com\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_VstsEndpoint_If_Specified()
        {
            _arguments.PullRequestVstsEndpoint = "https://azure.microsoft.com/en-gb/services/devops/server/";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.vsts.instanceUrl=\"https://azure.microsoft.com/en-gb/services/devops/server/\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_VstsProject_If_Specified()
        {
            _arguments.PullRequestVstsProject = "foo";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.vsts.project=\"foo\"", argumentString);
        }

        [Fact]
        public void Contains_The_PullRequest_VstsRepository_If_Specified()
        {
            _arguments.PullRequestVstsRepository = "d0d67431-afe2-4243-b38b-69574658ed82";

            var argumentString = _arguments.GetArguments(null).Render();

            Assert.Contains("sonar.pullrequest.vsts.repository=\"d0d67431-afe2-4243-b38b-69574658ed82\"", argumentString);
        }
    }
}

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class VersionResult
    {
        public string Url { get; set; }
        public Exception Exception { get; set; }
        public Version Version { get; set; }
    }

    /// <summary>
    /// Required prior to Sonar 6.1:
    ///  - Name
    ///  - Version
    /// Required from Sonar 6.1
    ///  - Key
    /// </summary>
    public class SonarBeginSettings : SonarSettings
    {
        public VersionResult VersionResult { get; set; }


        /// <summary>
        /// The url of the used sonar instance. When omitted, http://localhost:9000 is used.
        /// </summary>
        [Argument("/d:sonar.host.url=")]
        public string Url { get; set; }

        /// <summary>
        /// Path to alternative SonarQube.Analysis.xml
        /// </summary>
        [Argument("/s:")]
        public string SettingsFile { get; set; }

        /// <summary>
        /// Key of the project. Required for Sonar 6.1 and up.
        /// </summary>
        [Argument("/k:")]
        public string Key { get; set; }

        /// <summary>
        /// Organization name for sonar cloud versions.
        /// </summary>
        [Argument("/o:")]
        public string Organization { get; set; }

        /// <summary>
        /// Name of the project.
        /// Required prior to Sonar 6.1
        /// </summary>
        [Argument("/n:")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the current branch. Specifying a branch will cause Sonar to analyse different branches as different Sonar projects.
        /// This allows one to use sonar to compare branches on a pull review.
        /// </summary>
        [Argument("/d:sonar.branch.name=", true, From = "7.0")]
        [Argument("/d:sonar.branch=", false, ToExcluding = "7.0")]
        public string Branch { get; set; }

        /// <summary>
        /// Use exclusion to analyze everything but the specified files
        /// </summary>
        [Argument("/d:sonar.exclusions=")]
        public string Exclusions { get; set; }

        /// <summary>
        /// Comma-delimited list of file path patterns to be included in analysis. When set, only files matching the paths set here will be included in analysis.
        /// </summary>
        [Argument("/d:sonar.inclusions=")]
        public string Inclusions { get; set; }

        /// <summary>
        /// Comma-delimited list of file path patterns to be excluded from duplication detection
        /// </summary>
        [Argument("/d:sonar.cpd.exclusions=")]
        public string DuplicationExclusions { get; set; }

        /// <summary>
        /// Comma-delimited list of test file path patterns to be excluded from analysis.
        /// </summary>
        [Argument("/d:sonar.test.exclusions=")]
        public string TestExclusions { get; set; }

        /// <summary>
        /// Comma-delimited list of test file path patterns to be included in analysis. When set, only test files matching the paths set here will be included in analysis.
        /// </summary>
        [Argument("/d:sonar.test.inclusions=")]
        public string TestInclusions { get; set; }

        /// <summary>
        /// A version indicator, e.g. a semantic version or git revision hash.
        /// Required prior to Sonar 6.1
        /// </summary>
        [Argument("/v:")]
        public string Version { get; set; }

        [Argument("/d:sonar.cs.nunit.reportsPaths=")]
        public string NUnitReportsPath { get; set; }

        [Argument("/d:sonar.vbnet.nunit.reportsPaths=")]
        public string NUnitReportsPathVbNet { get; set; }

        [Argument("/d:sonar.cs.xunit.reportsPaths=")]
        public string XUnitReportsPath { get; set; }

        [Argument("/d:sonar.cs.vstest.reportsPaths=")]
        public string VsTestReportsPath { get; set; }

        [Argument("/d:testExecutionReportPaths=")]
        public string TestReportPaths { get; set; }

        /// <summary>
        /// Path to coverage report in the Generic Test Data format
        /// </summary>
        [Argument("/d:sonar.coverageReportPaths=", true)]
        public string CoverageReportPaths { get; set; }

        /// <summary>
        /// Comma-delimited list of file path patterns to be excluded from coverage calculations
        /// </summary>
        [Argument("/d:sonar.coverage.exclusions=")]
        public string CoverageExclusions { get; set; }

        [Argument("/d:sonar.cs.dotcover.reportsPaths=")]
        public string DotCoverReportsPath { get; set; }

        [Argument("/d:sonar.cs.dotcover.it.reportsPaths=")]
        public string DotCoverIntegrationReportsPath { get; set; }

        [Argument("/d:sonar.cs.opencover.reportsPaths=")]
        public string OpenCoverReportsPath { get; set; }

        [Argument("/d:sonar.vbnet.opencover.reportsPaths=")]
        public string OpenCoverReportsPathVbNet { get; set; }

        [Argument("/d:sonar.cs.opencover.it.reportsPaths=")]
        public string OpenCoverIntegrationReportsPath { get; set; }

        [Argument("/d:sonar.cs.vscoveragexml.reportsPaths=")]
        public string VsCoverageReportsPath { get; set; }

        [Argument("/d:sonar.cs.vscoveragexml.it.reportsPaths=")]
        public string VsCoverageIntegrationReportsPath { get; set; }

        [Argument("/d:sonar.cs.ncover3.reportsPaths=")]
        public string NCover3ReportsPath { get; set; }

        [Argument("/d:sonar.cs.ncover3.it.reportsPaths=")]
        public string NCover3IntegrationReportsPath { get; set; }

        [Argument("/d:sonar.cs.ndepend.projectPath=")]
        public string NDependProjectPath { get; set; }

        /// <summary>
        /// comma-separated list of paths to LCOV coverage report files
        /// </summary>
        [Argument("/d:sonar.javascript.lcov.reportPaths=")]
        public string JavascriptCoverageReportsPath { get; set; }

        /// <summary>
        /// comma-separated list of paths to LCOV coverage report files
        /// </summary>
        [Argument("/d:sonar.typescript.lcov.reportPaths=")]
        public string TypescriptCoverageReportsPath { get; set; }

        [Argument("/d:sonar.resharper.cs.reportPath=")]
        public string ResharperProjectPath { get; set; }

        [Argument("/d:sonar.resharper.solutionFile=")]
        public string ResharperSolutionFile { get; set; }

        [Argument("/d:sonar.analysis.mode=")]
        public string AnalysisMode { get; set; }

        [Argument("/d:sonar.useWsCache=")]
        public bool? UseWsCache { get; set; }

        [Argument("/d:sonar.issuesReport.html.enable=")]
        public bool? IssuesReportHtmlEnable { get; set; }

        [Argument("/d:sonar.issuesReport.console.enable=")]
        public bool? IssuesReportConsoleEnable { get; set; }

        [Argument("/d:sonar.language=")]
        public string Language { get; set; }

        /// <summary>
        /// Use this property when you need analysis to take place in a directory other than the one from which it was launched
        /// </summary>
        [Argument("/d:sonar.projectBaseDir=")]
        public string ProjectBaseDir { get; set; }

        #region GitHub integration, see https://docs.sonarqube.org/display/PLUG/GitHub+Plugin
        [Argument("/d:sonar.github.pullRequest=")]
        public string GitHubPullRequest { get; set; }

        [Argument("/d:sonar.github.repository=")]
        public string GitHubRepository { get; set; }

        [Argument("/d:sonar.github.login=", Secure = true)]
        public string GitHubLogin { get; set; }

        [Argument("/d:sonar.github.oauth=", Secure = true)]
        public string GitHubOAuth { get; set; }

        [Argument("/d:sonar.github.endpoint=")]
        public string GitHubEndpoint { get; set; }

        [Argument("/d:sonar.github.deleteOldComments=")]
        public bool? GitHubDeleteOldComments { get; set; }
        #endregion

        #region Quality Gate
        /// <summary>
        /// Forces the analysis step to poll the SonarQube instance and wait for the Quality Gate status
        /// </summary>
        [Argument("/d:sonar.qualitygate.wait=")]
        public bool? QualityGateWait { get; set; }

        /// <summary>
        /// Sets the number of seconds that the scanner should wait for a report to be processed
        /// </summary>
        [Argument("/d:sonar.qualitygate.timeout=")]
        public int? QualityGateTimeout { get; set; }
        #endregion

        /// <summary>
        /// This property accepts one or more JSON TSLint reports,
        /// paths to report files should be absolute or relative to the project base directory.
        /// </summary>
        [Argument("/d:sonar.typescript.tslint.reportPaths=")]
        public string TSLintReportPaths { get; set; }

        /// <summary>
        /// Gets or sets the pull request provider used by sonarcloud. github or vsts.
        /// See: https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis 
        /// </summary>
        [Argument("/d:sonar.pullrequest.provider=")]
        public string PullRequestProvider { get; set; }

        /// <summary>
        /// Gets or sets the branch name in case of a pull request validation
        /// See https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis
        /// </summary>
        [Argument("/d:sonar.pullrequest.branch=")]
        public string PullRequestBranch { get; set; }

        /// <summary>
        /// Gets or sets the pullrequest key in case of a pull request validation
        /// See https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis
        /// </summary>
        [Argument("/d:sonar.pullrequest.key=")]
        public int? PullRequestKey { get; set; }

        /// <summary>
        /// Gets or sets the base branch in which the pull request will be merged in case of a pull request validation
        /// See https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis
        /// </summary>
        [Argument("/d:sonar.pullrequest.base=")]
        public string PullRequestBase { get; set; }

        /// <summary>
        /// Gets or sets the github endpoint url.
        /// See: https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis 
        /// </summary>
        [Argument("/d:sonar.pullrequest.github.endpoint=")]
        public string PullRequestGithubEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the github repository for which the pull request should be validated. Typical github format CompanyOrUser/RepositoryName
        /// See: https://docs.sonarqube.org/display/SONAR/Pull+Request+Analysis
        /// </summary>
        [Argument("/d:sonar.pullrequest.github.repository=")]
        public string PullRequestGithubRepository { get; set; }

        /// <summary>
        /// Gets or sets the VSTS/Azure DevOps endpoint url.
        /// </summary>
        [Argument("/d:sonar.pullrequest.vsts.instanceUrl=")]
        public string PullRequestVstsEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the VSTS/Azure DevOps project.
        /// </summary>
        [Argument("/d:sonar.pullrequest.vsts.project=")]
        public string PullRequestVstsProject { get; set; }

        /// <summary>
        /// Gets or sets the VSTS/Azure DevOps repository.
        /// </summary>
        [Argument("/d:sonar.pullrequest.vsts.repository=")]
        public string PullRequestVstsRepository { get; set; }

        /// <summary>
        /// Print verbose output during the analysis.
        /// </summary>
        public bool Verbose { get; set; }

        public override ProcessArgumentBuilder GetArguments(ICakeEnvironment environment)
        {
            var args = new ProcessArgumentBuilder();
            args.Append("begin");
            AppendArguments(args, environment);

            if (Verbose)
            {
                args.Append("/d:sonar.verbose=true");
            }

            return args;
        }

        public virtual SonarEndSettings GetEndSettings()
        {
            return new SonarEndSettings()
            {
                Login = this.Login,
                Password = this.Password,
                Silent = this.Silent
            };
        }
    }
}
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    /// <summary>
    /// Required prior to Sonar 6.1:
    ///  - Name
    ///  - Version
    /// Required from Sonar 6.1
    ///  - Key
    /// </summary>
    public class SonarBeginSettings
    {
        /// <summary>
        /// The url of the used sonar instance. When omitted, http://localhost:9000 is used.
        /// </summary>
        [Argument("/d:sonar.host.url=")]
        public string Url { get; set; }

        /// <summary>
        /// Login to use for connecting to sonar.
        /// </summary>
        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        /// <summary>
        /// Password to use for connecting to sonar.
        /// </summary>
        [SecretArgument("/d:sonar.password=")]
        public string Password { get; set; }

        /// <summary>
        /// Key of the project. Required for Sonar 6.1 and up.
        /// </summary>
        [Argument("/k:")]
        public string Key { get; set; }

        /// <summary>
        /// Name of the project.
        /// Required prior to Sonar 6.1
        /// </summary>
        [Argument("/n:")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the current branch. Specifying a branch will cause Sonary to analyse different branches as different Sonar projects.
        /// This allows one to use sonar to compare branches on a pull review.
        /// </summary>
        [Argument("/d:sonar.branch=")]
        public string Branch { get; set; }

        /// <summary>
        /// Use exclusion to analyze everything but the specified files
        /// </summary>
        [Argument("/d:sonar.exclusions=")]
        public string Exclusions { get; set; }

        /// <summary>
        /// A version indicator, e.g. a semantic version or git revision hash.
        /// Required prior to Sonar 6.1
        /// </summary>
        [Argument("/v:")]
        public string Version{ get; set; }

        [Argument("/d:sonar.cs.nunit.reportsPaths=")]
        public string NUnitReportsPath { get; set; }

        [Argument("/d:sonar.cs.xunit.reportsPaths=")]
        public string XUnitReportsPath { get; set; }

        [Argument("/d:sonar.cs.vstest.reportsPaths=")]
        public string VsTestReportsPath { get; set; }

        [Argument("/d:sonar.cs.dotcover.reportsPaths=")]
        public string DotCoverReportsPath { get; set; }

        [Argument("/d:sonar.cs.opencover.reportsPaths=")]
        public string OpenCoverReportsPath { get; set; }

        [Argument("/d:sonar.cs.vscoveragexml.reportsPaths=")]
        public string VsCoverageReportsPath { get; set; }

        [Argument("/d:sonar.cs.ncover3.reportsPaths=")]
        public string NCover3ReportsPath { get; set; }

        /// <summary>
        /// Print verbose output during the analysis.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Suppress standard output from the sonar runner.
        /// </summary>
        public bool Silent { get; set; }
    }
}
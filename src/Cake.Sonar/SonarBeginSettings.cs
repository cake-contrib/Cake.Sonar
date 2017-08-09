using Cake.Core;
using Cake.Core.IO;
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
    public class SonarBeginSettings : SonarSettings
    {
        /// <summary>
        /// The url of the used sonar instance. When omitted, http://localhost:9000 is used.
        /// </summary>
        [Argument("/d:sonar.host.url=")]
        public string Url { get; set; }

        /// <summary>
        /// Path to alternative SonarQube.Analysis.xml
        /// </summary>
        [Argument("/s:")]
        public FilePath SettingsFile { get; set; }

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
        /// A version indicator, e.g. a semantic version or git revision hash.
        /// Required prior to Sonar 6.1
        /// </summary>
        [Argument("/v:")]
        public string Version { get; set; }

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

        [Argument("/d:sonar.cs.ndepend.projectPath=")]
        public string NDependProjectPath { get; set; }

        /// <summary>
        /// Print verbose output during the analysis.
        /// </summary>
        public bool Verbose { get; set; }

        public override ProcessArgumentBuilder GetArguments(ICakeEnvironment environment)
        {
            var args = new ProcessArgumentBuilder();
            args.Append("begin");
            AppendArguments(this, args, environment);

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
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarBeginSettings
    {
        [Argument("/d:sonar.host.url=")]
        public string Url { get; set; }

        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        [SecretArgument("/d:sonar.password=")]
        public string Password { get; set; }

        [Argument("/k:")]
        public string Key { get; set; }

        [Argument("/n:")]
        public string Name { get; set; }

        [Argument("/d:sonar.branch=")]
        public string Branch { get; set; }

        [Argument("/v:")]
        public string Version{ get; set; }

        [Argument("/d:sonar.cs.nunit.reportsPaths=")]
        public string NUnitReportsPath { get; set; }

        [Argument("/d:sonar.cs.dotcover.reportsPaths=")]
        public string DotCoverReportsPath { get; set; }

        [Argument("/d:sonar.cs.opencover.reportsPaths=")]
        public string OpenCoverReportsPath { get; set; }

        [Argument("/d:sonar.cs.xunit.reportsPaths=")]
        public string XUnitReportsPath { get; set; }

        [Argument("/d:sonar.cs.vstest.reportsPaths=")]
        public string VsTestReportsPath { get; set; }

        public bool Verbose { get; set; }
    }
}
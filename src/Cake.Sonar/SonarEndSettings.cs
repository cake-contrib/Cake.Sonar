using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarEndSettings
    {
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
    }
}
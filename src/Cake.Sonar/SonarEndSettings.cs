using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarEndSettings
    {

        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        [SecretArgument("/d:sonar.password=")]
        public string Password { get; set; }
    }
}
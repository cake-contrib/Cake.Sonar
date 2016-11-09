namespace Cake.Sonar
{
    public class SonarEndSettings
    {

        [Argument("/d:sonar.login=")]
        public string Login { get; set; }

        [Argument("/d:sonar.password=")]
        public string Password { get; set; }
    }
}
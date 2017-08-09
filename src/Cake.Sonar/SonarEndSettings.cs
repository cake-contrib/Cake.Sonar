using Cake.Core;
using Cake.Core.IO;
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarEndSettings : SonarSettings
    {
        /// <summary>
        /// Login to use for connecting to sonar.
        /// </summary>
        [SecretArgument("/d:sonar.login=")]
        public string Login { get; set; }

        /// <summary>
        /// Password to use for connecting to sonar.
        /// </summary>
        [SecretArgument("/d:sonar.password=")]
        public string Password { get; set; }


        public override ProcessArgumentBuilder GetArguments(ICakeEnvironment environment)
        {
            var args = new ProcessArgumentBuilder();
            args.Append("end");
            AppendArguments(this, args, environment);

            return args;
        }
    }
}
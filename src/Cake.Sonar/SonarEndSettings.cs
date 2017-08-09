using Cake.Core;
using Cake.Core.IO;
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public class SonarEndSettings : SonarSettings
    {
        public override ProcessArgumentBuilder GetArguments(ICakeEnvironment environment)
        {
            var args = new ProcessArgumentBuilder();
            args.Append("end");
            AppendArguments(this, args, environment);

            return args;
        }
    }
}
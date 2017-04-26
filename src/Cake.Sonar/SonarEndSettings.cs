using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Sonar.Attributes;

namespace Cake.Sonar
{
    public abstract class SonarSettings : ToolSettings
    {
        /// <summary>
        /// Suppress standard output from the sonar runner.
        /// </summary>
        public bool Silent { get; set; }

        public abstract ProcessArgumentBuilder GetArguments();


        public void AppendArguments(object s, ProcessArgumentBuilder builder)
        {
            foreach (var pi in s.GetType().GetProperties())
            {
                AppendArguments(s, builder, pi);
                AppendSecretArguments(s, builder, pi);
            }
        }

        private static void AppendArguments(object s, ProcessArgumentBuilder builder, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttributes<ArgumentAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var value = pi.GetValue(s);
                if (value != null)
                {
                    builder.Append($"{attr.Name}{pi.GetValue(s)}");
                }
            }
        }

        private static void AppendSecretArguments(object s, ProcessArgumentBuilder builder, PropertyInfo pi)
        {
            var attr = pi.GetCustomAttributes<SecretArgumentAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var value = pi.GetValue(s);
                if (value != null)
                {
                    builder.AppendSwitchSecret(attr.Name, "", pi.GetValue(s).ToString());
                }
            }
        }
    }

    public class SonarEndSettings : SonarSettings
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


        public override ProcessArgumentBuilder GetArguments()
        {
            var args = new ProcessArgumentBuilder();
            args.Append("end");
            AppendArguments(this, args);

            return args;
        }
    }
}
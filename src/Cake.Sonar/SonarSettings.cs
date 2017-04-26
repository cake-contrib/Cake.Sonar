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

        public abstract ProcessArgumentBuilder GetArguments(ICakeEnvironment environment);


        public void AppendArguments(object s, ProcessArgumentBuilder builder, ICakeEnvironment environment)
        {
            foreach (var pi in s.GetType().GetProperties())
            {
                AppendArguments(s, builder, pi, environment);
                AppendSecretArguments(s, builder, pi);
            }
        }

        private static void AppendArguments(object s, ProcessArgumentBuilder builder, PropertyInfo pi, ICakeEnvironment environment)
        {
            var attr = pi.GetCustomAttributes<ArgumentAttribute>().FirstOrDefault();
            if (attr != null)
            {
                var value = pi.GetValue(s);
                if (value != null)
                {
                    var stringValue = pi.GetValue(s).ToString();

                    var filePath = value as FilePath;
                    if (filePath != null)
                        stringValue = filePath.MakeAbsolute(environment).FullPath;

                    builder.Append($"{attr.Name}{stringValue}");
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
}
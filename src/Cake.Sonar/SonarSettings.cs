using System;
using System.Collections;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Sonar.Attributes;
using System.Linq;
using System.Reflection;

namespace Cake.Sonar
{
    public abstract class SonarSettings : ToolSettings
    {
        /// <summary>
        /// Suppress standard output from the sonar runner.
        /// </summary>
        public bool Silent { get; set; }

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

        public abstract ProcessArgumentBuilder GetArguments(ICakeEnvironment environment);

        public void AppendArguments(SonarSettings s, ProcessArgumentBuilder builder, ICakeEnvironment environment)
        {
            foreach (var pi in s.GetType().GetProperties())
            {
                AppendArguments(s, builder, pi, environment);
                AppendSecretArguments(s, builder, pi);
            }

            // Append custom arguments (if any)
            if (s.ArgumentCustomization != null)
            {
                builder = s.ArgumentCustomization(builder);
                // Reset the customization as it is now already applied
                s.ArgumentCustomization = null;
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
                    var stringValue = pi.GetValue(s).ToString().Quote();
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

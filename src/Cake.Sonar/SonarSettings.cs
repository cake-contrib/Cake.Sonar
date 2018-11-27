using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Sonar.Attributes;
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
        [Argument("/d:sonar.login=", Secure = true)]
        public string Login { get; set; }

        /// <summary>
        /// Password to use for connecting to sonar.
        /// </summary>
        [Argument("/d:sonar.password=", Secure = true)]
        public string Password { get; set; }
		
		/// <summary>
        /// Use .NET Core version of the SonarQube scanner in case of a .NET Core build.
        /// </summary>
        public bool UseCoreClr { get; set; }

        public abstract ProcessArgumentBuilder GetArguments(ICakeEnvironment environment);

        public void AppendArguments(ProcessArgumentBuilder builder, ICakeEnvironment environment)
        {
            foreach (var pi in this.GetType().GetProperties())
            {
                AppendArguments(builder, pi, environment);
            }

            // Append custom arguments (if any)
            if (this.ArgumentCustomization != null)
            {
                builder = this.ArgumentCustomization(builder);
                // Reset the customization as it is now already applied
                this.ArgumentCustomization = null;
            }
        }

        private void AppendArguments(ProcessArgumentBuilder builder, PropertyInfo pi, ICakeEnvironment environment)
        {
            var attrs = pi.GetCustomAttributes<ArgumentAttribute>();
            foreach (var attr in attrs)
            {
                var value = pi.GetValue(this);
                if (value != null) {
                    attr.Apply(builder, ConvertToString(value), this);
                }
            }
        }

        private string ConvertToString(object value) {
            return value is bool 
                ? value.ToString().ToLowerInvariant() 
                : value.ToString();
        }
    }
}

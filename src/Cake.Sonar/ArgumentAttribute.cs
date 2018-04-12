using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Sonar.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ArgumentAttribute : Attribute
    {
        private Version _toExcludingVersion;
        private Version _fromVersion;

        public bool Secure { get; set; }

        public string ToExcluding { 
            set {
                if( value != null )
                    _toExcludingVersion = Version.Parse(value);
            }

            get {
                return _toExcludingVersion?.ToString();
            }
        }

        public string From { set {
                if( value != null )
                    _fromVersion = Version.Parse(value);
            }
            get {
                return _fromVersion?.ToString();
            }
        }

        public string Name { get; set; }

        public bool Default { get; set; }

        public ArgumentAttribute(string name, bool isDefault = true) 
        {
            Name = name;
            Default = isDefault;
        }

        public void Apply(ProcessArgumentBuilder builder, string value, SonarSettings settings)
        {
            var beginSettings = settings as SonarBeginSettings;

            var version = GetVersion(beginSettings);

            if (Match(version))
            {
                if(Secure)
                {
                    builder.AppendSwitchQuotedSecret(Name, string.Empty, value);
                }
                else {
                    builder.AppendSwitchQuoted(Name, string.Empty, value);
                }
            }
        }


        public bool Match(Version version) {
            if (version == null)
                return Default;

            return ((_fromVersion == null || version >= _fromVersion) && (_toExcludingVersion == null || version < _toExcludingVersion));
        }

        private Version GetVersion(SonarBeginSettings settings) {
            return settings?.VersionResult?.Version;
        }
    }

}

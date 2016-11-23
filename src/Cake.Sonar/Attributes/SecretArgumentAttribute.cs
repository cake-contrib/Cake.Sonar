using System;

namespace Cake.Sonar.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SecretArgumentAttribute : Attribute
    {
        public string Name { get; set; }

        public SecretArgumentAttribute(string name)
        {
            Name = name;
        }
    }
}

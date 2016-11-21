using System;

namespace Cake.Sonar.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute 
    {
        public string Name { get; set; }

        public ArgumentAttribute(string name)
        {
            Name = name;
        }   
           
    }
}
 
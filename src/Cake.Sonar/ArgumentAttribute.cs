using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Sonar
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
 
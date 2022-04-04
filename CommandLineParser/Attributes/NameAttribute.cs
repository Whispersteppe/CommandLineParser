using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : Attribute, ICommandLineAttribute
    {
        public string Name { get; set; }
        
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute, ICommandLineAttribute
    {
        public string Description { get; set; }
        
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}

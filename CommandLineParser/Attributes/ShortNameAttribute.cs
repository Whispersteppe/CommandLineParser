using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShortNameAttribute : Attribute, ICommandLineAttribute
    {
        public char Name { get; set; }
        public ShortNameAttribute(char name)
        {
            Name = name;
        }

    }
}

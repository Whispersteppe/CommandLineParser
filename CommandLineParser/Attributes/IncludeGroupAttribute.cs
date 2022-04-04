using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IncludeGroupAttribute : Attribute, ICommandLineAttribute
    {
        public List<string> IncludeGroups { get; set; }
        public IncludeGroupAttribute(params string[] groups)
        {
            IncludeGroups = new List<string>(groups);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeGroupAttribute : Attribute, ICommandLineAttribute
    {
        public List<string> ExcludeGroups { get; set; }

        public ExcludeGroupAttribute(params string[] groups)
        {
            ExcludeGroups = new List<string>(groups);
        }
    }
}

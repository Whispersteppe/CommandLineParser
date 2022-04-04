using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedValuesAttribute : Attribute, ICommandLineAttribute
    {
        public List<string> AllowedValues { get; set; }

        public AllowedValuesAttribute(params string[] values)
        {
            AllowedValues = new List<string>(values);
        }
    }
}

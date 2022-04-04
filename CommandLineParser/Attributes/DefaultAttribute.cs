using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute, ICommandLineAttribute
    {
        public object Value { get; set; }
        public DefaultAttribute(object value)
        {
            Value = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Arguments
{
    /// <summary>
    /// an instance of an argument
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// the name of the argument
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// the value or set of values for the argument
        /// </summary>
        public string Value { get; set; } = "";

    }
}

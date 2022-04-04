using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public class CommandParserOptions
    {
        /// <summary>
        /// string that will start off any argument
        /// </summary>
        public string ArgStart { get; set; } = "/";

        /// <summary>
        /// if there is a separater between the argument name and the value
        /// </summary>
        public string NameValueSeparator { get; set; } = " ";

        /// <summary>
        /// if there is a list of items in an argument, this is the separator
        /// </summary>
        public string ListSeparator { get; set; } = " ";

    }
}

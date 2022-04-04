using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Arguments
{
    public class ArgumentSet
    {

        public List<Argument> Arguments { get; set; }
        /// <summary>
        /// split the arguments into Argument instances
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ArgumentSet(List<string> args, CommandParserOptions options)
        {
            //  split on the options arg start character
            Arguments = new List<Argument>();

            Argument? currentArgument = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith(options.ArgStart))
                {
                    //  starting a new argument.  save the previous one
                    if (currentArgument != null)
                    {
                        Arguments.Add(currentArgument);
                    }
                    currentArgument = new Argument();

                    if (string.IsNullOrEmpty(options.NameValueSeparator))
                    {
                        //  we don't worry - args will already be separated
                        currentArgument.Name = arg.Substring(options.ArgStart.Length); //  start it after the ArgStart
                    }
                    else
                    {
                        //  must separate by the name/value character
                        if (arg.Contains(options.NameValueSeparator))
                        {
                            var substrs = arg.Split(options.NameValueSeparator);
                            currentArgument.Name = substrs[0].Substring(options.ArgStart.Length);
                            currentArgument.Value = substrs[1];
                        }
                        else
                        {
                            //  no separator.  no worries
                            currentArgument.Name = arg.Substring(options.ArgStart.Length); //  start it after the ArgStart
                        }
                    }


                }
                else
                {
                    if (currentArgument != null)
                    {
                        if (string.IsNullOrEmpty(currentArgument.Value))
                        {
                            currentArgument.Value = arg;
                        }
                        else
                        {
                            //  moosh it onto the value
                            currentArgument.Value = String.Join(' ', currentArgument.Value, arg);
                        }
                    }
                }
            }

            if (currentArgument != null)
            {
                Arguments.Add(currentArgument);
            }

        }
    }
}

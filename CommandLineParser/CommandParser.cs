using CommandLineParser.Arguments;
using CommandLineParser.Definitions;
using CommandLineParser.Fluent;
using CommandLineParser.Utility;

namespace CommandLineParser
{
    public class CommandParser<TArgsClass> where TArgsClass : new()
    {

        #region Properties

        /// <summary>
        /// the current set of options
        /// </summary>
        public CommandParserOptions Options { get; set; }

        /// <summary>
        /// the command line argument class
        /// </summary>
        public TArgsClass CommandLineData { get; set; }

        public DefinitionSet<TArgsClass> Definitions { get; set; }
        public ArgumentSet Arguments { get; set; }
        /// <summary>
        /// the set of messages generated while parsing the command line
        /// </summary>
        public MessageLogger Messages { get; set; } = new MessageLogger();

        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="args"></param>
        /// <param name="options"></param>
        public CommandParser(string[] args, CommandParserOptions? options = null)
            : this(new List<string>(args), options)
        {
        }

        /// <summary>
        /// another constructor
        /// </summary>
        /// <param name="args"></param>
        /// <param name="options"></param>
        public CommandParser(List<string> args, CommandParserOptions? options = null)
        {
            Options = options ?? new CommandParserOptions();

            Arguments = new ArgumentSet(args, Options);

            //  build out the definitions
            Definitions = new DefinitionSet<TArgsClass>(Messages);


            CommandLineData = new TArgsClass();

            Definitions.ApplyDefinition(Arguments, CommandLineData);

        }

        #endregion

        #region Fluent componentry


        public static CommandParserBuilder<T> Build<T>() where T : new()
        {
            CommandParserBuilder<T> builder = new();

            return builder;
        }

        public static CommandParserBuilder<T> Build<T>(string[] args) where T : new()
        {
            CommandParserBuilder<T> builder = new();
            builder.WithArgs(args);

            return builder;
        }


        public static CommandParserBuilder<T> Build<T>(List<string> args) where T : new()
        {
            CommandParserBuilder<T> builder = new();
            builder.WithArgs(args);

            return builder;
        }


        #endregion


    }
}
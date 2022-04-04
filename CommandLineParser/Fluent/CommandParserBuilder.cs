using CommandLineParser.Arguments;
using CommandLineParser.Definitions;
using CommandLineParser.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Fluent
{
    public class CommandParserBuilder<TArgsClass> where TArgsClass : new()
    {

        public delegate bool ValidatorHandler(DefinitionSet<TArgsClass> definitions, ArgumentSet arguments, TArgsClass data);
        public delegate bool ActionHandler(DefinitionSet<TArgsClass> definitions, ArgumentSet arguments, TArgsClass data);
        public delegate bool DefaultParsedHandler(TArgsClass data);
        public delegate bool ErrorHandler(DefinitionSet<TArgsClass> definitions, ArgumentSet arguments, MessageLogger logger);


        List<string> _args = new();
        CommandParserOptions _options = new();

        #region handlers
        ValidatorHandler? _validatorHandler;
        DefaultParsedHandler? _defaultParsedHandler;
        ErrorHandler? _errorHandler;
        readonly Dictionary<string, ActionHandler> _actionHandlers = new();
        #endregion

        public CommandParserBuilder<TArgsClass> WithArgs(List<string> args)
        {
            _args = args;
            return this;
        }

        public CommandParserBuilder<TArgsClass> WithArgs(string[] args)
        {
            return WithArgs(new List<string>(args));
        }

        public CommandParserBuilder<TArgsClass> WithOptions(CommandParserOptions options)
        {
            _options = options;
            return this;
        }

//        public CommandParserBuilder<TArgsClass> WithValidator(Func<DefinitionSet<TArgsClass>, ArgumentSet, TArgsClass, bool> validationHandler)
        public CommandParserBuilder<TArgsClass> WithValidator(ValidatorHandler validatorHandler)
        {
            _validatorHandler = validatorHandler;
            return this;
        }

        public CommandParserBuilder<TArgsClass> WithActionHandler(string actionName, ActionHandler actionHandler)
        {
            _actionHandlers.Add(actionName, actionHandler);
            return this;
        }

        public CommandParserBuilder<TArgsClass> WithParsed(DefaultParsedHandler handler)
        {
            _defaultParsedHandler = handler;
            return this;
        }

        public CommandParserBuilder<TArgsClass> WithErrors(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        public CommandParser<TArgsClass> Parse()
        {
            CommandParser<TArgsClass> parser = new(_args, _options);

            if (parser.Messages.HasErrors)
            {
                _errorHandler?.Invoke(parser.Definitions, parser.Arguments, parser.Messages);
                return parser;
            }
            else
            {
                //  do any extra validations
                _validatorHandler?.Invoke(parser.Definitions, parser.Arguments, parser.CommandLineData);
                if (parser.Messages.HasErrors)
                {
                    _errorHandler?.Invoke(parser.Definitions, parser.Arguments, parser.Messages);
                    return parser;
                }
                else
                {
                    bool actionUsed = false;
                    //TODO need to figure out actions.
                    //  right now they are tied to a property.  they can still be so.
                    
                    foreach(var argument in parser.Arguments.Arguments)
                    {
                        if (parser.Definitions.IsAction(argument.Name) == true)
                        {
                            if (_actionHandlers.ContainsKey(argument.Name))
                            {
                                _actionHandlers[argument.Name](parser.Definitions, parser.Arguments, parser.CommandLineData);
                                actionUsed = true;
                            }
                        }
                    }


                    if (actionUsed == false)
                    {
                        _defaultParsedHandler?.Invoke(parser.CommandLineData);
                    }
                }
            }

            return parser;
        }

    }
}

using CommandLineParser.Arguments;
using CommandLineParser.Attributes;
using CommandLineParser.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Definitions
{
    public class DefinitionSet<TArgClass>
    {
        public List<PropertyDefinition> Properties { get; set; }
        readonly MessageLogger _logger;



        public DefinitionSet(MessageLogger logger)
        {
            _logger = logger;
            Properties = new List<PropertyDefinition>();    

            var properties = typeof(TArgClass).GetProperties();
            foreach (var property in properties)
            {
                PropertyDefinition definition = new(property, _logger);

                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is ICommandLineAttribute optionAttribute)
                    {
                        definition.Attributes.Add(optionAttribute);
                    }
                }

                Properties.Add(definition);
            }
        } //  end constructor

        public List<T> GetAttributes<T>(PropertyDefinition property) where T: ICommandLineAttribute
        {
            List<T> attributes = new List<T>();

            foreach (var attribute in property.Attributes)
            {
                if (attribute is T attr)
                {
                    attributes.Add(attr);
                }
            }

            return attributes;
        }
        #region Help Display

        public void DisplayHelp()
        {
            foreach(var property in Properties)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(SetShortName(property));
                sb.Append("\t");

                sb.Append(SetName(property));
                sb.Append("\t");

                sb.Append(SetDescription(property));
                sb.Append("\t");

                //TODO  we'll want more here later on.  but for now this will get us there
                Console.WriteLine(sb);
                
            }
        }

        private string SetShortName(PropertyDefinition property)
        {
            StringBuilder sb = new StringBuilder();

            //  find any short names
            List<char> shortNames = new List<char>();
            foreach (var shortName in GetAttributes<ShortNameAttribute>(property))
            {
                shortNames.Add(shortName.Name);
            }
            sb.Append(string.Join(',', shortNames));

            return sb.ToString();

        }

        private string SetName(PropertyDefinition property)
        {
            StringBuilder sb = new StringBuilder();

            //  find any names
            List<string> names = new List<string>();
            foreach (var name in GetAttributes<NameAttribute>(property))
            {
                names.Add(name.Name);
            }
            sb.Append(string.Join(',', names));

            return sb.ToString();

        }

        private string SetDescription(PropertyDefinition property)
        {
            StringBuilder sb = new StringBuilder();

            //  find any descriptions
            List<string> names = new List<string>();
            foreach (var name in GetAttributes<DescriptionAttribute>(property))
            {
                names.Add(name.Description);
            }
            sb.Append(string.Join(',', names));

            return sb.ToString();

        }


        #endregion

        public void ApplyDefinition(ArgumentSet arguments, TArgClass data)
        {
            //  see if we have a help argument out there
            foreach(var arg in arguments.Arguments)
            {
                if (arg.Name == "?" || arg.Name == "help")
                {
                    DisplayHelp();
                    return;
                }
            }


            foreach (var property in Properties)
            {
                if (data != null) property.SetValue(data, arguments);
                else _logger.LogError($"{property.Property.Name}: data is null");

            }
        }   //end ApplyDefinitions


        public bool IsAction(string actionName)
        {

            foreach (var property in Properties)
            {
                foreach (var attr in property.Attributes)
                {
                    if (attr is ActionAttribute)
                    {
                        if (property.Names.Contains(actionName)) return true;
                    }
                }

            }

            return false;
        }


    } //  end class
}

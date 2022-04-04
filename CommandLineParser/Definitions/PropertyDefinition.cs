using CommandLineParser.Arguments;
using CommandLineParser.Attributes;
using CommandLineParser.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Definitions
{
    public class PropertyDefinition
    {
        public PropertyInfo Property { get; set; }
        public List<ICommandLineAttribute> Attributes { get; set; } = new List<ICommandLineAttribute>();
        readonly MessageLogger _logger;


        public PropertyDefinition(PropertyInfo property, MessageLogger logger)
        {
            Property = property;
            _logger = logger;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public List<string> Names
        {
            get
            {
                List<string> list = new();

                foreach (var attribute in Attributes)
                {
                    if (attribute is NameAttribute attrName)
                    {
                        list.Add(attrName.Name);
                    }
                    else if (attribute is ShortNameAttribute attrShort)
                    {
                        list.Add(attrShort.Name.ToString());
                    }
                    else if (attribute is AlternateNameAttribute attrAlternate)
                    {
                        list.Add(attrAlternate.Name);
                    }
                }

                return list;
            }
        }

        public bool IsRequired
        {
            get
            {
                foreach (var attribute in Attributes)
                {
                    if (attribute is RequiredAttribute) return true;
                }

                return false;
            }
        }

        public bool HasDefaultValue
        {
            get
            {
                foreach (var attribute in Attributes)
                {
                    if (attribute is DefaultAttribute) return true;
                }

                return false;
            }
        }
        public object DefaultValue
        {
            get
            {
                foreach (var attribute in Attributes)
                {
                    if (attribute is DefaultAttribute attrRequired) return attrRequired.Value;
                }

                return null;
            }
        }


        private List<Argument> GetMatchingArguments(ArgumentSet arguments)
        {
            List<Argument> matchingArguments = arguments.Arguments.Where(o => Names.Contains(o.Name)).ToList();

            return matchingArguments;
        }

        private bool IsAllowedValue(string valueToCheck)
        {
            foreach (var attribute in Attributes)
            {
                if (attribute is AllowedValuesAttribute allowedValues)
                {
                    return allowedValues.AllowedValues.Contains(valueToCheck);
                }
            }

            return true;
        }

        private bool InitializeValue(object argumentData)
        {
            if (Property.PropertyType.IsPrimitive == true ||
                Property.PropertyType == typeof(string) ||
                Property.PropertyType == typeof(DateTime))
            {
                //  we're good
            }
            else if (Property.PropertyType.IsArray == true)
            {
                // NO ARRAYS (yet)

                _logger.LogError($"{Property.Name} no arrays - use List<{Property.PropertyType.GetElementType().Name}> instead");
                return false;

                ////  this may be salvageable
                //Type elementType = property.PropertyType.GetElementType();
                //if (elementType.IsPrimitive || elementType == typeof(string))
                //{
                //    //  we're good
                //}
                //else
                //{
                //    Messages.LogError($"{property.Name} is not a primitive or array of primitives");
                //    return false;
                //}
            }
            else if (typeof(IList).IsAssignableFrom(Property.PropertyType))
            {
                Type underlyingType = Property.PropertyType.IsGenericType == true
                    ? Property.PropertyType.GenericTypeArguments[0]
                    : Property.PropertyType;

                if (underlyingType.IsPrimitive == true ||
                        underlyingType == typeof(string) ||
                        underlyingType == typeof(DateTime))
                {
                }
                else
                {
                    //  we can't do lists of non-primitive types
                    _logger.LogError($"{Property.Name} is not a primitive or list of primitives");
                    return false;
                }

                Property.SetValue(argumentData, Activator.CreateInstance(Property.PropertyType));
                return true;

            }
            else if (Property.PropertyType.IsClass == true)
            {
                _logger.LogError($"{Property.Name} is not a primitive or list of primitives");
                return false;
            }

            //TODO may have to do this with an array, too.

            return true;
        }

        /// <summary>
        /// set a value to the command argument instance
        /// </summary>
        /// <param name="argumentData"></param>
        /// <param name="property"></param>
        /// <param name="argumentSet"></param>
        public void SetValue(object argumentData, ArgumentSet argumentSet)
        {
            InitializeValue(argumentData);


            //  get the arguments that match items in the name list
            var matchingArguments = GetMatchingArguments(argumentSet);
            if (matchingArguments.Any() == false)
            {
                if (HasDefaultValue)
                {
                    matchingArguments.Add(new Argument() { Name = "", Value = DefaultValue.ToString() });
                }
                else
                {
                    //  we can't set what doesn't exist. exit.
                    //todo need to log if required

                    return;
                }
            }

            if (Property.PropertyType.IsPrimitive == true ||
                Property.PropertyType == typeof(string) ||
                Property.PropertyType == typeof(DateTime))
            {
                if (IsAllowedValue(matchingArguments[0].Value))
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(matchingArguments[0].Value, Property.PropertyType);
                        Property.SetValue(argumentData, convertedValue);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"{Property.Name} Cannot set the value '{matchingArguments[0].Value}'. {ex.Message}");
                    }
                }
                else
                {
                    _logger.LogError($"{Property.Name} value '{matchingArguments[0].Value} is not an allowed value.");
                }

            }
            else if (Property.PropertyType.IsArray == true)
            {
                //  not sure what to do here yet

            }
            else if (typeof(IList).IsAssignableFrom(Property.PropertyType))
            {
                Type underlyingType = Property.PropertyType.IsGenericType == true
                    ? Property.PropertyType.GenericTypeArguments[0]
                    : Property.PropertyType;
                foreach(var argument in matchingArguments)
                {

                    if (IsAllowedValue(argument.Value))
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(argument.Value, underlyingType);
                            _ = Property.PropertyType.GetMethod("Add").Invoke(Property.GetValue(argumentData), new[] { convertedValue });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"{Property.Name} Cannot set the value '{matchingArguments[0].Value}'. {ex.Message}");
                        }

                    }
                    else
                    {
                        _logger.LogError($"{Property.Name} value '{argument.Value} is not an allowed value.");
                    }



                }

            }
            else if (Property.PropertyType.IsClass == true)
            {
                _logger.LogError($"{Property.Name} is not a primitive or list of primitives");
            }

        } //  end SetValue
    }
}

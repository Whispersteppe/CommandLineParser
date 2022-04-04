using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser.Attributes
{
    public interface ICommandLineAttribute
    {
        //todo we might want some validator logic in here, so that each attribute does it's own local validation

        //todo may want some class level attributes that can be applied to a set of attributes, or information at the class level (such as description)
    }
}

using CommandLineParser.Attributes;
using CommandLineParser.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CommandLineParser.Test
{
    public class TestTheUglies
    {

        readonly ITestOutputHelper _output;

        public TestTheUglies(ITestOutputHelper output)
        {
            _output = output;
        }

        internal void DumpParserMessages(MessageLogger messages)
        {
            foreach (var message in messages.Messages)
            {
                _output.WriteLine(message.ToString());
            }
        }

        internal void DumpObject(object o)
        {
            string data = JsonConvert.SerializeObject(o, Formatting.Indented);
            _output.WriteLine(data);
        }



        [Fact]
        public void CommandLineTest()
        {
            String[] cmdLine =
             {
                "/s", "this is string data",
            };


            CommandParser<UglyCrashesTestData> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


        }



    }  //  end class


    public class UglyCrashesTestData
    {
        [Name("string")]
        [ShortName('s')]
        [Description("this is a desription")]
        [Default("byork")]
        public int StringData { get; set; }
    }


}
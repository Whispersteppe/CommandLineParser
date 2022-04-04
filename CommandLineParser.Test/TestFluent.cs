using CommandLineParser.Attributes;
using CommandLineParser.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CommandLineParser.Test
{
    public class TestFluent
    {

        readonly ITestOutputHelper _output;

        public TestFluent(ITestOutputHelper output)
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
                "/int", "22",
                "/d", "1/27/1965",
                "/b", "true",
                "/l", "123456",
                "/h", "12"
            };


            var parser = CommandParser<CommandLinePrimitive>.Build<CommandLinePrimitive>()
                .WithArgs(cmdLine)
                .Parse();

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);

            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);
            Assert.Equal("this is string data", parser.CommandLineData.StringData);
            Assert.Equal(22, parser.CommandLineData.IntData);
            Assert.Equal(new DateTime(1965, 1, 27), parser.CommandLineData.DateData);
            Assert.Equal(12, parser.CommandLineData.ShortData);
            Assert.Equal(123456, parser.CommandLineData.LongData);
            Assert.True(parser.CommandLineData.Boolean);
        }

        [Fact]
        public void CommandLineTestWithParsed()
        {
            String[] cmdLine =
             {
                "/s", "this is string data",
                "/int", "22",
                "/d", "1/27/1965",
                "/b", "true",
                "/l", "123456",
                "/h", "12"
            };


            var parser = CommandParser<CommandLinePrimitive>.Build<CommandLinePrimitive>()
                .WithArgs(cmdLine)
                .WithParsed(DefaultParsedHandler)
                .Parse();

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);

            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);
        }

        [Fact]
        public void CommandLineTestInline()
        {
            String[] cmdLine =
             {
                "/s", "this is string data",
                "/int", "22",
                "/d", "1/27/1965",
                "/b", "true",
                "/l", "123456",
                "/h", "12"
            };


            var parser = CommandParser<CommandLinePrimitive>.Build<CommandLinePrimitive>()
                .WithArgs(cmdLine)
                .WithParsed((data) => {
                    Assert.Equal("this is string data", data.StringData);
                    Assert.Equal(22, data.IntData);
                    Assert.Equal(new DateTime(1965, 1, 27), data.DateData);
                    Assert.Equal(12, data.ShortData);
                    Assert.Equal(123456, data.LongData);
                    Assert.True(data.Boolean);

                    return true;
                }

                )
                .Parse();

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);

            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);
        }


        public bool DefaultParsedHandler(CommandLinePrimitive data)
        {

            Assert.Equal("this is string data", data.StringData);
            Assert.Equal(22, data.IntData);
            Assert.Equal(new DateTime(1965, 1, 27), data.DateData);
            Assert.Equal(12, data.ShortData);
            Assert.Equal(123456, data.LongData);
            Assert.True(data.Boolean);



            return true;
            //  do something here
        }

    }  //  end class



}
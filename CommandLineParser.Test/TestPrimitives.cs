using CommandLineParser.Attributes;
using CommandLineParser.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace CommandLineParser.Test
{
    public class TestPrimitives
    {

        readonly ITestOutputHelper _output;

        public TestPrimitives(ITestOutputHelper output)
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


            CommandParser<CommandLinePrimitive> parser = new(cmdLine);

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
        public void CommandLineListTest()
        {
            String[] cmdLine =
             {
                "/s", "this","is","string","data",
                "/t", "this is more string data",
            };

            CommandParser<CommandLineDataList> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);
        }

        [Fact]
        public void CommandLineArrayTest()
        {
            String[] cmdLine =
             {
                "/s", "this","is","string","data",
                "/t", "this is more string data",
            };

            CommandParser<CommandLineDataArray> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


            Assert.True(parser.Messages.HasErrors);
            Assert.Single(parser.Messages.Messages);
            //  should be 
            Assert.Equal("StringData no arrays - use List<String> instead", parser.Messages.Messages[0].MessageData);

        }

        [Fact]
        public void CommandLineExcludeRequiredTest()
        {
            String[] cmdLine =
             {
                "/s", "this","is","string","data",
                "/t", "this is more string data",
            };

            CommandParser<CommandLineExcludeRequired> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


            //            Assert.False(parser.Messages.HasErrors);
            //            Assert.False(parser.Messages.HasWarnings);
        }


        [Fact]
        public void AllowedValueTest()
        {
            String[] cmdLine =
             {
                "/s", "asdf",
            };

            CommandParser<AllowedValueTestData> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);

            string[] cmdLine2 =
 {
                "/s", "byork",
            };

            parser = new CommandParser<AllowedValueTestData>(cmdLine2);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);


            Assert.True(parser.Messages.HasErrors);

        }

        [Fact]
        public void DefaultValueTest()
        {
            String[] cmdLine =
             {
                //"/s", "asdf",
            };

            CommandParser<DefaultValueTestData> parser = new(cmdLine);

            DumpObject(parser.CommandLineData);
            DumpParserMessages(parser.Messages);

            
            Assert.False(parser.Messages.HasErrors);
            Assert.False(parser.Messages.HasWarnings);
            Assert.Equal("byork", parser.CommandLineData.StringData);



        }

    }  //  end class


    public class DefaultValueTestData
    {
        [Name("string")]
        [ShortName('s')]
        [Description("this is a desription")]
        [Default("byork")]
        public string? StringData { get; set; }
    }

    public class AllowedValueTestData
    {
        [Name("string")]
        [Required]
        [ShortName('s')]
        [Description("this is a desription")]
        [AllowedValues("asdf", "12345")]
        public string? StringData { get; set; }
    }

        public class CommandLinePrimitive
    {
        [Name("string")]
        [Required]
        [ShortName('s')]
        [Description("this is a desription")]
        public string? StringData { get; set; }

        [Name("int")]
        [Required]
        [ShortName('i')]
        [Description("this is a desription")]
        public int IntData { get; set; }

        [Name("date")]
        [Required]
        [ShortName('d')]
        [Description("this is a desription")]
        public DateTime DateData { get; set; }

        [Name("long")]
        [Required]
        [ShortName('l')]
        [Description("this is a desription")]
        public long LongData { get; set; }

        [Name("short")]
        [Required]
        [ShortName('h')]
        [Description("this is a desription")]
        public short ShortData { get; set; }

        [Name("bool")]
        [Required]
        [ShortName('b')]
        [Description("this is a desription")]
        public bool Boolean { get; set; }

    }

    public class CommandLineDataList
    {
        [Name("string")]
        [Required]
        [ShortName('s')]
        [Description("this is a desription")]
        public List<string>? StringData { get; set; }


    }

    public class CommandLineDataArray
    {
        [Name("string")]
        [Required]
        [ShortName('s')]
        [Description("this is a desription")]
        public string[]? StringData { get; set; }

        //  how to test required and excluded items

    }




    public class CommandLineExcludeRequired
    {
        [Name("string")]
        [Required]
        [ShortName('s')]
        [Description("this is a desription")]
        [IncludeGroup("int","date")]
        [ExcludeGroup("long","short")]
        public string? StringData { get; set; }

        [Name("int")]
        [Required]
        [ShortName('i')]
        [Description("this is a desription")]
        public int IntData { get; set; }

        [Name("date")]
        [Required]
        [ShortName('d')]
        [Description("this is a desription")]
        public DateTime DateData { get; set; }

        [Name("long")]
        [Required]
        [ShortName('l')]
        [Description("this is a desription")]
        public long LongData { get; set; }

        [Name("short")]
        [Required]
        [ShortName('h')]
        [Description("this is a desription")]
        public short ShortData { get; set; }

        [Name("bool")]
        [Required]
        [ShortName('b')]
        [Description("this is a desription")]
        public bool Boolean { get; set; }

    }
}
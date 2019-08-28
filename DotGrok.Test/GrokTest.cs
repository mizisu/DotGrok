namespace DotGrok.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using DotGrok;
    using System.Text.RegularExpressions;

    public class GrokTest
    {
        ITestOutputHelper output;

        public GrokTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Get basic grok parsing result
        ///
        /// Return:
        ///
        /// time : DateTime(2018, 01, 01, 12, 32, 23, 345)
        /// 
        /// level : INFO
        /// 
        /// message : test message 1233tdsg
        /// 
        /// </summary>
        public GrokResult GetBasicParsingResult()
        {
            var grok = DotGrok.Grok.NewBuilder(@"%{DateTime:time} %{LogLevel:level} %{Message:message}")
               .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
               .AddPattern("LogLevel", @".\w+")
               .AddPattern("Message", @".+")
               .AddConverter("DateTime", s => DateTime.Parse(s))
               .SetRegexOptions(RegexOptions.Compiled | RegexOptions.IgnoreCase)
               .Build();

            var r = grok.Match("2018-01-01 12:32:23.345 INFO test message 1233tdsg");

            return r;
        }

        [Fact]
        public void BasicParsingTest()
        {
            var r = GetBasicParsingResult();

            var items = r.Items.ToList();

            Assert.Equal("time", items[0].Name);
            Assert.Equal(new DateTime(2018, 01, 01, 12, 32, 23, 345), items[0].Value);

            Assert.Equal("level", items[1].Name);
            Assert.Equal("INFO", items[1].Value);

            Assert.Equal("message", items[2].Name);
            Assert.Equal("test message 1233tdsg", items[2].Value);
        }

        [Fact]
        public void ToDictionaryTest()
        {
            var r = GetBasicParsingResult();
            var dict = r.ToDictionary();
            Assert.IsType<Dictionary<string, object>>(dict);
            Assert.True(dict.Count > 0);
        }

        [Fact]
        public void ToDynamicTest()
        {
            var r = GetBasicParsingResult();
            dynamic obj = r.ToDynamic();

            Assert.Equal("INFO", obj.level);
        }
    }
}

namespace DotGrok.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class GrokTest
    {
        ITestOutputHelper output;
        public GrokTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var grok = DotGrok.Grok.NewBuilder(@"%{DateTime:time} %{LogLevel:level} %{Message:message}")
                .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
                .AddPattern("LogLevel", @".\w+")
                .AddPattern("Message", @".+")
                .AddConverter("DateTime", s => DateTime.Parse(s))
                .Build();

            var r = grok.Match("2018-01-01 12:32:23.345 INFO test message 1233tdsg");

            var items = r.Items.ToList();

            foreach (var item in r.Items)
            {
                output.WriteLine($"{item.Name} : {item.Value}");
            }

            Assert.Equal(items[0].Name, "time");
            Assert.Equal(items[0].Value, new DateTime(2018, 01, 01, 12, 32, 23, 345));

            Assert.Equal(items[1].Name, "level");
            Assert.Equal(items[1].Value, "INFO");

            Assert.Equal(items[2].Name, "message");
            Assert.Equal(items[2].Value, "test message 1233tdsg");
        }
    }
}

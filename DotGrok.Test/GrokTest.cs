namespace DotGrok.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Xunit;
    using Xunit.Abstractions;

    public class GrokTest
    {
        private readonly ITestOutputHelper output;

        public GrokTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var grok = DotGrok.Grok.NewBuilder(@"%{DateTime:Time}")
                .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
                .AddPattern("LogLevel", @".{5}")
                .AddPattern("Message", @".")
                .AddConverter("DateTime", s => DateTime.Parse(s))
                .Build();

            var lines = new string[]{
                "2018-01-01 12:32:23.345"
                };

            foreach (var item in lines)
            {
                var r = grok.Match(item);
                Trace.WriteLine($"{r.Success}");
                foreach (var resultItem in r.Items)
                {
                    Trace.WriteLine($"{resultItem.Name} : {resultItem.Value}");
                }
            }
        }
    }
}

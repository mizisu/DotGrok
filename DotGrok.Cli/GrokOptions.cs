namespace DotGrok.Cli
{
    using CommandLine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public enum OutputType
    {
        Text,
        Csv,
        Json,
        Xml,
    }

    [Verb("run", HelpText = "Grok input.")]
    class GrokOptions : Options
    {
        [Option('i', "input", HelpText = "Input file or text.", Required = true)]
        public string Input { get; set; }

        [Option('o', "out", HelpText = "Output grok result")]
        public string Output { get; set; }

        [Option("output-type", Default = OutputType.Text, HelpText = "Output type (Text, Csv, Json, Xml)")]
        public OutputType OutputType { get; set; }

        [Value(0, HelpText ="Input grok template ex) %{Date:name1} ${Word:name2} ...")]
        public string Template { get; set; }

        public override void Run()
        {
            var builder = Grok.NewBuilder()
                .SetTemplate(this.Template)
                .AddPattern(Configuration.Grok.Patterns.Values);

            var grok = builder.Build();

            foreach (var line in this.InputToLines())
            {
                var result = grok.Match(line);
                if (result.Success)
                    WriteOutput(result);
            }

            //var grok = Grok.Create(@"%{DateTime:Time}")
            //    .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
            //    .AddPattern("LogLevel", @".{5}")
            //    .AddPattern("Message", @".")
            //    .AddConverter("DateTime", s => DateTime.Parse(s))
            //    .Build();

            //foreach (var item in new string[]
            //    {
            //        $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")}",
            //    })
            //{
            //    var r = grok.Match(item);
            //    Console.WriteLine($"{r.Success}");
            //    Console.WriteLine($"{string.Join(',', r.Items)}");
            //}
        }

        private IEnumerable<string> InputToLines()
        {
            if (File.Exists(this.Input))
            {
                Logger.Info($"Read file : {this.Input}");
                return File.ReadLines(this.Input);
            }
            else
                return new string[] { Input };
        }

        private void WriteOutput(GrokResult result)
        {
            switch(this.OutputType)
            {
                case OutputType.Text:
                    foreach (var item in result.Items)
                        Logger.Info(string.Join(", ", result.Items));
                    break;
            }
        }
    }
}

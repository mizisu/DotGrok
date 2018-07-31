namespace DotGrok.Cli
{
    using CommandLine;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    [Verb("pt",  HelpText = "Manage grok patterns.")]
    class PatternOptions : Options
    {
        [Option('a', "add", HelpText ="Add patterns ex) name1:exp1")]
        public IEnumerable<string> Patterns { get; set; }

        public override void Run()
        {
            if (this.Patterns.Count() != 0)
                AddPatterns();
            else
                PrintAllPatterns();
        }

        private void AddPatterns()
        {
            var items = this.Patterns.Select(s =>
            {
                var data = s.Split(':');
                return (data[0], string.Join("", data.Skip(1)));
            });

            foreach (var (name, expression) in items)
                Configuration.Grok.Patterns[name] = new GrokPattern(name, expression);

            Configuration.Save();
        }

        private void PrintAllPatterns()
        {
            Logger.Info("Print all patterns.");
            foreach (var kv in Configuration.Grok.Patterns)
                Logger.Info(kv.Value.ToString());
        }
    }
}

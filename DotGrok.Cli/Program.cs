namespace DotGrok.Cli
{
    using CommandLine;
    using System;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments(args, typeof(GrokOptions), typeof(PatternOptions))
                .MapResult(
                opt => Run(opt as Options),
                err => HandleError(err));
        }

        static int Run(Options options)
        {
            Configuration.Load();
            options.Run();
            return 0;
        }

        public static int HandleError(IEnumerable<Error> errors)
        {
            foreach (var item in errors)
            {
            }

            return 1;
        }
    }
}

namespace DotGrok.Test
{
    static class TestUtils
    {
        public static void Run(params string[] args)
        {
            DotGrok.Cli.Program.Main(args);
        }
    }
}
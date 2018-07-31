namespace DotGrok.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;
    public class CliTest
    {
        [Fact]
        public void EmptyRun()
        {
            TestUtils.Run();
        }

        [Fact]
        public void ShowPatterns()
        {
            TestUtils.Run(
                "pt"
            );
        }

        [Fact]
        public void AddPattern()
        {
            TestUtils.Run(
                "pt",
                "-a",
                @"DateTime:\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}"
            );
        }
    }
}

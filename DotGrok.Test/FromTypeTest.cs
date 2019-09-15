namespace DotGrok.Test
{
    using System.IO;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    class GrokInfo
    {

    }

    public class FromTypeTest
    {
        ITestOutputHelper output;

        public FromTypeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GrokFromType()
        {
            var grok = Grok.FromType<GrokInfo>();
        }
    }
}
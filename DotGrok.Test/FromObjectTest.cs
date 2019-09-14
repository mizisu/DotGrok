namespace DotGrok.Test
{
    using System.IO;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class FromObjectTest
    {
        ITestOutputHelper output;

        public FromObjectTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GrokFromObject()
        {

        }
    }
}
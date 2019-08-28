namespace DotGrok.Test
{
    using System.IO;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class ReadLogTest
    {

        ITestOutputHelper output;

        public ReadLogTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ReadApacheAccessLog()
        {
            // 64.242.88.10 - - [07/Mar/2004:16:10:02 -0800] "GET /mailman/listinfo/hsdivision HTTP/1.1" 200

            var grok = DotGrok.Grok.NewBuilder(
                "%{ClientId:ClientId} - - \\[%{Text:Date}\\] \"%{Method:Method} %{Text:Url} %{HttpVersion:HttpVersion}\" %{StateCode:StateCode}")
                .AddPattern("ClientId", @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}|.+")
                .AddPattern("Text", @".+")
                .AddPattern("Method", @"\w{3,4}")
                .AddPattern("HttpVersion", @"HTTP\/\d+.\d+")
                .AddPattern("StateCode", @"\d{3}")
                .Build();

            foreach (var line in File.ReadLines("./sample_apache_access_log.txt").Take(10))
            {
                var result = grok.Match(line);
                this.output.WriteLine(result.Success.ToString());
                var text = string.Join(' ', result.Items.Select(item => item.ToString()));
                this.output.WriteLine(text);
            }
        }
    }
}
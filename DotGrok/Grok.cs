namespace DotGrok
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Grok
    {
        public static GrokBuilder NewBuilder(string template)
        {
            return new GrokBuilder(template);
        }

        public static Grok FromObject(object obj)
        {
            return null;
        }

        internal Regex GrokRegex { get; set; }
        internal List<string> SemanticNames { get; set; }
        internal Dictionary<string, Func<string, object>> ValueConverters { get; set; } = new Dictionary<string, Func<string, object>>();

        internal Grok() { }

        public GrokResult Match(string text)
        {
            var match = this.GrokRegex.Match(text);

            var nameValues = this.SemanticNames
                .Select(name =>
                {
                    object value;

                    if (this.ValueConverters.TryGetValue(name, out var func))
                        value = func(match.Groups[name].Value);
                    else
                        value = match.Groups[name].Value;

                    return new GrokResultItem()
                    {
                        Name = name,
                        Value = value
                    };
                });

            return new GrokResult(match.Success, nameValues);
        }
    }
}

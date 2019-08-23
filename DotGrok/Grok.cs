namespace DotGrok
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Grok
    {
        public class GrokBuilder
        {
            private static readonly Regex GrokPatternRegex = new Regex(@"%{(\w+):(\w+)}", RegexOptions.Compiled);

            internal static Dictionary<string, GrokPattern> GlobalPatterns = new Dictionary<string, GrokPattern>();

            private Grok _grok = new Grok();
            private string _template;
            private Dictionary<string, GrokPattern> _patterns = new Dictionary<string, GrokPattern>();
            private Dictionary<string, Func<string, object>> _converters = new Dictionary<string, Func<string, object>>();
            private RegexOptions _regexOptions = RegexOptions.None;

            public GrokBuilder() { }

            public GrokBuilder(string template)
            {
                this._template = template;
            }

            public GrokBuilder SetTemplate(string template)
            {
                this._template = template;
                return this;
            }

            public GrokBuilder AddPattern(GrokPattern pattern)
            {
                this._patterns.Add(pattern.Name, pattern);
                return this;
            }

            public GrokBuilder AddPattern(IEnumerable<GrokPattern> patterns)
            {
                foreach (var pattern in patterns)
                    this._patterns.Add(pattern.Name, pattern);

                return this;
            }

            public GrokBuilder AddPattern(string name, string expression)
            {
                this.AddPattern(new GrokPattern(name, expression));
                return this;
            }

            public GrokBuilder AddConverter(string name, Func<string, object> converter)
            {
                this._converters.Add(name, converter);
                return this;
            }

            public GrokBuilder SetRegexOptions(RegexOptions options) {
                this._regexOptions = options;
                return this;
            }

            public Grok Build()
            {
                this._grok.GrokRegex = this.TemplateToRegex(this._template);
                this._grok.SemanticNames = this._grok.GrokRegex
                                    .GetGroupNames()
                                    .Skip(1)
                                    .ToList();

                return this._grok;
            }

            private Regex TemplateToRegex(string template)
            {
                string eval(Match match)
                {
                    // %{WORD:name} -> (?<name>\w+)
                    var patternName = match.Groups[1].Value; // WORD
                    var semantic = match.Groups[2].Value; // method
                    var pattern = this.GetPattern(patternName); // \w+

                    if (this._converters.TryGetValue(patternName, out var func))
                        this._grok.ValueConverters[semantic] = func;

                    return $"(?<{semantic}>{pattern.Expression ?? ""})";
                }

                var regex = GrokPatternRegex.Replace(template, eval);

                return new Regex(regex, this._regexOptions);
            }

            private GrokPattern GetPattern(string name)
            {
                GrokPattern value;
                if (this._patterns.TryGetValue(name, out value))
                    return value;
                else if (GlobalPatterns.TryGetValue(name, out value))
                    return value;

                return value;
            }
        }

        public static GrokBuilder NewBuilder()
        {
            return new GrokBuilder();
        }

        public static GrokBuilder NewBuilder(string template)
        {
            return new GrokBuilder(template);
        }

        public static void AddGlobalPattern(params GrokPattern[] patterns)
        {
            foreach (var item in patterns)
                GrokBuilder.GlobalPatterns[item.Name] = item;
        }
        
        public static void ClearGlobalPattern()
        {
            GrokBuilder.GlobalPatterns.Clear();
        }

        private Regex GrokRegex { get; set; }
        private List<string> SemanticNames { get; set; }
        private Dictionary<string, Func<string, object>> ValueConverters { get; set; } = new Dictionary<string, Func<string, object>>();

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

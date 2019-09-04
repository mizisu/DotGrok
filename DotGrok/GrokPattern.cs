namespace DotGrok
{
    using System;
    using System.Text.RegularExpressions;

    public class GrokPattern
    {
        public GrokPattern(string name, string expression)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));

            Regex.Match("", expression);
        }

        public string Name { get; set; }

        public string Expression { get; set; }

        public override string ToString() => $"{this.Name} : {this.Expression}";
    }
}

namespace DotGrok
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GrokResult
    {
        public GrokResult(bool success, IEnumerable<GrokResultItem> items)
        {
            this.Success = success;
            this.Items = items;
        }

        public bool Success { get; }
        public IEnumerable<GrokResultItem> Items { get; }
    }

    public class GrokResultItem
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString() => $"{this.Name} : {this.Value}";
    }
}

namespace DotGrok
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Dynamic;
    using System.Collections;

    public class GrokResult
    {
        public GrokResult(bool success, IEnumerable<GrokResultItem> items)
        {
            this.Success = success;
            this.Items = items;
        }

        public Dictionary<string, object> ToDictionary()
        {
            if (!Success) return new Dictionary<string, object>();
            return this.Items.ToDictionary(item => item.Name, item => item.Value);
        }

        public dynamic ToDynamic()
        {
            if (!Success) new object();
            var obj = new ExpandoObject();
            var dict = (ICollection<KeyValuePair<string, object>>)obj;
            foreach (var item in this.Items)
                dict.Add(new KeyValuePair<string, object>(item.Name, item.Value));
            return dict;
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

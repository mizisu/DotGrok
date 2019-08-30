[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false)]
public sealed class GrokPropertyAttribute : System.Attribute
{
    public GrokPropertyAttribute(string name, string expression)
    {
        this.Name = name;
        this.Expression = expression;
    }

    public string Name { get; }
    public string Expression { get; }
}
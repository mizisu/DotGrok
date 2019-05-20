# DotGrok
.NET Parse text with pattern.

## Usage
```csharp
var grok = DotGrok.Grok.NewBuilder(@"%{DateTime:Time}")
    .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
    .AddPattern("LogLevel", @".{5}")
    .AddPattern("Message", @".")
    .AddConverter("DateTime", s => DateTime.Parse(s))
    .Build();

var lines = new string[]{
    "2018-01-01 12:32:23.345"
    };

foreach (var item in lines)
{
    var r = grok.Match(item);
    Console.WriteLine($"{r.Success}");
    foreach (var resultItem in r.Items)
    {
        Console.WriteLine($"{resultItem.Name} : {resultItem.Value}");
    }
}
```

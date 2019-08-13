# DotGrok

.NET Parse text with pattern.

## Usage

```csharp
var grok = DotGrok.Grok.NewBuilder(@"%{DateTime:time} %{LogLevel:level} %{Message:message}")
                .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
                .AddPattern("LogLevel", @".\w+")
                .AddPattern("Message", @".+")
                .AddConverter("DateTime", s => DateTime.Parse(s))
                .Build();

var r = grok.Match("2018-01-01 12:32:23.345 INFO test message 1233tdsg");

foreach (var item in r.Items)
{
    System.Console.WriteLine($"{item.Name} : {item.Value}");
}
```

## Result

```
time : 01/01/2018 12:32:23
level : INFO
message : test message 1233tdsg
```

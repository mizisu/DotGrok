# DotGrok

.NET Parse text with pattern.

![build](https://travis-ci.org/mizisu/DotGrok.svg?branch=master)
<a href="https://www.nuget.org/packages/DotGrok"><img src="https://img.shields.io/nuget/v/dotgrok.svg?style=flat"></a>

## Installation
Package Manager
```
Install-Package DotGrok
```
.NET CLI
```
dotnet add package DotGrok
```

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

// To dictionary
var dict = r.ToDictionary();

// To dynamic type
dynamic obj = r.ToDynamic();
var level = obj.level; // get value
```

## Result

```
time : 01/01/2018 12:32:23
level : INFO
message : test message 1233tdsg
```

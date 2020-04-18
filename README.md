# DotGrok

.NET Parse text with pattern.

![build](https://travis-ci.org/mizisu/DotGrok.svg?branch=master)
<a href="https://www.nuget.org/packages/DotGrok"><img src="https://img.shields.io/nuget/v/dotgrok.svg?style=flat"></a>
<img src="https://img.shields.io/nuget/dt/dotgrok">

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

### Example 1

```csharp
// Setting extract format "%{PatternName:ExtractName}"
var grok = DotGrok.Grok.NewBuilder("%{Method:Method} %{HttpVersion:Version} %{StatusCode:StatusCode}")
    .AddPattern("Method", @"\w{3,4}")
    .AddPattern("HttpVersion", @"HTTP\/\d+.\d+")
    .AddPattern("StatusCode", @"\d{3}")
    .Build();

var sampleData = [
    "GET HTTP/1.1 200",
    "GET HTTP/1.1 404"
    "POST HTTP/1.1 200"
];

foreach(var line in sampleData)
{
    var result = grok.Match(line);
    foreach(var item in result.Items)
    {
        System.Console.WriteLine($"{item.Name} : {item.Value}");
    }
}
```

### Example 2

```csharp
var grok = DotGrok.Grok.NewBuilder("%{DateTime:Date}")
    .AddPattern("DateTime", @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3}")
    .AddConverter(s => DateTime.parse())
    .Build();

var r = grok.Match("2018-01-01 12:32:23.345 INFO test message 1233tdsg");

var items = r.Items.ToList();

Assert.Equal(new DateTime(2018, 01, 01, 12, 32, 23, 345), items[0].Value);
```

### Example 3

```csharp
// Read apache access log format like below
// 64.242.88.10 - - [07/Mar/2004:16:10:02 -0800] "GET /mailman/listinfo/hsdivision HTTP/1.1" 200
var grok = DotGrok.Grok.NewBuilder(
    "%{ClientId:ClientId} - - \\[%{Text:Date}\\] \"%{Method:Method} %{Text:Url} %{HttpVersion:HttpVersion}\" %{StatusCode:StatusCode}")
    .AddPattern("ClientId", @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}|.+")
    .AddPattern("Text", @".+")
    .AddPattern("Method", @"\w{3,4}")
    .AddPattern("HttpVersion", @"HTTP\/\d+.\d+")
    .AddPattern("StatusCode", @"\d{3}")
    .Build();

foreach (var line in File.ReadLines("./sample_apache_access_log.txt").Take(10))
{
    var result = grok.Match(line);
    var text = string.Join(", ", result.Items.Select(item => $"{item.Name}:{item.Value}"));
    System.Console.WriteLine(text);
    System.Console.WriteLine();
}
```

### Result

```
ClientId:64.242.88.10, Date:07/Mar/2004:17:17:27 -0800, Method:GET, Url:/twiki/bin/search/TWiki/?scope=topic&regex=on&search=^d, HttpVersion:HTTP/1.1, StateCode:200

ClientId:lj1036.inktomisearch.com, Date:07/Mar/2004:17:18:36 -0800, Method:GET, Url:/robots.txt, HttpVersion:HTTP/1.0, StateCode:200

ClientId:lj1090.inktomisearch.com, Date:07/Mar/2004:17:18:41 -0800, Method:GET, Url:/twiki/bin/view/Main/LondonOffice, HttpVersion:HTTP/1.0, StateCode:200

ClientId:64.242.88.10, Date:07/Mar/2004:17:21:44 -0800, Method:GET, Url:/twiki/bin/attach/TWiki/TablePlugin, HttpVersion:HTTP/1.1, StateCode:401
...
```

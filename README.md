# LibFree.AspNet.Mvc.Bundle
A library to bundle (minify/concat) css and js for Asp.Net Mvc 6

## Installation
In nuget console:

    Install-Package LibFree.AspNet.Mvc.Bundle -Pre
    Install-Package LibFree.AspNet.Mvc.Bundle.HtmlAgilityPack -Pre
    Install-Package LibFree.AspNet.Mvc.Bundle.YUICompressor -Pre

## Startup.cs

```csharp

using LibFree.AspNet.Mvc.Bundle.HtmlAgilityPack;
using LibFree.AspNet.Mvc.Bundle.Middlewares;
using LibFree.AspNet.Mvc.Bundle.YUICompressor;
```

```csharp

public void ConfigureServices(IServiceCollection services)
{
    ...
	services.UserYUICompressor();
	services.UserHtmlAgilityPackParser();
	...
}
```

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	...
	app.UseBundle();
	...
}
```
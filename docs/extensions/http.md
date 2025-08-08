# Sending HTTP Requests
FFlow provides a package for sending HTTP requests as part of your workflow. This is particularly useful for interacting with web APIs, downloading files, or sending data to remote services.
To use the HTTP extensions and steps, you need to install the `FFlow.Steps.Http` package:

```bash
dotnet add package FFlow.Steps.Http
```

All the HTTP requests are done with `HttpRequestStep` and its extension methods to allow for a fluent API. Each `HttpRequestStep` defines its output in the context with the `HttpResponseMessage` result, which can be obtained through `IFlowContext.GetOutputFor<HttpRequestStep, HttpResponseMessage>()`.
The package includes the following commands and overloads:

- `HttpGet()`
- `HttpPost()`
- `HttpPut()`
- `HttpDelete()`
- `HttpPatch()`
- `HttpHead()`
- `HttpOptions()`

These either take the url as a string or an `Action<HttpRequestStep>` to configure the request. 
### Example Usage
```csharp
builder.StartWith<SomeSetupStep>()
    .HttpGet("https://api.example.com/data")
    .Finally((ctx, ct) => {
        var response = ctx.GetOutputFor<HttpRequestStep, HttpResponseMessage>();
        if (response.IsSuccessStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Response: {content}");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    });
```

You can set additional options for the HTTP requests, such as headers, query parameters, request body and other options using the overload with a configure action provided by the extension methods. For example:

```csharp
var builder = new FFlowBuilder()
     .HttpPost(step =>
     {
         step.Url = "https://jsonplaceholder.typicode.com/posts/1";
         step.AcceptableStatusCodes = new[] { System.Net.HttpStatusCode.OK };
         step.Timeout = TimeSpan.FromSeconds(10);
     });
```







using System.Text.Json;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;
using FFlow.Steps.Http;

var builder = new FFlowBuilder()
     .HttpGet(step =>
     {
         step.Url = "https://jsonplaceholder.typicode.com/posts/1";
         step.AcceptableStatusCodes = new[] { System.Net.HttpStatusCode.OK };
         step.Timeout = TimeSpan.FromSeconds(10);
     })
     .Then(async (context, ct) =>  Console.WriteLine(await context.GetLastOutput<HttpResponseMessage>().Content.ReadAsStringAsync()));

await builder.Build().RunAsync();



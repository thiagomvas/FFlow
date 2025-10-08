using System.Text.Json;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.DSL;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;
using FFlow.Steps.FileIO;
using FFlow.Steps.Http;

var lexer = new Lexer(@"
pipeline ""Hello World"":
demo.hello(name=""World"")
");

var parser = new Parser(lexer.Tokenize());
var pipelineNode = parser.ParsePipeline();

Console.WriteLine(JsonSerializer.Serialize(pipelineNode, new JsonSerializerOptions { WriteIndented = true }));

var container = new StepContainer();
container.LoadAllRegistries();
await container.GetStep("demo.hello").RunAsync(new InMemoryFFLowContext());


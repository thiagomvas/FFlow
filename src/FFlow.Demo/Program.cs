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
pipeline ""BuildAndPublish"":
    dotnet.build(""."")
    dotnet.test(""."", noBuild=true)
    throwIf dotnet.test.failed > 0, ""Tests have failed""
    dotnet.publish(""."", configuration=""Release"")
    dotnet.pack(""."", configuration=""Release"", output=""nupkgs"")
    dotnet.nugetPush(""nupkgs"", apiKey=ctx.NUGET_API_KEY)
");

foreach (var t in lexer.Tokenize())
{
    Console.WriteLine($"{t.Type} '{t.Value}' ({t.Line},{t.Column})");
    
}



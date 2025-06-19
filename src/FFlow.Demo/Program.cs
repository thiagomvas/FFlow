using System.Reflection;
using FFlow.Core;

string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var assemblies = Directory.GetFiles(path, "*.dll")
    .Select(Assembly.LoadFrom)
    .Where(a => a.GetTypes().Any(t => typeof(IFlowStep).IsAssignableFrom(t) || typeof(IWorkflowDefinition).IsAssignableFrom(t)))
    .ToList();
Console.WriteLine("Available Assemblies:");
foreach (var assembly in assemblies)
{
    Console.WriteLine($"- {assembly.GetName().Name}");
}
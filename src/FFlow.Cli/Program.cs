using FFlow.Cli;
using FFlow.Cli.Commands;
using Spectre.Console;

var root = new RootCommand();

var doctor = new DoctorCommand();
var init = new InitCommand();
var run = new RunCommand();

root.Subcommands.Add(doctor);
root.Subcommands.Add(init);
root.Subcommands.Add(run);

var dispatcher = new CommandDispatcher(root);

return dispatcher.Dispatch(args);
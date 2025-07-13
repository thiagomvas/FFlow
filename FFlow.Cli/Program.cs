using FFlow.Cli;
using FFlow.Cli.Commands;
using Spectre.Console;

var root = new RootCommand();

var doctor = new DoctorCommand();
var init = new InitCommand();

root.Subcommands.Add(doctor);
root.Subcommands.Add(init);

var dispatcher = new CommandDispatcher(root);

return dispatcher.Dispatch(args);
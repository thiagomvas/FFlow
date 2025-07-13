using FFlow.Cli;
using FFlow.Cli.Commands;

var root = new RootCommand();

var doctor = new DoctorCommand();
root.Subcommands.Add(doctor);

var dispatcher = new CommandDispatcher(root);

return dispatcher.Dispatch(args);
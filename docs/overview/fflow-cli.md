# Using FFlow's CLI for workflow validation
One of the main selling points for FFlow is the ability to test the workflows locally before deploying them to production. This is made easy with the FFlow CLI, which allows you to run and test your workflows directly from the command line.

## Installing the FFlow CLI
To install the FFlow CLI, you can use the following command:

```bash
dotnet tool install -g FFlow.Cli
```

> [!IMPORTANT]
> Make sure you have Docker installed and running on your machine, as the CLI uses Docker to create an isolated environment for running your workflows.


## Running a Workflow
Once you have the CLI installed, you can test a workflow by running the following command:

```bash
fflow run path/to/your/workflow.cs
```

It'll spin up a docker container with all the dependencies and run your workflow, providing you with the output and any errors that may occur.

You can also set a folder as the root used when starting the image, allowing you to simulate scenarios where, for example, you run FFlow from Github Actions which include your repository most of the time.

```bash
fflow run path/to/your/workflow.cs --root path/to/your/root
```

Or you can also simply include files manually by passing a **comma separated** list of files to include
```bash
fflow run path/to/your/workflow.cs --include path/to/your/file1.txt,path/to/your/file2.json
```

## Initializing a workflow
You can create a new workflow file with the following command:

```bash
fflow init path/to/your/workflow.cs --name "My Workflow" --path "path/to/your/workflow.cs"
```

Or, you can do it interactively, where it will help you set up the workflow file following **file based projects** introduced in .NET 10, including any extra packages you might need.

```bash
fflow init -i
```

## Checking tool health
You can check if your machine has everything needed to run FFlow workflows with the following command:

```bash
fflow doctor
```

It'll diagnose your machine and let you know if anything is missing or misconfigured.
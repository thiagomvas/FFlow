# Transfering files via SFTP
You can do certain operations via SFTP using the `FFlow.Steps.SFTP` package which uses `Renci.SshNet.SftpClient` internally. To install it, use the Nuget Package Manager or
```bash
dotnet add package FFlow.Steps.SFTP
```

The package supports the following operations:
- Connecting and Disconnecting to an SFTP server
- File & Directory Creation
- File & Directory Deletion
- File & Directory Upload
- File & Directory Download
- File renaming

Each step has their own respective `IWorkflowBuilder` extension method (e.g. `UploadFileViaSftp` and so on). You can use these methods to add the steps to your workflow.

> [!IMPORTANT]
> Before using the SFTP steps, you **need** to connect to the SFTP server using the `ConnectToSftp` extension method (or `ConnectToSftpStep`). This step establishes a connection to the SFTP server and is required for all subsequent SFTP operations. If you want to use the connection in your own steps, you can get it via `context.GetSingleValue<SftpClient>()`

## Example
```csharp
using FFlow;
using FFlow.Steps.SFTP;

var flow = new FFlowBuilder()
    .ConnectToSftp("localhost", 2222, "user", "password")
    .UploadDirectoryViaSftp("/path/to/a/directory", "some/directory")
    .Build();

var ctx = await flow.RunAsync();
```

If you do not connect to the SFTP server, you will get an exception when trying to run the workflow. The exception will indicate that the connection is not established.
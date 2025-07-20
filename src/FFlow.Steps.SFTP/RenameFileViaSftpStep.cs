using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class RenameFileViaSftpStep : FlowStep
{
    public string RemoteFilePath { get; set; } = string.Empty;
    public string NewRemoteFilePath { get; set; } = string.Empty;

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        if (string.IsNullOrEmpty(NewRemoteFilePath))
            throw new InvalidOperationException("New remote file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        sftpClient.RenameFile(RemoteFilePath, NewRemoteFilePath);

        return Task.CompletedTask;
    }
    
}
using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class DeleteFileViaSftpStep : FlowStep
{
    public string RemoteFilePath { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        sftpClient.DeleteFile(RemoteFilePath);

        return Task.CompletedTask;
    }
}
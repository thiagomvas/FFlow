using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class CreateDirectoryViaSftpStep : FlowStep
{
    public string RemoteDirectoryPath { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteDirectoryPath))
            throw new InvalidOperationException("Remote directory path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        if (!sftpClient.Exists(RemoteDirectoryPath))
        {
            sftpClient.CreateDirectory(RemoteDirectoryPath);
        }

        return Task.CompletedTask;
    }
}
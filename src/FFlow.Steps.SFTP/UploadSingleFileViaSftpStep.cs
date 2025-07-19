using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class UploadSingleFileViaSftpStep : FlowStep
{
    public string LocalFilePath { get; set; } = string.Empty;
    public string RemoteFilePath { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(LocalFilePath))
            throw new InvalidOperationException("Local file path must be set.");

        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        using var fileStream = new FileStream(LocalFilePath, FileMode.Open, FileAccess.Read);
        sftpClient.UploadFile(fileStream, RemoteFilePath);

        return Task.CompletedTask;
    }
}
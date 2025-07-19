using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class DownloadFileViaSftpStep : FlowStep
{
    public string RemoteFilePath { get; set; } = string.Empty;
    public string LocalFilePath { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        if (string.IsNullOrEmpty(LocalFilePath))
            throw new InvalidOperationException("Local file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();
        
        var localDir = Path.GetDirectoryName(LocalFilePath);
        if (!string.IsNullOrEmpty(localDir) && !Directory.Exists(localDir))
        {
            Directory.CreateDirectory(localDir);
        }

        using var fileStream = new FileStream(LocalFilePath, FileMode.Create, FileAccess.Write);
        sftpClient.DownloadFile(RemoteFilePath, fileStream);

        return Task.CompletedTask;
    }
}
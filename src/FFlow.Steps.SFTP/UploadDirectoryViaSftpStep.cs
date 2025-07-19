using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class UploadDirectoryViaSftpStep : FlowStep
{
    public string LocalDirectoryPath { get; set; } = string.Empty;
    public string RemoteDirectoryPath { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(LocalDirectoryPath))
            throw new InvalidOperationException("Local directory path must be set.");

        if (string.IsNullOrEmpty(RemoteDirectoryPath))
            throw new InvalidOperationException("Remote directory path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(LocalDirectoryPath))
            throw new DirectoryNotFoundException($"Local directory '{LocalDirectoryPath}' does not exist.");

        // Ensure the remote directory exists
        if (!sftpClient.Exists(RemoteDirectoryPath))
        {
            sftpClient.CreateDirectory(RemoteDirectoryPath);
        }

        // Upload all files in the local directory
        foreach (var file in Directory.GetFiles(LocalDirectoryPath))
        {
            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var remoteFilePath = Path.Combine(RemoteDirectoryPath, Path.GetFileName(file));
            sftpClient.UploadFile(fileStream, remoteFilePath);
        }

        return Task.CompletedTask;
    }
}
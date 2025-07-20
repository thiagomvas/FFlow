using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class UploadDirectoryViaSftpStep : FlowStep
{
    public string LocalDirectoryPath { get; set; } = string.Empty;
    public string RemoteDirectoryPath { get; set; } = string.Empty;

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        if (string.IsNullOrEmpty(LocalDirectoryPath))
            throw new InvalidOperationException("Local directory path must be set.");

        if (string.IsNullOrEmpty(RemoteDirectoryPath))
            throw new InvalidOperationException("Remote directory path must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(LocalDirectoryPath))
            throw new DirectoryNotFoundException($"Local directory '{LocalDirectoryPath}' does not exist.");

        UploadDirectoryRecursive(sftpClient, LocalDirectoryPath, RemoteDirectoryPath, cancellationToken);

        return Task.CompletedTask;
    }

    private void UploadDirectoryRecursive(SftpClient client, string localPath, string remotePath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!client.Exists(remotePath))
        {
            client.CreateDirectory(remotePath);
        }

        // Upload files in current directory
        foreach (var file in Directory.GetFiles(localPath))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var remoteFilePath = remotePath.TrimEnd('/') + "/" + Path.GetFileName(file);
            client.UploadFile(fileStream, remoteFilePath);
        }

        // Recurse into subdirectories
        foreach (var dir in Directory.GetDirectories(localPath))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var remoteSubDir = remotePath.TrimEnd('/') + "/" + Path.GetFileName(dir);
            UploadDirectoryRecursive(client, dir, remoteSubDir, cancellationToken);
        }
    }
}

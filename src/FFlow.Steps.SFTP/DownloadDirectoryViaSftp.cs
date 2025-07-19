using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class DownloadDirectoryViaSftp : FlowStep
{
    public string RemoteDirectory { get; set; } = string.Empty;
    public string LocalDirectory { get; set; } = string.Empty;

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        if (string.IsNullOrEmpty(RemoteDirectory))
            throw new InvalidOperationException("Remote directory must be set.");

        if (string.IsNullOrEmpty(LocalDirectory))
            throw new InvalidOperationException("Local directory must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        DownloadDirectoryRecursive(sftpClient, RemoteDirectory, LocalDirectory, cancellationToken);

        return Task.CompletedTask;
    }

    private void DownloadDirectoryRecursive(SftpClient client, string remotePath, string localPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!Directory.Exists(localPath))
            Directory.CreateDirectory(localPath);

        foreach (var entry in client.ListDirectory(remotePath))
        {
            if (entry.Name == "." || entry.Name == "..")
                continue;

            var remoteFilePath = entry.FullName;
            var localFilePath = Path.Combine(localPath, entry.Name);

            if (entry.IsDirectory)
            {
                DownloadDirectoryRecursive(client, remoteFilePath, localFilePath, cancellationToken);
            }
            else if (entry.IsRegularFile)
            {
                using var fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write);
                client.DownloadFile(remoteFilePath, fs);
            }
        }
    }
}
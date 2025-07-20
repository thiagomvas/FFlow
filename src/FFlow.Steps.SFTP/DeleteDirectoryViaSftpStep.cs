using System.Threading;
using System.Threading.Tasks;
using FFlow.Core;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace FFlow.Steps.SFTP;

public class DeleteDirectoryViaSftpStep : FlowStep
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

        if (sftpClient.Exists(RemoteDirectoryPath))
        {
            DeleteDirectoryRecursive(sftpClient, RemoteDirectoryPath, cancellationToken);
        }

        return Task.CompletedTask;
    }

    private void DeleteDirectoryRecursive(SftpClient client, string remotePath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entries = client.ListDirectory(remotePath);

        foreach (var entry in entries)
        {
            if (entry.Name == "." || entry.Name == "..")
                continue;

            if (entry.IsDirectory)
            {
                DeleteDirectoryRecursive(client, entry.FullName, cancellationToken);
            }
            else
            {
                client.DeleteFile(entry.FullName);
            }
        }

        client.DeleteDirectory(remotePath);
    }
}
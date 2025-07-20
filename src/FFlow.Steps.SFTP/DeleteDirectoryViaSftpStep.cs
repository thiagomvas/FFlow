using System.Threading;
using System.Threading.Tasks;
using FFlow.Core;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to delete a directory on an SFTP server.
/// </summary>
public class DeleteDirectoryViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the remote directory path to be deleted.
    /// </summary>
    public string RemoteDirectoryPath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to delete a directory on the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the remote directory path is not set or the SFTP client is not connected.
    /// </exception>
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

    /// <summary>
    /// Recursively deletes a directory and its contents on the SFTP server.
    /// </summary>
    /// <param name="client">The SFTP client.</param>
    /// <param name="remotePath">The remote directory path to delete.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
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
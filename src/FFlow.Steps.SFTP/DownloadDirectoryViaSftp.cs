using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to download a directory from an SFTP server to a local directory.
/// </summary>
[StepName("Download Directory via SFTP")]
[StepTags("sftp")]
public class DownloadDirectoryViaSftp : FlowStep
{
    /// <summary>
    /// Gets or sets the remote directory path to be downloaded.
    /// </summary>
    public string RemoteDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the local directory path where the remote directory will be downloaded.
    /// </summary>
    public string LocalDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to download a directory from the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the SFTP client is not connected, or when the remote or local directory paths are not set.
    /// </exception>
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

    /// <summary>
    /// Recursively downloads a directory and its contents from the SFTP server to a local directory.
    /// </summary>
    /// <param name="client">The SFTP client.</param>
    /// <param name="remotePath">The remote directory path to download.</param>
    /// <param name="localPath">The local directory path where the contents will be saved.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to upload a local directory to a remote directory on an SFTP server.
/// </summary>
[StepName("Upload Directory via SFTP")]
[StepTags("sftp")]
public class UploadDirectoryViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the local directory path to be uploaded.
    /// </summary>
    public string LocalDirectoryPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remote directory path where the local directory will be uploaded.
    /// </summary>
    public string RemoteDirectoryPath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to upload a local directory to the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the SFTP client is not connected, or when the local or remote directory paths are not set.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when the local directory does not exist.
    /// </exception>
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

    /// <summary>
    /// Recursively uploads a local directory and its contents to a remote directory on the SFTP server.
    /// </summary>
    /// <param name="client">The SFTP client.</param>
    /// <param name="localPath">The local directory path to upload.</param>
    /// <param name="remotePath">The remote directory path where the contents will be uploaded.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    private void UploadDirectoryRecursive(SftpClient client, string localPath, string remotePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!client.Exists(remotePath))
        {
            client.CreateDirectory(remotePath);
        }

        foreach (var file in Directory.GetFiles(localPath))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var remoteFilePath = remotePath.TrimEnd('/') + "/" + Path.GetFileName(file);
            client.UploadFile(fileStream, remoteFilePath);
        }

        foreach (var dir in Directory.GetDirectories(localPath))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var remoteSubDir = remotePath.TrimEnd('/') + "/" + Path.GetFileName(dir);
            UploadDirectoryRecursive(client, dir, remoteSubDir, cancellationToken);
        }
    }
}
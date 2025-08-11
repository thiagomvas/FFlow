using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to download a file from an SFTP server to a local file path.
/// </summary>
[StepName("Download File via SFTP")]
[StepTags("sftp")]
public class DownloadFileViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the remote file path to be downloaded.
    /// </summary>
    public string RemoteFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the local file path where the remote file will be saved.
    /// </summary>
    public string LocalFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to download a file from the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the remote file path or local file path is not set, or when the SFTP client is not connected.
    /// </exception>
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
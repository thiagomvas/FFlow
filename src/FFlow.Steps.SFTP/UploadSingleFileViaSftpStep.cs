using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to upload a single local file to a remote file path on an SFTP server.
/// </summary>
[StepName("Upload Single File via SFTP")]
[StepTags("sftp")]
public class UploadSingleFileViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the local file path to be uploaded.
    /// </summary>
    public string LocalFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remote file path where the local file will be uploaded.
    /// </summary>
    public string RemoteFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to upload a single file to the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the local file path or remote file path is not set, or when the SFTP client is not connected.
    /// </exception>
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
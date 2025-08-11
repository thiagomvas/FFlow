using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to rename a file on an SFTP server.
/// </summary>
[StepName("Rename File via SFTP")]
[StepTags("sftp")]
public class RenameFileViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the remote file path to be renamed.
    /// </summary>
    public string RemoteFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new remote file path after renaming.
    /// </summary>
    public string NewRemoteFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to rename a file on the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the remote file path or the new remote file path is not set, or when the SFTP client is not connected.
    /// </exception>
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        if (string.IsNullOrEmpty(NewRemoteFilePath))
            throw new InvalidOperationException("New remote file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        sftpClient.RenameFile(RemoteFilePath, NewRemoteFilePath);

        return Task.CompletedTask;
    }
}
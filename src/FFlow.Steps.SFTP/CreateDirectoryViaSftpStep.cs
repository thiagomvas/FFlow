using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to create a directory on an SFTP server.
/// </summary>
public class CreateDirectoryViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the remote directory path to be created.
    /// </summary>
    public string RemoteDirectoryPath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to create a directory on the SFTP server.
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

        if (!sftpClient.Exists(RemoteDirectoryPath))
        {
            sftpClient.CreateDirectory(RemoteDirectoryPath);
        }

        return Task.CompletedTask;
    }
}
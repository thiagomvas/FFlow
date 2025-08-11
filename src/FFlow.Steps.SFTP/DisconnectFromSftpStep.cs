using FFlow.Core;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to disconnect from an SFTP server.
/// </summary>
[StepName("Disconnect from SFTP Server")]
[StepTags("sftp")]
public class DisconnectFromSftpStep : FlowStep
{
    /// <summary>
    /// Executes the step to disconnect from the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the SFTP client is not connected or if the disconnection fails.
    /// </exception>
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        var sftpClient = context.GetSingleValue<Renci.SshNet.SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            if (sftpClient.IsConnected)
            {
                sftpClient.Disconnect();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to disconnect from SFTP server: {ex.Message}", ex);
        }

        return Task.CompletedTask;
    }
}
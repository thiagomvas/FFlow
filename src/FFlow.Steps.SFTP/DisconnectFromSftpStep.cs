using FFlow.Core;

namespace FFlow.Steps.SFTP;

public class DisconnectFromSftpStep : FlowStep
{
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
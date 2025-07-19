using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

public class ConnectToSftpStep : FlowStep
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 22;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(Host))
            throw new InvalidOperationException("Host must be set.");

        if (string.IsNullOrEmpty(Username))
            throw new InvalidOperationException("Username must be set.");
        
        var sftpClient = new SftpClient(Host, Port, Username, Password);
        try
        {
            sftpClient.Connect();
            context.SetSingleValue(sftpClient);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to connect to SFTP server: {ex.Message}", ex);
        }
        
        return Task.CompletedTask;
    }
}
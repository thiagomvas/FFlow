using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFlow.Core;
using Renci.SshNet;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Represents a workflow step to create a file on an SFTP server.
/// </summary>
[StepName("Create File via SFTP")]
[StepTags("sftp")]
public class CreateFileViaSftpStep : FlowStep
{
    /// <summary>
    /// Gets or sets the remote file path where the file will be created.
    /// </summary>
    public string RemoteFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Executes the step to create a file on the SFTP server.
    /// </summary>
    /// <param name="context">The flow context containing the SFTP client.</param>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the remote file path is not set or the SFTP client is not connected.
    /// </exception>
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RemoteFilePath))
            throw new InvalidOperationException("Remote file path must be set.");

        var sftpClient = context.GetSingleValue<SftpClient>();
        if (sftpClient == null)
            throw new InvalidOperationException("SFTP client is not connected.");

        cancellationToken.ThrowIfCancellationRequested();

        using var emptyStream = new MemoryStream(); // zero-length stream
        sftpClient.UploadFile(emptyStream, RemoteFilePath);

        return Task.CompletedTask;
    }
}
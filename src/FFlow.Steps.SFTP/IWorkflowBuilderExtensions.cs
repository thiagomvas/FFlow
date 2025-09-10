using FFlow.Core;

namespace FFlow.Steps.SFTP;

/// <summary>
/// Provides extension methods for building SFTP-related workflow steps.
/// </summary>
public static class IWorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a step to connect to an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="host">The SFTP server host.</param>
    /// <param name="port">The SFTP server port (default is 22).</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <returns>A configurable step builder.</returns>
    /// <remarks>
    /// This step is required before performing any SFTP operations.
    /// </remarks>
    public static WorkflowBuilderBase ConnectToSftp(
        this WorkflowBuilderBase builder,
        string host,
        int port = 22,
        string username = "",
        string password = "")
    {
        builder
            .Then<ConnectToSftpStep>()
            .Input<ConnectToSftpStep>(step => step.Host = host)
            .Input<ConnectToSftpStep>(step => step.Port = port)
            .Input<ConnectToSftpStep>(step => step.Username = username)
            .Input<ConnectToSftpStep>(step => step.Password = password);
        return builder;
    }

    /// <summary>
    /// Adds a step to upload a file to an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="localFilePath">The local file path.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase UploadFileViaSftp(
        this WorkflowBuilderBase builder,
        string localFilePath,
        string remoteFilePath)
    {
        return builder
            .Then<UploadSingleFileViaSftpStep>()
            .Input<UploadSingleFileViaSftpStep>(step => step.LocalFilePath = localFilePath)
            .Input<UploadSingleFileViaSftpStep>(step => step.RemoteFilePath = remoteFilePath);
    }

    /// <summary>
    /// Adds a step to upload a directory to an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="localDirectoryPath">The local directory path.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase UploadDirectoryViaSftp(
        this WorkflowBuilderBase builder,
        string localDirectoryPath,
        string remoteDirectoryPath)
    {
        return builder
            .Then<UploadDirectoryViaSftpStep>()
            .Input<UploadDirectoryViaSftpStep>(step => step.LocalDirectoryPath = localDirectoryPath)
            .Input<UploadDirectoryViaSftpStep>(step => step.RemoteDirectoryPath = remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to disconnect from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase DisconnectFromSftp(this WorkflowBuilderBase builder)
    {
        return builder.Then<DisconnectFromSftpStep>();
    }

    /// <summary>
    /// Adds a step to download a file from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="localFilePath">The local file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase DownloadFileViaSftp(
        this WorkflowBuilderBase builder,
        string remoteFilePath,
        string localFilePath)
    {
        return builder
            .Then<DownloadFileViaSftpStep>()
            .Input<DownloadFileViaSftpStep>(step => step.RemoteFilePath = remoteFilePath)
            .Input<DownloadFileViaSftpStep>(step => step.LocalFilePath = localFilePath);
    }

    /// <summary>
    /// Adds a step to download a directory from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <param name="localDirectoryPath">The local directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase DownloadDirectoryViaSftp(
        this WorkflowBuilderBase builder,
        string remoteDirectoryPath,
        string localDirectoryPath)
    {
        return builder
            .Then<DownloadDirectoryViaSftp>()
            .Input<DownloadDirectoryViaSftp>(step => step.RemoteDirectory = remoteDirectoryPath)
            .Input<DownloadDirectoryViaSftp>(step => step.LocalDirectory = localDirectoryPath);
    }

    /// <summary>
    /// Adds a step to delete a file from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase DeleteFileViaSftp(
        this WorkflowBuilderBase builder,
        string remoteFilePath)
    {
        return builder
            .Then<DeleteFileViaSftpStep>()
            .Input<DeleteFileViaSftpStep>(step => step.RemoteFilePath = remoteFilePath);
    }

    /// <summary>
    /// Adds a step to delete a directory from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase DeleteDirectoryViaSftp(
        this WorkflowBuilderBase builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<DeleteDirectoryViaSftpStep>()
            .Input<DeleteDirectoryViaSftpStep>(step => step.RemoteDirectoryPath = remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to create a directory on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase CreateDirectoryViaSftp(
        this WorkflowBuilderBase builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<CreateDirectoryViaSftpStep>()
            .Input<CreateDirectoryViaSftpStep>(step => step.RemoteDirectoryPath = remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to create a file on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase CreateFileViaSftp(
        this WorkflowBuilderBase builder,
        string remoteFilePath)
    {
        return builder
            .Then<CreateFileViaSftpStep>()
            .Input<CreateFileViaSftpStep>(step => step.RemoteFilePath = remoteFilePath);
    }

    /// <summary>
    /// Adds a step to rename a file on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The current remote file path.</param>
    /// <param name="newRemoteFilePath">The new remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static WorkflowBuilderBase RenameFileViaSftp(
        this WorkflowBuilderBase builder,
        string remoteFilePath,
        string newRemoteFilePath)
    {
        return builder
            .Then<RenameFileViaSftpStep>()
            .Input<RenameFileViaSftpStep>(step => step.RemoteFilePath = remoteFilePath)
            .Input<RenameFileViaSftpStep>(step => step.NewRemoteFilePath = newRemoteFilePath);
    }
}
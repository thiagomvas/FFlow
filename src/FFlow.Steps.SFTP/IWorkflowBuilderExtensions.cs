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
    public static IConfigurableStepBuilder ConnectToSftp(
        this IWorkflowBuilder builder,
        string host,
        int port = 22,
        string username = "",
        string password = "")
    {
        return builder
            .Then<ConnectToSftpStep>()
            .Input<ConnectToSftpStep, string>(step => step.Host, host)
            .Input<ConnectToSftpStep, int>(step => step.Port, port)
            .Input<ConnectToSftpStep, string>(step => step.Username, username)
            .Input<ConnectToSftpStep, string>(step => step.Password, password);
    }

    /// <summary>
    /// Adds a step to upload a file to an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="localFilePath">The local file path.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder UploadFileViaSftp(
        this IWorkflowBuilder builder,
        string localFilePath,
        string remoteFilePath)
    {
        return builder
            .Then<UploadSingleFileViaSftpStep>()
            .Input<UploadSingleFileViaSftpStep, string>(step => step.LocalFilePath, localFilePath)
            .Input<UploadSingleFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath);
    }

    /// <summary>
    /// Adds a step to upload a directory to an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="localDirectoryPath">The local directory path.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder UploadDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string localDirectoryPath,
        string remoteDirectoryPath)
    {
        return builder
            .Then<UploadDirectoryViaSftpStep>()
            .Input<UploadDirectoryViaSftpStep, string>(step => step.LocalDirectoryPath, localDirectoryPath)
            .Input<UploadDirectoryViaSftpStep, string>(step => step.RemoteDirectoryPath, remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to disconnect from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder DisconnectFromSftp(this IWorkflowBuilder builder)
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
    public static IConfigurableStepBuilder DownloadFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath,
        string localFilePath)
    {
        return builder
            .Then<DownloadFileViaSftpStep>()
            .Input<DownloadFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath)
            .Input<DownloadFileViaSftpStep, string>(step => step.LocalFilePath, localFilePath);
    }

    /// <summary>
    /// Adds a step to download a directory from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <param name="localDirectoryPath">The local directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder DownloadDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string remoteDirectoryPath,
        string localDirectoryPath)
    {
        return builder
            .Then<DownloadDirectoryViaSftp>()
            .Input<DownloadDirectoryViaSftp, string>(step => step.RemoteDirectory, remoteDirectoryPath)
            .Input<DownloadDirectoryViaSftp, string>(step => step.LocalDirectory, localDirectoryPath);
    }

    /// <summary>
    /// Adds a step to delete a file from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder DeleteFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath)
    {
        return builder
            .Then<DeleteFileViaSftpStep>()
            .Input<DeleteFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath);
    }

    /// <summary>
    /// Adds a step to delete a directory from an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder DeleteDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<DeleteDirectoryViaSftpStep>()
            .Input<DeleteDirectoryViaSftpStep, string>(step => step.RemoteDirectoryPath, remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to create a directory on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteDirectoryPath">The remote directory path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder CreateDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<CreateDirectoryViaSftpStep>()
            .Input<CreateDirectoryViaSftpStep, string>(step => step.RemoteDirectoryPath, remoteDirectoryPath);
    }

    /// <summary>
    /// Adds a step to create a file on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder CreateFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath)
    {
        return builder
            .Then<CreateFileViaSftpStep>()
            .Input<CreateFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath);
    }

    /// <summary>
    /// Adds a step to rename a file on an SFTP server.
    /// </summary>
    /// <param name="builder">The workflow builder.</param>
    /// <param name="remoteFilePath">The current remote file path.</param>
    /// <param name="newRemoteFilePath">The new remote file path.</param>
    /// <returns>A configurable step builder.</returns>
    public static IConfigurableStepBuilder RenameFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath,
        string newRemoteFilePath)
    {
        return builder
            .Then<RenameFileViaSftpStep>()
            .Input<RenameFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath)
            .Input<RenameFileViaSftpStep, string>(step => step.NewRemoteFilePath, newRemoteFilePath);
    }
}
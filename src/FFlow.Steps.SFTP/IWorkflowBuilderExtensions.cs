using FFlow.Core;

namespace FFlow.Steps.SFTP;

public static class IWorkflowBuilderExtensions
{
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

    public static IConfigurableStepBuilder DisconnectFromSftp(this IWorkflowBuilder builder)
    {
        return builder.Then<DisconnectFromSftpStep>();
    }
    
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
    
    public static IConfigurableStepBuilder DeleteFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath)
    {
        return builder
            .Then<DeleteFileViaSftpStep>()
            .Input<DeleteFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath);
    }
    
    public static IConfigurableStepBuilder DeleteDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<DeleteDirectoryViaSftpStep>()
            .Input<DeleteDirectoryViaSftpStep, string>(step => step.RemoteDirectoryPath, remoteDirectoryPath);
    }
    
    public static IConfigurableStepBuilder CreateDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string remoteDirectoryPath)
    {
        return builder
            .Then<CreateDirectoryViaSftpStep>()
            .Input<CreateDirectoryViaSftpStep, string>(step => step.RemoteDirectoryPath, remoteDirectoryPath);
    }
    
    public static IConfigurableStepBuilder CreateFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath)
    {
        return builder
            .Then<CreateFileViaSftpStep>()
            .Input<CreateFileViaSftpStep, string>(step => step.RemoteFilePath, remoteFilePath);
    }
    
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

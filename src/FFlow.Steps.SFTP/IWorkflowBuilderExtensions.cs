using FFlow.Core;

namespace FFlow.Steps.SFTP;

public static class IWorkflowBuilderExtensions
{
    public static IWorkflowBuilder ConnectToSftp(
        this IWorkflowBuilder builder,
        string host,
        int port = 22,
        string username = "",
        string password = "")
    {
        return builder.AddStep(new ConnectToSftpStep
        {
            Host = host,
            Port = port,
            Username = username,
            Password = password
        });
    }
    
    public static IWorkflowBuilder UploadFileViaSftp(
        this IWorkflowBuilder builder,
        string localFilePath,
        string remoteFilePath)
    {
        return builder.AddStep(new UploadSingleFileViaSftpStep()
        {
            LocalFilePath = localFilePath,
            RemoteFilePath = remoteFilePath
        });
    }
    
    public static IWorkflowBuilder UploadDirectoryViaSftp(
        this IWorkflowBuilder builder,
        string localDirectoryPath,
        string remoteDirectoryPath)
    {
        return builder.AddStep(new UploadDirectoryViaSftpStep()
        {
            LocalDirectoryPath = localDirectoryPath,
            RemoteDirectoryPath = remoteDirectoryPath
        });
    }

    public static IWorkflowBuilder DisconnectFromSftp(this IWorkflowBuilder builder)
    {
        return builder.AddStep(new DisconnectFromSftpStep());
    }
    
    public static IWorkflowBuilder DownloadFileViaSftp(
        this IWorkflowBuilder builder,
        string remoteFilePath,
        string localFilePath)
    {
        return builder.AddStep(new DownloadFileViaSftpStep()
        {
            RemoteFilePath = remoteFilePath,
            LocalFilePath = localFilePath
        });
    }
    
    
}
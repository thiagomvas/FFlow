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
    
    public static IWorkflowBuilder UploadFileToSftp(
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
    
    
}
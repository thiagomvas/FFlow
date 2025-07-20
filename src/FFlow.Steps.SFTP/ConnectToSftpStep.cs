using FFlow.Core;
    using Renci.SshNet;
    
    namespace FFlow.Steps.SFTP;
    
    /// <summary>
    /// Represents a workflow step to connect to an SFTP server.
    /// </summary>
    public class ConnectToSftpStep : FlowStep
    {
        /// <summary>
        /// Gets or sets the SFTP server host.
        /// </summary>
        public string Host { get; set; } = string.Empty;
    
        /// <summary>
        /// Gets or sets the SFTP server port. Default is 22.
        /// </summary>
        public int Port { get; set; } = 22;
    
        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public string Username { get; set; } = string.Empty;
    
        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    
        /// <summary>
        /// Executes the step to connect to the SFTP server.
        /// </summary>
        /// <param name="context">The flow context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the Host or Username is not set, or if the connection fails.
        /// </exception>
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
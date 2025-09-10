using FFlow.Core;

namespace FFlow.Steps.FileIO;

public static class WorkflowBuilderExtensions
{
    /// <summary>
    /// Adds a step to check if a file exists at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileExists(this WorkflowBuilderBase builder, Action<FileExistsStep> configure)
    {
        var step = new FileExistsStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to check if a file exists at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to check existence of.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileExists(this WorkflowBuilderBase builder, string path,
        Action<FileExistsStep>? configure = null)
    {
        var step = new FileExistsStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all text from a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder,
        Action<FileReadAllTextStep> configure)
    {
        var step = new FileReadAllTextStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all text from a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to read all the text from</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder, string path,
        Action<FileReadAllTextStep>? configure = null)
    {
        var step = new FileReadAllTextStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all text from a file at the specified path and save it to a context key.
    /// </summary>
    /// <param name="path">The path of the file to read all the text from</param>
    /// <param name="saveToKey">The key to store the contents into a <see cref="IFlowContext"/></param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllText(this WorkflowBuilderBase builder, string path, string saveToKey,
        Action<FileReadAllTextStep>? configure = null)
    {
        var step = new FileReadAllTextStep
        {
            Path = path,
            SaveToKey = saveToKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all bytes from a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllBytes(this WorkflowBuilderBase builder,
        Action<FileReadAllBytesStep> configure)
    {
        var step = new FileReadAllBytesStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all bytes from a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to read all the bytes from.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllBytes(this WorkflowBuilderBase builder, string path,
        Action<FileReadAllBytesStep>? configure = null)
    {
        var step = new FileReadAllBytesStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to read all bytes from a file at the specified path and save it to a context key.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to read all the bytes from.</param>
    /// <param name="saveToKey">The key to store the contents into a <see cref="IFlowContext"/>.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileReadAllBytes(this WorkflowBuilderBase builder, string path, string saveToKey,
        Action<FileReadAllBytesStep>? configure = null)
    {
        var step = new FileReadAllBytesStep
        {
            Path = path,
            SaveToKey = saveToKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Touches a file at the configured path, creating it if it doesn't exist or updating its timestamps if it does and configured to do so.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase TouchFile(this WorkflowBuilderBase builder, Action<TouchFileStep> configure)
    {
        var step = new TouchFileStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Touches a file at the specified path, creating it if it doesn't exist or updating its timestamps if it does and configured to do so.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to touch.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase TouchFile(this WorkflowBuilderBase builder, string path,
        Action<TouchFileStep>? configure = null)
    {
        var step = new TouchFileStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Creates a directory at the configured path if it doesn't already exist.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase CreateDirectory(this WorkflowBuilderBase builder,
        Action<CreateDirectoryStep> configure)
    {
        var step = new CreateDirectoryStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Creates a directory at the specified path if it doesn't already exist.
    /// </summary>
    /// <param name="path">The path of the directory to create.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase CreateDirectory(this WorkflowBuilderBase builder, string path,
        Action<CreateDirectoryStep>? configure = null)
    {
        var step = new CreateDirectoryStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write text to a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteText(this WorkflowBuilderBase builder,
        Action<FileWriteTextStep> configure)
    {
        var step = new FileWriteTextStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write text to a file at the specified path, using content from a context key.
    /// </summary>
    /// <param name="path">The path of the file to write the text to.</param>
    /// <param name="contextKey">The key to get the contents to write from an <see cref="IFlowContext"/></param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteText(this WorkflowBuilderBase builder, string path, string contextKey,
        Action<FileWriteTextStep>? configure = null)
    {
        var step = new FileWriteTextStep
        {
            Path = path,
            ContextSourceKey = contextKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write text to a file at the specified path, using content from a context key.
    /// </summary>
    /// <param name="path">The path of the file to write the text to.</param>
    /// <param name="contextKey">The key to get the contents to write from an <see cref="IFlowContext"/></param>
    /// <param name="append">Whether to append instead of overriding file contents. Default is <see langword="false"/> .</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteText(this WorkflowBuilderBase builder, string path, string contextKey,
        bool append,
        Action<FileWriteTextStep>? configure = null)
    {
        var step = new FileWriteTextStep
        {
            Path = path,
            ContextSourceKey = contextKey,
            Append = append
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to append text to a file at the specified path, using content from a context key.
    /// </summary>
    /// <param name="path">The path of the file to append the text to.</param>
    /// <param name="contextKey">The key to get the contents to append from an <see cref="IFlowContext"/></param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileAppendText(this WorkflowBuilderBase builder, string path, string contextKey,
        Action<FileWriteTextStep>? configure = null)
    {
        var step = new FileWriteTextStep
        {
            Path = path,
            ContextSourceKey = contextKey,
            Append = true
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write bytes to a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteBytes(this WorkflowBuilderBase builder,
        Action<FileWriteBytesStep> configure)
    {
        var step = new FileWriteBytesStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write bytes to a file at the specified path, using content from a context key.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to write the bytes to.</param>
    /// <param name="contextKey">The key to get the byte content from an <see cref="IFlowContext"/>.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteBytes(this WorkflowBuilderBase builder, string path, string contextKey,
        Action<FileWriteBytesStep>? configure = null)
    {
        var step = new FileWriteBytesStep
        {
            Path = path,
            ContextSourceKey = contextKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to write bytes to a file at the specified path, using content from a context key,
    /// with an option to append instead of overwriting.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to write the bytes to.</param>
    /// <param name="contextKey">The key to get the byte content from an <see cref="IFlowContext"/>.</param>
    /// <param name="append">Whether to append to the file instead of overwriting. Default is <see langword="false"/>.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileWriteBytes(this WorkflowBuilderBase builder, string path, string contextKey,
        bool append,
        Action<FileWriteBytesStep>? configure = null)
    {
        var step = new FileWriteBytesStep
        {
            Path = path,
            ContextSourceKey = contextKey,
            Append = append
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }

    /// <summary>
    /// Adds a step to append bytes to a file at the specified path, using content from a context key.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="path">The path of the file to append the bytes to.</param>
    /// <param name="contextKey">The key to get the byte content from an <see cref="IFlowContext"/>.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileAppendBytes(this WorkflowBuilderBase builder, string path, string contextKey,
        Action<FileWriteBytesStep>? configure = null)
    {
        var step = new FileWriteBytesStep
        {
            Path = path,
            ContextSourceKey = contextKey,
            Append = true
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to copy a file from a source path to a destination path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase CopyFile(this WorkflowBuilderBase builder,
        Action<CopyFileStep> configure)
    {
        var step = new CopyFileStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to copy a file from a source path to a destination path.
    /// </summary>
    /// <param name="sourcePath">The path of the source file.</param>
    /// <param name="destinationPath">The path of the destination copied file.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase CopyFile(this WorkflowBuilderBase builder, string sourcePath, string destinationPath,
        Action<CopyFileStep>? configure = null)
    {
        var step = new CopyFileStep
        {
            SourcePath = sourcePath,
            DestinationPath = destinationPath
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to move a file from a source path to a destination path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase MoveFile(this WorkflowBuilderBase builder,
        Action<MoveFileStep> configure)
    {
        var step = new MoveFileStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to move a file from a source path to a destination path.
    /// </summary>
    /// <param name="sourcePath">The path of the source file that will be moved.</param>
    /// <param name="destinationPath">The destination path that the file will be moved to.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase MoveFile(this WorkflowBuilderBase builder, string sourcePath, string destinationPath,
        Action<MoveFileStep>? configure = null)
    {
        var step = new MoveFileStep
        {
            SourcePath = sourcePath,
            DestinationPath = destinationPath
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to delete a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase DeleteFile(this WorkflowBuilderBase builder,
        Action<DeleteFileStep> configure)
    {
        var step = new DeleteFileStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to delete a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to delete.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase DeleteFile(this WorkflowBuilderBase builder, string path,
        Action<DeleteFileStep>? configure = null)
    {
        var step = new DeleteFileStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to compute the checksum of a file at the specified path.
    /// </summary>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileChecksum(this WorkflowBuilderBase builder,
        Action<ChecksumStep> configure)
    {
        var step = new ChecksumStep();
        configure(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to compute the checksum of a file at the specified path and save it to a context key.
    /// </summary>
    /// <param name="path">The path of the file to compute the checksum of.</param>
    /// <param name="saveToKey">The key to save the checksum in a <see cref="IFlowContext"/></param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileChecksum(this WorkflowBuilderBase builder, string path, string saveToKey,
        Action<ChecksumStep>? configure = null)
    {
        var step = new ChecksumStep
        {
            Path = path,
            SaveToKey = saveToKey
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to compute the checksum of a file at the specified path.
    /// </summary>
    /// <param name="path">The path of the file to compute the checksum of.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileChecksum(this WorkflowBuilderBase builder, string path,
        Action<ChecksumStep>? configure = null)
    {
        var step = new ChecksumStep
        {
            Path = path
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
    
    /// <summary>
    /// Adds a step to compute the checksum of a file at the specified path and compare it with an expected value.
    /// </summary>
    /// <param name="path">The path of the file to compute the checksum of.</param>
    /// <param name="algorithm">The algorithm to use to compute the checksum.</param>
    /// <param name="expect">The expected checksum.</param>
    /// <param name="builder">The workflow builder instance.</param>
    /// <param name="configure">Optional action to configure the step.</param>
    /// <returns>The same workflow builder instance for chaining.</returns>
    public static WorkflowBuilderBase FileChecksum(this WorkflowBuilderBase builder, 
        string path, 
        ChecksumStep.ChecksumAlgorithm algorithm, 
        string expect,
        Action<ChecksumStep>? configure = null)
    {
        var step = new ChecksumStep
        {
            Path = path,
            Algorithm = algorithm,
            CompareWith = expect
        };
        configure?.Invoke(step);
        builder.AddStep(step);
        return builder;
    }
}
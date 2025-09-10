# Manipulating Files with FFlow
FFlow provides a package for manipulating files as part of your workflow. This is particularly useful for reading from or writing to files, processing file contents, or managing file system operations.
To use the File I/O extensions and steps, you need to install the `FFlow.Steps.FileIO` package:
```bash
dotnet add package FFlow.Steps.FileIO
```

The package includes the following commands and overloads:
- FileChecksum
- CopyFile
- CreateDirectory
- TouchFile
- DeleteFile
- MoveFile
- ReadFile (Text and Bytes)
- WriteFile (Text and Bytes)
- AppendToFile (Text and Bytes)
- RenameFile
- FileExists

These either take the file path as a string or an `Action<StepType>` to configure the operation. Each step defines its output in the context, which can be obtained through `IFlowContext.GetOutputFor<StepType, OutputType>()`.
### Example Usage
```csharp
builder.FileChecksum("path/to/file.txt", ChecksumStep.ChecksumAlgorithm.MD5, "some-checksum")
    .Finally((ctx, ct) => {
        var checksum = ctx.GetOutputFor<FileChecksumStep, string>();
        Console.WriteLine($"File Checksum: {checksum}");
    });
```

### File Exists
You can check if a file exists using the `FileExists` step, you can either call it by passing the path to the file or configuring via an Action

```csharp
builder.FileExists("path/to/file.txt")
    .Finally((ctx, ct) => {
        var exists = ctx.GetOutputFor<FileExistsStep, bool>();
        Console.WriteLine($"File exists: {exists}");
    });
```
You can also configure it to throw an exception if the file doesn't exist by setting the `ThrowIfNotExists` property.

### Reading text from a file
You can read text from a file using the `FileReadAllText` step, you can either call it by passing the path to the file or configuring via an Action

```csharp
builder.FileReadAllText("path/to/file.txt")
    .Finally((ctx, ct) => {
        var content = ctx.GetOutputFor<FileReadAllTextStep, string>();
        Console.WriteLine($"File content: {content}");
    });
```

Optionally, you can also specify a key to save into an `IFlowContext` either via the Action or the overload that takes a key parameter.

### Reading bytes from a file
You can read bytes from a file using the `FileReadAllBytes` step, you can either call it by passing the path to the file or configuring via an Action

```csharp
builder.FileReadAllBytes("path/to/file.bin")
    .Finally((ctx, ct) => {
        var bytes = ctx.GetOutputFor<FileReadAllBytesStep, byte[]>();
        Console.WriteLine($"File bytes length: {bytes.Length}");
    });
```
Optionally, you can also specify a key to save into an `IFlowContext` either via the Action or the overload that takes a key parameter.

### Writing text to a file
You can write text to a file using the `FileWriteText` step, you can either call it by passing the path to the file and the `IFlowContext` key containing the content or configuring via an Action

```csharp
builder.FileWriteText("path/to/file.txt", "my-content-key")
    .Finally((ctx, ct) => {
        Console.WriteLine("File written successfully.");
    });
```

You can also optionally choose to append to the file instead of overwriting it by setting the `Append` property to true, as well as appending into a new line by setting the `AppendNewLine` to true.

### Writing bytes to a file
You can write bytes to a file using the `FileWriteBytes` step, you can either call it by passing the path to the file and the `IFlowContext` key containing the byte array or configuring via an Action

```csharp
builder.FileWriteBytes("path/to/file.bin", "my-bytes-key")
    .Finally((ctx, ct) => {
        Console.WriteLine("File written successfully.");
    });
```

You can also optionally choose to append to the file instead of overwriting it by setting the `Append` property to true.

### Appending text to a file
You can append text to a file using the `FileAppendText` step, you can either call it by passing the path to the file and the `IFlowContext` key containing the content or configuring via an Action

```csharp
builder.FileAppendText("path/to/file.txt", "my-content-key")
    .Finally((ctx, ct) => {
        Console.WriteLine("File appended successfully.");
    });
```

> [!NOTE]
> This works exactly like the `FileWriteText` step, but it will always append to the file.

### Appending bytes to a file
You can append bytes to a file using the `FileAppendBytes` step, you can either call it by passing the path to the file and the `IFlowContext` key containing the byte array or configuring via an Action

```csharp
builder.FileAppendBytes("path/to/file.bin", "my-bytes-key")
    .Finally((ctx, ct) => {
        Console.WriteLine("File appended successfully.");
    });
```

> [!NOTE]
> This works exactly like the `FileWriteBytes` step, but it will always append to the file.

### Copying a file
You can copy a file using the `CopyFile` step, you can either call it by passing the source and destination paths or configuring via an Action

```csharp
builder.CopyFile("path/to/source.txt", "path/to/destination.txt")
    .Finally((ctx, ct) => {
        Console.WriteLine("File copied successfully.");
    });
```
You can also configure options such as overwriting existing files by setting the `Overwrite` property to true.

### Moving a file
You can move a file using the `MoveFile` step, you can either call it by passing the source and destination paths or configuring via an Action

```csharp
builder.MoveFile("path/to/source.txt", "path/to/destination.txt")
    .Finally((ctx, ct) => {
        Console.WriteLine("File moved successfully.");
    });
```
You can also configure options such as overwriting existing files by setting the `Overwrite` property to true.

### Deleting a file
You can delete a file using the `DeleteFile` step, you can either call it by passing the path to the file or configuring via an Action

```csharp
builder.DeleteFile("path/to/file.txt")
    .Finally((ctx, ct) => {
        Console.WriteLine("File deleted successfully.");
    });
```

### Deleting a directory
You can delete a directory using the `DeleteDirectory` step, you can either call it by passing the path to the directory or configuring via an Action

```csharp
builder.DeleteDirectory("path/to/directory")
    .Finally((ctx, ct) => {
        Console.WriteLine("Directory deleted successfully.");
    });
```

You can also configure options such as recursive deletion by setting the `Recursive` property to true.

### Checksum
You can compute the checksum of a file using the `FileChecksum` step, you can either call it by passing the path to the file, the algorithm and an optional expected checksum to validate against, or configuring via an Action. By default it uses SHA256 as the algorithm.

```csharp
builder.FileChecksum("path/to/file.txt", ChecksumStep.ChecksumAlgorithm.MD5, "some-checksum")
    .Finally((ctx, ct) => {
        var checksum = ctx.GetOutputFor<FileChecksumStep, string>();
        Console.WriteLine($"File Checksum: {checksum}");
    });
```

Or, if you simply want to compute it without validating against an expected checksum:

```csharp
builder.FileChecksum("path/to/file.txt")
    .Finally((ctx, ct) => {
        var checksum = ctx.GetOutputFor<FileChecksumStep, string>();
        Console.WriteLine($"File Checksum: {checksum}");
    });
```
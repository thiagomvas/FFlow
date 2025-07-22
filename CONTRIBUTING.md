# Contributing to FFlow

🎉 Thanks for taking the time to contribute to **FFlow**!  
This project aims to be a modular, scalable, and developer-friendly workflow engine for .NET, and your help is invaluable in making that happen.

Please read this guide before submitting issues, features, or pull requests.


## 💡 Before You Start

- **Check existing issues first** to avoid duplicates.
- For **questions or discussion**, consider opening a “Question” issue or joining our community chat.


## 📁 Repository Structure

FFlow is a modular ecosystem of packages. Please try to isolate changes within the correct package or create a new one when appropriate

| Package | Description |
|--------|-------------|
| `FFlow` | Entry point for building and running workflows |
| `FFlow.Core` | Core abstractions and interfaces |
| `FFlow.Cli` | CLI tool for running, simulating, and scaffolding workflows |
| `FFlow.Steps.*` | Step extensions (e.g. Shell, Dotnet, SFTP) |
| `FFlow.Observability.*` | Logging, tracing, metrics integrations |
| `FFlow.Scheduling` | Delayed or scheduled execution support |

And so on

## 🐞 Reporting Bugs

Use the **Bug Report** issue template.

Include:
- A **minimal reproducible snippet**
- What you **expected to happen**
- What **actually happened**
- Environment info (.NET version, OS, package version)

## 🚀 Requesting Features

Use the **Feature Request** template and explain:
- What you’re proposing
- Why it matters (the problem it solves)
- Where it fits in the ecosystem
- Which package it affects (`FFlow`, `FFlow.Core`, etc.)


## 🛠 Contributing Code

### 1. Fork the repository  
Create a feature branch with a clear name.

### 2. Follow our basic code style
- Use `var` when the type is obvious.
- Use expression-bodied members where appropriate.
- Public APIs must have XML doc comments.
- Use `internal` for anything not meant to be exposed across packages.

> [!NOTE]
> All public-facing APIs are assumed to be *potentially consumed via source generators*, so avoid dynamic patterns and reflection when possible.

### 3. Write tests for your changes
- New steps must include **unit tests** with realistic context usage.

### 4. Document your changes
- Update XML docs or the documentation page for the affected package.
- For larger features, explain the reasoning in the PR description.


## ✅ Pull Request Process

1. Ensure your code **builds and passes tests**.
2. Link to the issue you're fixing with `Fixes #123`.


## ✅ Git Commit Tips

- Follow the format: `feat(<package>): description`, e.g.
    `feat(cli): add new command for workflow simulation`
- Use `fix(<package>): description` for bug fixes.
- Use `docs(<package>): description` for documentation changes.
- Use `refactor(<package>): description` for code improvements that don't change behavior.
- Use `chore(<package>): description` for maintenance tasks (e.g. updating dependencies).
- Use `test(<package>): description` for adding or updating tests.
- Use `style(<package>): description` for changes that do not affect the code's logic (e.g. formatting).
- Use `perf(<package>): description` for performance improvements.

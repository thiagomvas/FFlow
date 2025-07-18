---
title: Introduction
description: Fluent, code-first workflow automation for .NET
---

## Overview
**FFlow** is a fluent and code-first workflow automation library for .NET. It is designed to help developers orchestrate business logic, automation tasks and CI/CD pipelines using simple, readable syntax.

The main objective with the library is to achieve **production-ready pipelines** where a developer can **fully test** them in their machine to ensure correct deployment, giving more flexibility on what an automation can and can't do. If there isn't a script for it then writing one yourself that fully integrates into the ecosystem should not be a problem.

It supports branching for parallel execution, decorator patterns for extending the steps to add more functionality like observability and validation, as well as full support for Dependency Injection using the standard `IServiceProvider` interface. 

<div class="hero-buttons">
  <a class="btn btn-primary" href="./getting-started.html">Get Started</a>
  <a class="btn btn-outline" href="https://github.com/thiagomvas/FFlow" target="_blank">View on GitHub</a>
</div>

---

## Terminology
In general, FFlow uses the following terminology:
- **Workflow**: A sequence of steps that define a process or automation task.
- **Step**: An individual action or operation within a workflow.
- **Branch**: A parallel execution path within a workflow.

---

## How it works
FFlow allows you to define workflows using a fluent API, which makes it easy to read and write. You can create workflows by chaining steps together, and you can use branches to run multiple steps in parallel.

Workflows are built using `IWorkflowBuilder`, which provides methods to add steps and branches. Each step can be a simple action, a complex operation, or even a call to an external service. The default `FFlowBuilder` also resolves dependencies using the standard `IServiceProvider` interface, allowing you to inject services and other dependencies into your steps.

An `IFlowContext` passes the data through each step in the workflow, allowing you to share state and data between steps. This context can be used to store results, pass parameters, and manage the flow of execution.


## Why it exists
Writing and testing CI/CD pipelines has always been frustrating. It usually went from "waiting to compile" to "waiting for CI/CD", just to realize you missed something, fix it, and rerun the whole thing again. And again.

The feedback loop was too long. Small mistakes led to wasted time, and workflows often lived outside the codebase in YAML files or GUI editors that were hard to test, debug, or reuse.

FFlow was born out of this frustration. **It came from the idea that automation should feel like regular code.** Something you can write fluently, test locally, and plug into your existing services just like anything else in your app.

Tools like `Cake` or `Nuke` solve part of the problem, but I wanted something more structured and flexible. Less about running scripts. More about building flows.
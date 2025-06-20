# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="1.0.0-alpha.2"></a>
## [1.0.0-alpha.2](https://www.github.com/thiagomvas/FFlow/releases/tag/v1.0.0-alpha.2) (2025-06-20)

### Features

* Add automatic assembly discovery for registering services ([3e486e4](https://www.github.com/thiagomvas/FFlow/commit/3e486e41d7ba410639d516f5800da0a7c1f32e90))
* Add base FlowStep class for providing a base for common implementations ([920abc2](https://www.github.com/thiagomvas/FFlow/commit/920abc2c785f4535a5c2b5630d166a36d1f37d1d))
* Add CheckForKey to validate if a key is present ([b1f24fb](https://www.github.com/thiagomvas/FFlow/commit/b1f24fbe8a1f108f4c581632ac61e0290eae3343))
* Add CheckForRegexPattern to validate strings using regex ([c50488c](https://www.github.com/thiagomvas/FFlow/commit/c50488c284f806124b7d154eef31067e26b3d936))
* Add IConfigurableStepBuilder to allow more specific configuration for individual steps ([2dd9087](https://www.github.com/thiagomvas/FFlow/commit/2dd90877fe4526d8f0c9ee3a8e8ee0aa53e0fbfe))
* Add IFlowContext.LoadEnvironmentVariables extension ([b61740f](https://www.github.com/thiagomvas/FFlow/commit/b61740f9879734c629d3bf632133f2903401253d))
* Add input overloads for easier configuration ([fcab480](https://www.github.com/thiagomvas/FFlow/commit/fcab480cbce09a67424716fbf093d91f0d9b5738))
* Add Input() for allowing setting properties inside steps ([19c19d7](https://www.github.com/thiagomvas/FFlow/commit/19c19d724b66479f9aaf14b5eeb1977c6c858198))
* Add not empty step ([5d5340b](https://www.github.com/thiagomvas/FFlow/commit/5d5340b1d7104bb3d9a19e97009696b6518f4520))
* Add output setter steps ([9ea605f](https://www.github.com/thiagomvas/FFlow/commit/9ea605f1c90d29c1f9b7a9002081fca4700e3378))
* Add RequireKey attribute ([97ed9e0](https://www.github.com/thiagomvas/FFlow/commit/97ed9e0b45184a07ad440061cb2e21bc7646dfd1))
* Add RequireKeys to check multiple keys at once ([0f9f7e2](https://www.github.com/thiagomvas/FFlow/commit/0f9f7e2b70645495c2f1411a72ad2c28a551353b))
* Add RequireNotNull attribute ([ced717c](https://www.github.com/thiagomvas/FFlow/commit/ced717c4ecea6608d53598e93e9c33976e0a4320))
* Add RequireNotNull checks to validate if keys have a not null value ([397f664](https://www.github.com/thiagomvas/FFlow/commit/397f664b0a67bf1e355de6f04f0922167de376d0))
* Add RequireRegex attribute ([cbe4de5](https://www.github.com/thiagomvas/FFlow/commit/cbe4de58c6ca28aa7a650e5342cd27fec7d89c2a))
* Add Retry Policies support ([dee6fd5](https://www.github.com/thiagomvas/FFlow/commit/dee6fd56af1a8e7455d60cb6b0a5144fa9671659))
* Add validation attribute discovery and decorators ([a42b755](https://www.github.com/thiagomvas/FFlow/commit/a42b7559e71dff305f905627335fdc84218cb553))
* Allow delegate steps to be configured ([ee3fd4f](https://www.github.com/thiagomvas/FFlow/commit/ee3fd4f5c148ffa294a91a01de9314a89d43a311))
* Dotnet steps are now configured via properties rather than context ([7a2fa59](https://www.github.com/thiagomvas/FFlow/commit/7a2fa59f5ca8b6b3d7b24e19e97d4e7c8b8481f0))
* Finally sets step as a finalizer to run regardless of workflow success or failure ([a19e1dd](https://www.github.com/thiagomvas/FFlow/commit/a19e1dd9793d0436519fe33ec3d99c3c6d97e9fa))
* Register input and outputs into the context ([f5aa540](https://www.github.com/thiagomvas/FFlow/commit/f5aa540e87ff0eb6d84b12f24f0310ff1633f408))
* Throw a custom validation exception rather than general purpose ones ([f6752f7](https://www.github.com/thiagomvas/FFlow/commit/f6752f7fb40ce96898ee44199091dd0176d6392d))

### Bug Fixes

* Fix incorrect relative source code path ([1278975](https://www.github.com/thiagomvas/FFlow/commit/12789753092d65364c38173ca3bd1fe5c4f94171))

<a name="1.0.0-alpha.1"></a>
## [1.0.0-alpha.1](https://www.github.com/thiagomvas/FFlow/releases/tag/v1.0.0-alpha.1) (2025-06-14)

### Features

* Add Forking for parallel processing ([0172fc3](https://www.github.com/thiagomvas/FFlow/commit/0172fc3632446542ae6b11b752cadd8f6fcf3d61))
* Add IWorkflowBuilder.Delay ([6c7c5cd](https://www.github.com/thiagomvas/FFlow/commit/6c7c5cd2e59fce758f98aa1eb7416200f4ccfeb5))
* Add Observer Pattern support with IFlowEventListeners ([7c32747](https://www.github.com/thiagomvas/FFlow/commit/7c327470909da7c582d96a176c9d42e3ce9be39f))
* Add Options container ([0c15760](https://www.github.com/thiagomvas/FFlow/commit/0c1576051d46dfd00b580f8c0213bc2e3dee965e))
* Add steps for throwing exceptions with and without condition ([170ccf0](https://www.github.com/thiagomvas/FFlow/commit/170ccf0b837f89034ccd8c92fc75bb0894eb0980))
* Add support for a sync error handler ([30aa189](https://www.github.com/thiagomvas/FFlow/commit/30aa189f57753352ae3ec983fff8e76952e2654f))
* Add syncronous overloads to IWorkflowBuilder by wrapping the actions in a Task ([3024363](https://www.github.com/thiagomvas/FFlow/commit/30243636e7fe16f35c1b19ec2a9993ed27f45419))
* Improve exception message for when a step can't be created due to not being registered in DI or having a parameterless constructor ([4b31735](https://www.github.com/thiagomvas/FFlow/commit/4b31735b615773efc654af89a7fd54dda49c6f0b))
* Throw StepCreationException when creating steps fails ([e561668](https://www.github.com/thiagomvas/FFlow/commit/e5616683fb0620b37fa6ce6787d321410b5729dd))

### Bug Fixes

* FireAndForget fork strategy no longer causes subthreads to be killed when the main one is done. It now waits for all to finish before finishing the workflow ([88caaa0](https://www.github.com/thiagomvas/FFlow/commit/88caaa0920eb8070452a5ad9595a2fe6fe2393e9))
* Register base IWorkflowDefinition and IFlowStep interfaces as well in DI container ([be81217](https://www.github.com/thiagomvas/FFlow/commit/be812173bf1178a82f19d5e86a22108f82bd4fdb))
* Register step types by themselves as well as by the interface ([c421861](https://www.github.com/thiagomvas/FFlow/commit/c421861d0c75a72ce81ac9f933789367183d062c))
* Throw all task exceptions when awaiting forked steps rather than only the first one that throws. ([0ece32e](https://www.github.com/thiagomvas/FFlow/commit/0ece32ea067c059b1d0cb78062198aa557c9573d))

<a name="1.0.0-alpha.0"></a>
## [1.0.0-alpha.0](https://www.github.com/thiagomvas/FFlow/releases/tag/v1.0.0-alpha.0) (2025-06-10)

### Features

* Add Builder Step for using builders to generate step sequences in branching workflows ([d6fba91](https://www.github.com/thiagomvas/FFlow/commit/d6fba91ce61c23e3bf7e49efbe2b307a40db6506))
* Add CancellationToken support for cancelling execution ([75c2064](https://www.github.com/thiagomvas/FFlow/commit/75c2064b194694d473b7dc22c452feb9c2035ec4))
* Add Configuration object ([563780b](https://www.github.com/thiagomvas/FFlow/commit/563780bce848713952e63da6cec2107bcaadd208))
* Add ContinueWith to continue the steps with a workflow definition instead of a single step ([74c5788](https://www.github.com/thiagomvas/FFlow/commit/74c5788271e648ee50bffd5ea6da3135620cd16d))
* Add DI support using IServiceProvider ([09b53db](https://www.github.com/thiagomvas/FFlow/commit/09b53db5d5039c1686c46e3e59be1dd69242c4a5))
* Add Dotnet Build Step ([7924c92](https://www.github.com/thiagomvas/FFlow/commit/7924c92a18dfe927ad8075b2bf4d1d1928ac0ad6))
* Add Dotnet Pack Step ([b178442](https://www.github.com/thiagomvas/FFlow/commit/b1784428e7599b468d4dd3da854f7587c4f4cc52))
* Add Dotnet Publish command ([01c23d9](https://www.github.com/thiagomvas/FFlow/commit/01c23d9a7a940eb18194520e81b743bda490f181))
* Add Dotnet Run Step ([03bbf6b](https://www.github.com/thiagomvas/FFlow/commit/03bbf6bf39f94e4d8ad851969dcf61e9fc796f37))
* Add Dotnet Test Step ([19e5348](https://www.github.com/thiagomvas/FFlow/commit/19e5348113976be6cee891a631ad047edb490881))
* Add DotnetRestore step ([e75c492](https://www.github.com/thiagomvas/FFlow/commit/e75c4928e364b314197426dad0833c232c69e204))
* Add ForEach processor ([4554fd1](https://www.github.com/thiagomvas/FFlow/commit/4554fd1a0b4778e63d5bd83b88b43470b6052dfc))
* Add functions to change a workflows context ([d7c871b](https://www.github.com/thiagomvas/FFlow/commit/d7c871b8bdf08dc872abef60a16c1fb33f436d27))
* Add global error handling ([22838aa](https://www.github.com/thiagomvas/FFlow/commit/22838aa2f953948c5ced71d85f212d402c207a6d))
* Add If builder method ([668f1a7](https://www.github.com/thiagomvas/FFlow/commit/668f1a7f074fafc7e6e6d0df9b89e37145892d78))
* Add If builder methods with generic steps ([1ce9cda](https://www.github.com/thiagomvas/FFlow/commit/1ce9cda1bc6385c1fed6c72ccbe805a8f5379ff3))
* Add individual configurations for the steps rather than a global one ([b3423a7](https://www.github.com/thiagomvas/FFlow/commit/b3423a74553487959719c1f669d2c418849ab52d))
* Add Microsoft Dependency Injection registration extension ([511ca9b](https://www.github.com/thiagomvas/FFlow/commit/511ca9b0bb7fe38e0f28252298ac6bf276c13ab4))
* Add step results to dotnet package ([32b8122](https://www.github.com/thiagomvas/FFlow/commit/32b812286317b71724ea33b0744c784718e643ec))
* Add Switch case step ([043083c](https://www.github.com/thiagomvas/FFlow/commit/043083c5d03b9d83e73db701d9cb467138610ae0))

### Bug Fixes

* Builder Step now forwards the context instead of generating a new one altogether ([4e6a2f8](https://www.github.com/thiagomvas/FFlow/commit/4e6a2f8654e43ba8bd487c5260e155d6d5aad870))


# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="1.0.0"></a>
## [1.0.0](https://www.github.com/thiagomvas/FFlow/releases/tag/v1.0.0) (2025-07-02)

### Features

* Add attributes to attaching metadata to steps ([a0b1b2f](https://www.github.com/thiagomvas/FFlow/commit/a0b1b2fad7f73549f5ad0552b31ff25921df06e5))
* Add basic single time flow scheduling ([6310369](https://www.github.com/thiagomvas/FFlow/commit/631036908a15810a7d78d32750312a14ed515377))
* Add Cron support ([18ab007](https://www.github.com/thiagomvas/FFlow/commit/18ab007b90f3a24b6aafc75973f973db04ee9526))
* Add default pipeline logger for easy logging in cicd scripts ([03d3092](https://www.github.com/thiagomvas/FFlow/commit/03d309213fd3873fcf9255cb869727d526329236))
* Add file based schedule store ([9190a05](https://www.github.com/thiagomvas/FFlow/commit/9190a0578021d9614467f5f65a7be903278b2d7c))
* Add generic Flow Step Builder for less verbose step configuration ([43a69ff](https://www.github.com/thiagomvas/FFlow/commit/43a69ffe7530a5afd4bb9ac3b56f882577132cb6))
* Add recurring job support ([cc6d9cf](https://www.github.com/thiagomvas/FFlow/commit/cc6d9cfcd1229a4b06bb41ac7cddd9944bcd10fb))
* Add reference to executing workflow in flow context using SetSingleValue ([1594d08](https://www.github.com/thiagomvas/FFlow/commit/1594d087011452ea1729a6a98ff82471342358ac))
* Add Scheduler Options ([898d162](https://www.github.com/thiagomvas/FFlow/commit/898d162b1fc13f9522486722bd7dd23640ca8161))
* Add SilentStep attribute ([0549799](https://www.github.com/thiagomvas/FFlow/commit/0549799060ea7f1eb89ea46256257efef281a79c))
* Add step skipping support ([1669987](https://www.github.com/thiagomvas/FFlow/commit/166998766ba373270043e548841075ae7a6dbc41))
* Add step templating through a Template Registry ([fa5a552](https://www.github.com/thiagomvas/FFlow/commit/fa5a5521f54c1b51c4c5d70392e4c52c6369a3e7))
* Add StepTags attribute ([853881c](https://www.github.com/thiagomvas/FFlow/commit/853881c4fb907f254d783be1c1f7c28f78a9c630))
* Add workflow schedule building extension ([1fcc2e1](https://www.github.com/thiagomvas/FFlow/commit/1fcc2e111c88efe53a805495c9b10ee410fe13a6))

### Bug Fixes

* Make input parameter optional for IWorkflow.RunAsync ([aaa3f93](https://www.github.com/thiagomvas/FFlow/commit/aaa3f9300fe1e54969e2ea0ae329da6ab0e9efe5))
* Remove context id from metric listener to prevent spam ([0d0e9e5](https://www.github.com/thiagomvas/FFlow/commit/0d0e9e59622abd3390d5834a38c690bfdc0c6945))
* Require store registration instead of automatically registering the store. ([dc5413d](https://www.github.com/thiagomvas/FFlow/commit/dc5413d4ff39a2d32c2b860aa270cd4b2e4531b6))
* Schedule Runner no longer waits polling time when the next workflow is due sooner ([e1562d5](https://www.github.com/thiagomvas/FFlow/commit/e1562d5e6859c7e29f8972fe72d41ef6a48dd079))
* Schedule Runner options are now correctly injected ([41cb90a](https://www.github.com/thiagomvas/FFlow/commit/41cb90a1e41b8dda06039226d0120b52e3c16920))
* Use correct factory signature for If and ForEach overloads ([6ca9d73](https://www.github.com/thiagomvas/FFlow/commit/6ca9d7355b4841a8cb3e9103d7808265c9658325))

<a name="1.0.0-alpha.3"></a>
## [1.0.0-alpha.3](https://www.github.com/thiagomvas/FFlow/releases/tag/v1.0.0-alpha.3) (2025-06-24)

### Features

* Add compensation support ([2f39877](https://www.github.com/thiagomvas/FFlow/commit/2f398772771cd317cf1b263382bdaf3fe6ace7f1))
* Add compensation support for conditional step ([2167409](https://www.github.com/thiagomvas/FFlow/commit/2167409bcbce329dc0789cb6e41fb49729c77bbc))
* Add compensation support for ContinueWith (aka subworkflow) step ([97f5865](https://www.github.com/thiagomvas/FFlow/commit/97f58653f531a17092405a320ec2773ff41893ff))
* Add compensation support for switch cases ([dcf945c](https://www.github.com/thiagomvas/FFlow/commit/dcf945ca3d2789195c21a34cea3bfc4d6671e9e6))
* Add ConsoleFlowEventListener ([63da69e](https://www.github.com/thiagomvas/FFlow/commit/63da69e1e25f243f7a280fa9e2b77147762c02f7))
* Add context injection to commands using {context:Key} syntax ([160791f](https://www.github.com/thiagomvas/FFlow/commit/160791ff4c4a6e01f4d0ccddc11f3b7dc5546d7a))
* Add Dotnet CLI Extension methods for more convenient use ([dd0e23e](https://www.github.com/thiagomvas/FFlow/commit/dd0e23e59af6ef8236b70cbc3623a1a6a47509f3))
* Add Dotnet Nuget Push step ([72889f7](https://www.github.com/thiagomvas/FFlow/commit/72889f7c437035cdb4cd6cd8e2a8bcb3e793a4fb))
* Add extensions for getting and setting the context id ([56126b3](https://www.github.com/thiagomvas/FFlow/commit/56126b3ff7f10b10ea8a27426957a62df172a23a))
* Add Gauges to IMetricsSink ([3ae2581](https://www.github.com/thiagomvas/FFlow/commit/3ae25817498fbddef960889417a7a793ddc11572))
* Add GetLastInput/Output methods ([b731ba0](https://www.github.com/thiagomvas/FFlow/commit/b731ba02080a667b65113e57733275044caf29f9))
* Add IFlowContext extensions for getting metrics sinks and event listeners ([795112c](https://www.github.com/thiagomvas/FFlow/commit/795112c7d1031fcd41668bb822da71390353dddb))
* Add IMetricSink with in memory implementation ([4e44fc2](https://www.github.com/thiagomvas/FFlow/commit/4e44fc2ac761d6ed3fc42d167a102d85c45a0d83))
* Add IWorkflow.CompensateAsync() ([bd46af1](https://www.github.com/thiagomvas/FFlow/commit/bd46af1522f30df817b6a4a366df9b17d10356c0))
* Add IWorkflowMetadata to attach metadata properties in workflows ([dae9c37](https://www.github.com/thiagomvas/FFlow/commit/dae9c37c868e844c19a0dc313a49c63e4ac03ef7))
* Add IWorkflowMetadataStore to store workflow metadata rather than adding properties. ([1f85849](https://www.github.com/thiagomvas/FFlow/commit/1f85849d661f1d019155945bb0056d41e1b88705))
* Add MetricsSnapshot and IMetricsSink.Flush ([6ff313e](https://www.github.com/thiagomvas/FFlow/commit/6ff313e966eaf143cdf1b49ec0e1087bc0dffb24))
* Add MetricTrackingListener ([f0da764](https://www.github.com/thiagomvas/FFlow/commit/f0da764a60a5e875df185047d1b5eb8b7710049c))
* Add new base WorkflowDefinition class to introduce default behaviours for workflow definitions ([eb5f7ed](https://www.github.com/thiagomvas/FFlow/commit/eb5f7edf4cee68cf15e184472d1c102311393d9a))
* Add NoOpMemorySink ([193e43e](https://www.github.com/thiagomvas/FFlow/commit/193e43e8916d18bc037b01178f7df09aee310c3b))
* Add Run Command step ([709851e](https://www.github.com/thiagomvas/FFlow/commit/709851e5faf6da2d365e19916bac0f202f615797))
* Add RunScriptFile extension method ([f0fc4c1](https://www.github.com/thiagomvas/FFlow/commit/f0fc4c1066c2631c8c7b9bbeca635778a50fa6d6))
* Add RunScriptRaw step ([2620c39](https://www.github.com/thiagomvas/FFlow/commit/2620c3984782063cfcfd340df2c867765a034285))
* Add Timeline Recorder ([1125f08](https://www.github.com/thiagomvas/FFlow/commit/1125f08ff66a1cba9b3aac77e3c307694146d687))
* Add UseMetrics extension to attach an IMetricSink to a FFlowBuilder ([a836136](https://www.github.com/thiagomvas/FFlow/commit/a83613648fcc7e8ae932c833ba705d8abbb8c2b5))
* Refactor IFlowContext to be more convenient when getting values from the workflow ([487b879](https://www.github.com/thiagomvas/FFlow/commit/487b879ef34bb2c661b28a8041143ad140487c68))
* Shell executions now set the exit code as output rather than logging to output ([3dc808c](https://www.github.com/thiagomvas/FFlow/commit/3dc808cd4c31139fdf8e1306f0bbb56b73e2f8d7))

### Bug Fixes

* Dotnet Nuget Push source now defaults to nuget.org ([8ebbc24](https://www.github.com/thiagomvas/FFlow/commit/8ebbc24b23e0316146fc8f139de2d83fd5a886aa))
* Log exit code for shell related step ([cc6cc8b](https://www.github.com/thiagomvas/FFlow/commit/cc6cc8b1d91a4673c4913c4b18535671827734a5))
* Register workflow definition types as well ([b8d38eb](https://www.github.com/thiagomvas/FFlow/commit/b8d38eb0775b784ccbea1a8ef3041dcec9ece512))
* Shell steps no longer log empty lines to output handler ([0bc04f1](https://www.github.com/thiagomvas/FFlow/commit/0bc04f116e33675db2c73055d7d073f8da5d2050))
* Use temp file for scripts instead of raw strings to properly support env variables ([c8e53c5](https://www.github.com/thiagomvas/FFlow/commit/c8e53c577ee9a86d429b34ecf3726a494103861c))
* Workflow no longer stops compensation when the last step isnt compensable ([075d08b](https://www.github.com/thiagomvas/FFlow/commit/075d08b031222291695452b494ff118c5995f207))

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


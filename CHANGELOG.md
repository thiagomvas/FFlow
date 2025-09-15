# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="1.2.3"></a>
## [1.2.3](https://www.github.com/thiagomvas/fflow/releases/tag/v1.2.3) (2025-09-15)

### Bug Fixes

* Fork step no longer crashes when visualizing a broken graph ([e431667](https://www.github.com/thiagomvas/fflow/commit/e431667df8f66ee4d8edde0b81c42f30cab1983f))
* Graphs are included when building a Workflow ([ec904b9](https://www.github.com/thiagomvas/fflow/commit/ec904b9ffd91e38385af9fd2e0d3df7b8b6f94a7))
* Missing foreach step visualization ([cf2641c](https://www.github.com/thiagomvas/fflow/commit/cf2641c61ba88bc3481f7e61cf32ed8e3e957793))

<a name="1.2.2"></a>
## [1.2.2](https://www.github.com/thiagomvas/fflow/releases/tag/v1.2.2) (2025-09-10)

### Bug Fixes

* Fix FFlow.Steps.Http version ([f184b93](https://www.github.com/thiagomvas/fflow/commit/f184b933e3e9dac80b3d1bd4033dbaab7acc66eb))

<a name="1.2.1"></a>
## [1.2.1](https://www.github.com/thiagomvas/fflow/releases/tag/v1.2.1) (2025-09-10)

### Bug Fixes

* Fix CLI version ([69a2b7e](https://www.github.com/thiagomvas/fflow/commit/69a2b7e9da15149a9db07ac93caf3fae624eb1ec))

<a name="1.2.0"></a>
## [1.2.0](https://www.github.com/thiagomvas/fflow/releases/tag/v1.2.0) (2025-09-10)

### Features

* Add a way to set where to continue from in the graph ([431a1ab](https://www.github.com/thiagomvas/fflow/commit/431a1ab0a4b5f17851735773fd9bce37e5217080))
* Add base http request step ([83ed3ca](https://www.github.com/thiagomvas/fflow/commit/83ed3ca388ba968428565a2d92f0504cdb253a62))
* Add basic graph building ([f9419b4](https://www.github.com/thiagomvas/fflow/commit/f9419b4621a38587d8632524f2e84ea19064d912))
* Add basic visualization graph structure ([a4495a5](https://www.github.com/thiagomvas/fflow/commit/a4495a57cda5e241df345f7b1d37499d97cd0529))
* Add ConfigureAwait(false) to all await statements in library code ([b24a592](https://www.github.com/thiagomvas/fflow/commit/b24a59218831d3fcb5c4da528fbd786adc861209))
* Add ContentType configuration support ([d7f887b](https://www.github.com/thiagomvas/fflow/commit/d7f887b904c3fe4f5d672f4cc85e69f01e4c261c))
* Add ContinueFromLabel to set a custom label for the exiting edge ([0af17fa](https://www.github.com/thiagomvas/fflow/commit/0af17fa144a109e6319afb779226047c6f1545ab))
* Add custom graph for foreach step ([da49b3e](https://www.github.com/thiagomvas/fflow/commit/da49b3ef2e480efe1b71307f23871f1b888ff091))
* Add custom graph for Fork step ([fd11a3a](https://www.github.com/thiagomvas/fflow/commit/fd11a3a00c1c673f20b76b4a613f8c889ad2bb3d))
* Add custom graph for If node ([990b565](https://www.github.com/thiagomvas/fflow/commit/990b5656d0442129618562f1f512fdb780eb8340))
* Add describable steps support so steps can generate their own graphs ([97c7e75](https://www.github.com/thiagomvas/fflow/commit/97c7e75944d2f5cae5c5deb1f6d2e1ffde41e707))
* Add Describe() to base WorkflowDefinition to support describing definitions ([71f1599](https://www.github.com/thiagomvas/fflow/commit/71f1599686ca89684815e69f1d701f80fe3cbb37))
* Add DOT Graph support for visualization ([6bbaed5](https://www.github.com/thiagomvas/fflow/commit/6bbaed5ee2c46b2192bd1711b1d81d3cc098dbfe))
* Add extension methods for sending http requests with all methods ([12eef3e](https://www.github.com/thiagomvas/fflow/commit/12eef3ecc99a8d895ad209e597c39423713e6fc0))
* Add File Exists Step ([bd1f3c8](https://www.github.com/thiagomvas/fflow/commit/bd1f3c80aa3e916c6898a670bcdcf475f2bc652c))
* Add IWorkflowDefinition.CreateBuilder to create configured builders for that definition ([26d70f3](https://www.github.com/thiagomvas/fflow/commit/26d70f30b867a47af9bc9599b97f3ba86f84662b))
* Add new ForEachStep implementation ([60f5287](https://www.github.com/thiagomvas/fflow/commit/60f5287af090b0d42afbf38b1ec3c13fdd7d5c7c))
* Add step metadata registry to get and store metadata for certain steps ([49b00bf](https://www.github.com/thiagomvas/fflow/commit/49b00bf06b8eb9d99195b9418dc7c69d273a1964))
* Add switch case graph builder ([6d858b5](https://www.github.com/thiagomvas/fflow/commit/6d858b5c4b7f28e682ae7686702581614c79c305))
* Add workflow definition name to continuation node ([08904dd](https://www.github.com/thiagomvas/fflow/commit/08904dd40e38805f17f5cdb0bab4c179b2d37888))
* Create new base workflow builder ([96011fe](https://www.github.com/thiagomvas/fflow/commit/96011fee31d81bd0ac0c6f297e4a256b3bb57bce))
* **cli:** Add missing packages into the cli ([463ff7d](https://www.github.com/thiagomvas/fflow/commit/463ff7df14f13da3ac3f1495e0c8633441d754d9))
* **fileio:** Add Copy File step ([f522457](https://www.github.com/thiagomvas/fflow/commit/f5224573ef68e59e1338c75419a01280e8c9d33e))
* **fileio:** Add Create Directory Step ([a1904a8](https://www.github.com/thiagomvas/fflow/commit/a1904a8a4fabcee385bd02e6c6a3da4246f28350))
* **fileio:** Add Delete Directory Step ([5c153b6](https://www.github.com/thiagomvas/fflow/commit/5c153b613393df74f5402153573b6ef65ccc461d))
* **fileio:** Add Delete File Step ([8f53861](https://www.github.com/thiagomvas/fflow/commit/8f5386139a471941caf9070cf10aa5bd1d781827))
* **fileio:** Add File Checksum step with support for comparing checksums ([4206061](https://www.github.com/thiagomvas/fflow/commit/4206061abee8d3290294e711862bff58e28ffccc))
* **fileio:** Add File Read All Bytes ([38db7bf](https://www.github.com/thiagomvas/fflow/commit/38db7bf9ef1e9fb1b69f1a615dd05c860f536544))
* **fileio:** Add File Read All Text Step ([7964982](https://www.github.com/thiagomvas/fflow/commit/7964982fb8402f857cd933e083f99be6b187e79d))
* **fileio:** Add File Write Text Step with append support ([d1ca713](https://www.github.com/thiagomvas/fflow/commit/d1ca713308573527ea69273d07ca9fc52f39d9ff))
* **fileio:** Add File Write/Append Bytes Step ([bf3ea6e](https://www.github.com/thiagomvas/fflow/commit/bf3ea6e3e6a286586116e5db6f418080bee97bc1))
* **fileio:** Add Move File Step ([03c0b4c](https://www.github.com/thiagomvas/fflow/commit/03c0b4c14947b7752b5c552e505169aa66064747))
* **fileio:** Add Rename File Step ([dcb8030](https://www.github.com/thiagomvas/fflow/commit/dcb8030dda993bbf27428535a86d45ef1cb9026b))
* **fileio:** Add Touch File Step ([93bde14](https://www.github.com/thiagomvas/fflow/commit/93bde14ce4b125d238f2c3dee864eefd62f3a8aa))
* **http:** Add HttpMethod(string Url) extension methods ([b96a6ca](https://www.github.com/thiagomvas/fflow/commit/b96a6ca7cd8ee2d7fc21b0a2344dcaea55b389d3))
* **http:** Add query parameters support ([7bb458d](https://www.github.com/thiagomvas/fflow/commit/7bb458d3315c1d205df6f5bd053af20b15f4e58f))

### Bug Fixes

* Acceptable Status Codes filter correctly apply on http request step ([855e3de](https://www.github.com/thiagomvas/fflow/commit/855e3de988036ff8eb7e5e5d56ffb42249083470))
* Fix mergeIntoId not actually merging connected into the target node ([9555fbc](https://www.github.com/thiagomvas/fflow/commit/9555fbc3ac84175ffa30e90b23fc505151d283a0))
* fix package namespaces ([257eb5f](https://www.github.com/thiagomvas/fflow/commit/257eb5f6eaa753c911fa3dc00cf91174c3d4535c))
* Input setters now work with multiple Input calls ([89ac27b](https://www.github.com/thiagomvas/fflow/commit/89ac27b4cb285bb36bbd2e0356cf58bafb66e180))
* Use generic type definition for generic steps ([467b272](https://www.github.com/thiagomvas/fflow/commit/467b27210a1f5c90502e2cc62db213dad795df08))
* **sftp:** Added missing metadata to sftp steps (closes [#31](https://www.github.com/thiagomvas/fflow/issues/31)) ([356e919](https://www.github.com/thiagomvas/fflow/commit/356e9191f76e034a1b80c9dd07755c09ba78b921))

<a name="1.1.2"></a>
## [1.1.2](https://www.github.com/thiagomvas/fflow/releases/tag/v1.1.2) (2025-07-21)

<a name="1.1.1"></a>
## [1.1.1](https://www.github.com/thiagomvas/fflow/releases/tag/v1.1.1) (2025-07-21)

### Bug Fixes

* Fix source generators not generating code ([7e4ad3b](https://www.github.com/thiagomvas/fflow/commit/7e4ad3bc7c116bfdd7a65377c07b8bacb575f39f))
* Remove test projects from packaging ([4899887](https://www.github.com/thiagomvas/fflow/commit/4899887f6a2fc1803b6479a22a737a31dad701ef))

<a name="1.1.0"></a>
## [1.1.0](https://www.github.com/thiagomvas/fflow/releases/tag/v1.1.0) (2025-07-19)

### Features

* Add --root option to set the root directory for the container ([acc6f68](https://www.github.com/thiagomvas/fflow/commit/acc6f68e57a8f9a4f5de82882fec3d89d8772c74))
* Add a Create File via SFTP step to create an empty file ([ddd8cd6](https://www.github.com/thiagomvas/fflow/commit/ddd8cd655dc556ee9a8d0fa496cea93a877d74e8))
* Add Create Directory via FSTP step to create an empty directory ([48e28bb](https://www.github.com/thiagomvas/fflow/commit/48e28bb074ddcdca2b53584c945e42daf266c472))
* Add Delete Directory via SFTP to delete all the contents inside the directory ([a1ea068](https://www.github.com/thiagomvas/fflow/commit/a1ea06845baa568a0950c4414c46456dba0eff49))
* Add Delete File via SFTP ([820ebad](https://www.github.com/thiagomvas/fflow/commit/820ebad8ab934c864c9e73e9e91d8d81621dca7a))
* Add Disconnect from SFTP Step ([c7e769d](https://www.github.com/thiagomvas/fflow/commit/c7e769dc8c98255e1844b98f5c36a004f8a5b8e6))
* Add doctor command ([5986bd9](https://www.github.com/thiagomvas/fflow/commit/5986bd9af97fa93331f027f353a5d0d0af938c89))
* Add Download Directory via Sftp step ([8dda4a4](https://www.github.com/thiagomvas/fflow/commit/8dda4a42586fe5e27d6994bb6cb7a4b87552811d))
* Add Download File via FSTP step ([5e018cd](https://www.github.com/thiagomvas/fflow/commit/5e018cd7cc68a2a8093fe5d687530f812031c05d))
* Add dynamic status when setting up docker and executing pipeline ([6c340b9](https://www.github.com/thiagomvas/fflow/commit/6c340b9e039ba73834b6d4616176dbaef49796d3))
* Add extension method to add a step to log into console ([3c96564](https://www.github.com/thiagomvas/fflow/commit/3c96564b3f5472ce298aecc7177803d3d58ee775))
* Add help and short option name support ([8763673](https://www.github.com/thiagomvas/fflow/commit/87636734ff02251acd7126b2cb2eda73d64b96f8))
* Add help options for commands ([34bf7ca](https://www.github.com/thiagomvas/fflow/commit/34bf7caaa2097bf0fb4f5f013e947f798be7f5c3))
* Add help to run ([7cfe36e](https://www.github.com/thiagomvas/fflow/commit/7cfe36ed1fcbd4e445b53ffb0691cf8112dae45a))
* Add initialization command to create a file-based pipeline ([c34ca44](https://www.github.com/thiagomvas/fflow/commit/c34ca4445d69844c6611763c197ae01f7304a494))
* Add live logging support to fflow run command ([d0ee94f](https://www.github.com/thiagomvas/fflow/commit/d0ee94f0a1ff2aaa6fb6b7b7eea45158993836cb))
* Add note about docker image to doctor command ([0566afb](https://www.github.com/thiagomvas/fflow/commit/0566afbcf02d50690f915252d3df3d2a0b7b75da))
* Add Rename File via SFTP ([7635f43](https://www.github.com/thiagomvas/fflow/commit/7635f43c0a4337ed898cd23a81622811c5e214e7))
* Add run command to run the workflow isolated in a docker container ([2c8264c](https://www.github.com/thiagomvas/fflow/commit/2c8264c0a509295e2bf94ca002f1123309067da7))
* Add single file upload via sftp logic ([caea3d6](https://www.github.com/thiagomvas/fflow/commit/caea3d6dc5b66aa6f521ccf9217f675c69f698c2))
* Add source generators for dotnet step extensions and add IFlowContext.GetDotnet__Output extension methods ([b5ebbaf](https://www.github.com/thiagomvas/fflow/commit/b5ebbaff426ec5d286f3450b83b6500869d3bb54))
* Add support for stopping the workflow execution without throwing ([6a83d72](https://www.github.com/thiagomvas/fflow/commit/6a83d72338dca863d65e543bff3e795606c5be8a))
* Add Upload Directory Via Sftp step ([fc068e0](https://www.github.com/thiagomvas/fflow/commit/fc068e0970ab7bf9f10da4cd8c273ca9cb443339))
* Check for docker image for .NET 10 SDK ([e6bfb4c](https://www.github.com/thiagomvas/fflow/commit/e6bfb4cf6def90d70d978a7306f71f9cb07edb53))
* Separate docker version and image checks in doctor command ([551a950](https://www.github.com/thiagomvas/fflow/commit/551a950f34fd3ccfd4cde220e8315517d0d52ae8))
* **dotnet:** Add Dotnet Clean step ([2db2302](https://www.github.com/thiagomvas/fflow/commit/2db2302bb26c41a8af00c80f8235f8ed413ab7a0))

### Bug Fixes

* Dotnet Test step now correctly counts test results when multiple test projects are present ([5e3313e](https://www.github.com/thiagomvas/fflow/commit/5e3313efcf97fbc747c505e84eb71a87c54d389f))
* UploadDirectoryViaSftp now uploads recursively ([0191591](https://www.github.com/thiagomvas/fflow/commit/01915911d3859b87ae9772fdda52d2492e9eb38d))

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


# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

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


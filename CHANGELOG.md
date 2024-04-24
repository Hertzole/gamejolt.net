## [1.1.1](https://github.com/Hertzole/gamejolt.net/compare/v1.1.0...v1.1.1) (2024-04-24)


### Bug Fixes

* nullability issues ([#3](https://github.com/Hertzole/gamejolt.net/issues/3)) ([54c7ba4](https://github.com/Hertzole/gamejolt.net/commit/54c7ba492798d4f87d6fe4917c57190b2c8e9725))
* session events being static ([f2a7284](https://github.com/Hertzole/gamejolt.net/commit/f2a728444bdf634fa2f072b53ae2e991c1186cc0))
* system json boolean converters not throwing exception with invalid number ([3e2f54b](https://github.com/Hertzole/gamejolt.net/commit/3e2f54b93969063ba44f185e85e620e61f0d8627))

# [1.1.0](https://github.com/Hertzole/gamejolt.net/compare/v1.0.3...v1.1.0) (2024-04-16)


### Bug Fixes

* don't use ConfigureAwait as it doesn't work in Unity ([3ef3ba4](https://github.com/Hertzole/gamejolt.net/commit/3ef3ba4e4ccc79da4d24b74f9546288eddbf1aa0))
* GameJoltManager destroy cancellation token pre Unity 2022.2 ([c23c4d7](https://github.com/Hertzole/gamejolt.net/commit/c23c4d76b2b3677acdc9f43ce5cc9aaf4cb117ff))
* **GameJoltManager:** trying to close sessions and shutdown when not initialized ([8c04239](https://github.com/Hertzole/gamejolt.net/commit/8c0423999e27862c407cdc0fd4669c342e9b208d))
* missing Auto Close Sessions in GameJoltSettings editor ([1d8a66c](https://github.com/Hertzole/gamejolt.net/commit/1d8a66cac683f69f9ea31b7910116e570c4d496a))
* not being able to build .NET project ([1641124](https://github.com/Hertzole/gamejolt.net/commit/16411243dd0fe9e0dd874cc89abc56e2ed5f49dc))
* ToCommaSeparatedString throwing exception if array is null ([9c02d11](https://github.com/Hertzole/gamejolt.net/commit/9c02d11a48fa8869e0696b4d89b773906e37f253))
* **Unity:** File.ReadAllTextAsync not being available pre .NET Standard 2.1 ([c78207b](https://github.com/Hertzole/gamejolt.net/commit/c78207b49104e10b2b910519ec58f097955d1172))
* **Unity:** FindAnyObjectByType is not always available ([507aea6](https://github.com/Hertzole/gamejolt.net/commit/507aea6b8d0600e3e075ccc3f77a995ad7d9584c))
* **Unity:** integration, not intergration ([b28766c](https://github.com/Hertzole/gamejolt.net/commit/b28766c02e8243c4b5e0180339f1c80e7d4a6108))
* **Unity:** missing IMGUI dependency ([f49e992](https://github.com/Hertzole/gamejolt.net/commit/f49e9923c74d452fa7a648406609d6c1c44508d6))
* **Unity:** no editor assembly ([98651a5](https://github.com/Hertzole/gamejolt.net/commit/98651a54cfb494937475789dbbf5f1d02a77216a))
* **Unity:** turn off automatic initialization ([fc41c09](https://github.com/Hertzole/gamejolt.net/commit/fc41c09e48bce5a68dc5bf245090cfd84d571e8d))


### Features

* GameJoltManager for Unity ([8b6b95b](https://github.com/Hertzole/gamejolt.net/commit/8b6b95bf0f9b6c9d24c9465fcc768a317581019b))
* OnInitialized, OnShutdown, and OnShutdownComplete events ([095a1f4](https://github.com/Hertzole/gamejolt.net/commit/095a1f46e961cb8dbcd6fde2b8c2fd3b666f8a6e))
* override ToString in all types ([#2](https://github.com/Hertzole/gamejolt.net/issues/2)) ([32b4dee](https://github.com/Hertzole/gamejolt.net/commit/32b4dee8eb8115e1b1d9cba407fd13b7f5cdb088))
* session open/close/ping events ([b78423a](https://github.com/Hertzole/gamejolt.net/commit/b78423a977d27959296ccae473da1e9b0744ed3f))
* **Unity:** auto ping interval ([619317d](https://github.com/Hertzole/gamejolt.net/commit/619317da2d0544bdb6e40fe56bd697368c6e877d))
* **Unity:** clarify auto sign in options in editor ([1defee1](https://github.com/Hertzole/gamejolt.net/commit/1defee144fc3eae523376e63914bdd6e9bbe930b))


### Performance Improvements

* use Unity's Awaitable in 2023.1+ when calling the web ([2ca142f](https://github.com/Hertzole/gamejolt.net/commit/2ca142fb0a999f6d8b8ba72fec377196576d55ec))

## [1.0.3](https://github.com/Hertzole/gamejolt.net/compare/v1.0.2...v1.0.3) (2024-01-30)


### Bug Fixes

* no readme or license in NuGet package ([8abed6d](https://github.com/Hertzole/gamejolt.net/commit/8abed6d0dafdbd57fe70a98f859dbeb5aeb4aee1))

## [1.0.2](https://github.com/Hertzole/gamejolt.net/compare/v1.0.1...v1.0.2) (2024-01-11)


### Bug Fixes

* making nuget package ([1519ff7](https://github.com/Hertzole/gamejolt.net/commit/1519ff749d1d079116256301554b597b6b7efd2a))

## [1.0.1](https://github.com/Hertzole/gamejolt.net/compare/v1.0.0...v1.0.1) (2024-01-11)


### Bug Fixes

* producing invalid signature in .NET 5+ ([e748955](https://github.com/Hertzole/gamejolt.net/commit/e7489557ad00ad48624f85251f8ed6bbfc84b8b2))

# 1.0.0 (2024-01-11)


### Bug Fixes

* bunch of nullability warnings ([836299f](https://github.com/Hertzole/gamejolt.net/commit/836299f5b180513d0384f7ccbf832ff08d4b0ef5))
* data store using wrong return types ([16b642b](https://github.com/Hertzole/gamejolt.net/commit/16b642b1ecc63e68726f69659c1187070f3c255c))
* more nullability warnings ([0e02699](https://github.com/Hertzole/gamejolt.net/commit/0e0269911b4b4bc6b58069590269f2f04476d011))
* newtonsoft converters ([7fdb369](https://github.com/Hertzole/gamejolt.net/commit/7fdb36925955bd6100550093b01f0a52c03f6c15))
* newtonsoft json being included in .NET 7+ ([d6ec8ad](https://github.com/Hertzole/gamejolt.net/commit/d6ec8ad6855f9501f0a5ecc5bc2b716c3927372c))
* no token provided in Sessions.CheckAsync ([9893b7f](https://github.com/Hertzole/gamejolt.net/commit/9893b7fb5c07fbcf8b70fe7602debcc26e42e82b))
* nullability warnings ([ca27596](https://github.com/Hertzole/gamejolt.net/commit/ca27596200f94ccd493f2199a26fa3841ebfb334))
* number parsers not being invariant ([d98d5ec](https://github.com/Hertzole/gamejolt.net/commit/d98d5ecf09853ed754afbd9ab41e231b69249f1c))
* tests for pre .NET 6 ([34eebaa](https://github.com/Hertzole/gamejolt.net/commit/34eebaa73bd180a1eeae2d084a81866e68c5326b))
* unity related errors ([6a723b9](https://github.com/Hertzole/gamejolt.net/commit/6a723b9a5b3ccd40a15c7539f4c4d4461f7d3396))
* wrong naming for DataStore methods ([96e2af0](https://github.com/Hertzole/gamejolt.net/commit/96e2af0c3914b46b51e4607f2461e5301a64435e))


### Features

* pre .NET 6 converters ([b59cdad](https://github.com/Hertzole/gamejolt.net/commit/b59cdad7678f3fefa32af1c7f607c595ca3fe812))
* Set/Get bool to DataStore ([48c7055](https://github.com/Hertzole/gamejolt.net/commit/48c70556db5c831d624b26c0c9e10ddb95844049))
* unity support ([3246f54](https://github.com/Hertzole/gamejolt.net/commit/3246f5438a9d9e6f7201e58c6fbda6ffc803a524))


### Performance Improvements

* use ConfigureAwait(false) when possible on internal calls ([ed7331d](https://github.com/Hertzole/gamejolt.net/commit/ed7331d2aef28ea5c82b2924c49d037a1856938e))
* use ordinal ignore case string comparison ([ba71ff7](https://github.com/Hertzole/gamejolt.net/commit/ba71ff799dd2fa2938372f73b4d8a467d167daf5))
* use spans when building signature, if possible ([6fb3083](https://github.com/Hertzole/gamejolt.net/commit/6fb30834ec23c02f2f3c6bb73feac752b422b68f))
* use valuetask when possible on internal calls ([3173f3e](https://github.com/Hertzole/gamejolt.net/commit/3173f3edab34c0e54d83cfb591a3b6ec88ed632c))

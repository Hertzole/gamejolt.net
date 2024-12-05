# [1.6.0](https://github.com/Hertzole/gamejolt.net/compare/v1.5.1...v1.6.0) (2024-12-05)


### Features

* new background GameJoltManager for Unity ([fa77d4f](https://github.com/Hertzole/gamejolt.net/commit/fa77d4f5bdea859f9cc5eff11f8e5c93760dfb77))

## [1.5.1](https://github.com/Hertzole/gamejolt.net/compare/v1.5.0...v1.5.1) (2024-12-04)


### Bug Fixes

* nuget publish ([d5b222c](https://github.com/Hertzole/gamejolt.net/commit/d5b222c7e909bef5478fb9655f9946eb35561446))

# [1.5.0](https://github.com/Hertzole/gamejolt.net/compare/v1.4.2...v1.5.0) (2024-12-04)


### Bug Fixes

* GameJoltSettings not being included when batch building in Unity ([b06d150](https://github.com/Hertzole/gamejolt.net/commit/b06d15072b27edb4c4eafce53c3c3e2656a0334d))


### Features

* allow for editing settings with GameJolt disabled ([cc6772f](https://github.com/Hertzole/gamejolt.net/commit/cc6772f8d925111b5bf2ca384822fdefb680ceb5))


### Reverts

* GameJoltSettings meta guid ([bb0207f](https://github.com/Hertzole/gamejolt.net/commit/bb0207f81a5a60e297884b4e6fd900fa8f994212))

## [1.4.2](https://github.com/Hertzole/gamejolt.net/compare/v1.4.1...v1.4.2) (2024-10-14)


### Bug Fixes

* missing ToString on GameJoltResult ([68d2afb](https://github.com/Hertzole/gamejolt.net/commit/68d2afbf36c47730075d0b609a85dcde34934aa2))
* missing xml docs on certain types ([91b7ede](https://github.com/Hertzole/gamejolt.net/commit/91b7ede65730026ab8da69c0b1a57e7a72595b82))
* xml docs not being included in nuget package ([82ccb71](https://github.com/Hertzole/gamejolt.net/commit/82ccb71854d4aff6eb1531b6c6da2fdfb56521cf))

## [1.4.1](https://github.com/Hertzole/gamejolt.net/compare/v1.4.0...v1.4.1) (2024-07-08)


### Reverts

* Revert "chore(release): 1.4.0 [skip ci]" ([4441b70](https://github.com/Hertzole/gamejolt.net/commit/4441b706cacf5bab06195c92230c1f066e8ee2c8))

# [1.4.0](https://github.com/Hertzole/gamejolt.net/compare/v1.3.4...v1.4.0) (2024-07-08)


### Bug Fixes

* GameJoltResult not implementing IEquatable ([f70e8f3](https://github.com/Hertzole/gamejolt.net/commit/f70e8f36e2ef40448b5e92ff9725145fc18a9c2f))
* no error if AuthenticateFromCredentialsFileAsync is called with null array ([353c243](https://github.com/Hertzole/gamejolt.net/commit/353c243003bfac1420d4964d230f63e1e5b8f764))
* no error if AuthenticateFromCredentialsFileAsync is called with null/empty strings ([b78a821](https://github.com/Hertzole/gamejolt.net/commit/b78a8218ac483fbf593e973bc6b2ff304c1fc01c))
* no validation for urls in AuthenticateFromUrlAsync ([ce63c85](https://github.com/Hertzole/gamejolt.net/commit/ce63c85e429aa1e10f3f36102199401a4cba6395))
* PoolHandle not implementing IEquatable ([97e2e08](https://github.com/Hertzole/gamejolt.net/commit/97e2e0801a9d1f92c56945de10e4707936f97e94))
* use Append(char) instead of Append(string) where applicable ([2a40aec](https://github.com/Hertzole/gamejolt.net/commit/2a40aec2bd31f679129553e012c94c59db6b59fa))

### Features

* `DISABLE_GAMEJOLT` define ([daeb7f4](https://github.com/Hertzole/gamejolt.net/commit/daeb7f4e5db09f3cb5ba47064da95fc55343b9a3))
* `FORCE_SYSTEM_JSON` define ([f3db9ba](https://github.com/Hertzole/gamejolt.net/commit/f3db9bafdbc6c27b96832ce38995e798301a4477))
* trimming and AOT support ([3227e8e](https://github.com/Hertzole/gamejolt.net/commit/3227e8eebc0e74b44db479b1e1615e9bdda67065))

## [1.3.4](https://github.com/Hertzole/gamejolt.net/compare/v1.3.3...v1.3.4) (2024-06-02)


### Bug Fixes

* scoreboard query not handling guest users ([2b42af2](https://github.com/Hertzole/gamejolt.net/commit/2b42af21587fb6b65206ee94b7b6ff70f3a575a0))
* **Unity:** integration not being active below 2021.1 ([2d46bbf](https://github.com/Hertzole/gamejolt.net/commit/2d46bbf098d228700db26f3bdddf0dd71e76ae9e))
* **Unity:** session not staying open if game is in the background ([81f6255](https://github.com/Hertzole/gamejolt.net/commit/81f62551a00526e21c599b85efd2b96601aa0889))

## [1.3.3](https://github.com/Hertzole/gamejolt.net/compare/v1.3.2...v1.3.3) (2024-05-24)


### Bug Fixes

* string operaitons being swapped ([d6f0988](https://github.com/Hertzole/gamejolt.net/commit/d6f0988572db3700557b7a5a09ec86124b7d02d9))

## [1.3.2](https://github.com/Hertzole/gamejolt.net/compare/v1.3.1...v1.3.2) (2024-05-20)


### Bug Fixes

* nullability warnings ([8403d48](https://github.com/Hertzole/gamejolt.net/commit/8403d4811f2e3fb34e714141fc68cac1708aeb0b))

## [1.3.1](https://github.com/Hertzole/gamejolt.net/compare/v1.3.0...v1.3.1) (2024-05-18)


### Bug Fixes

* **Unity:** game jolt instance not surviving scene loads ([9729422](https://github.com/Hertzole/gamejolt.net/commit/97294226c32e5b881f492ae9424b91f384140d48))
* **Unity:** removed left over debug logs ([ddbe91f](https://github.com/Hertzole/gamejolt.net/commit/ddbe91fb00e546c0b292236b19970ce62b1f4881))

# [1.3.0](https://github.com/Hertzole/gamejolt.net/compare/v1.2.0...v1.3.0) (2024-05-14)


### Bug Fixes

* exceptions not being sealed ([cd05490](https://github.com/Hertzole/gamejolt.net/commit/cd05490846d31c04ac7981a61c6e58273fd3b545))
* made internal object pool thread safe ([1d5f7ac](https://github.com/Hertzole/gamejolt.net/commit/1d5f7acb68cb078c8daceafdcf3ecbf06b8549a8))
* wrong exception type when data key is invalid ([fc464f4](https://github.com/Hertzole/gamejolt.net/commit/fc464f40aa4511d5180b7b98a96fc5baab19a1f1))


### Features

* errorIfUnlocked/notUnlocked to trophies ([7a298f7](https://github.com/Hertzole/gamejolt.net/commit/7a298f78788de37f93b0f16c8ef9c5b264c31174))


### Performance Improvements

* use empty array instead of new array if results are empty ([da611df](https://github.com/Hertzole/gamejolt.net/commit/da611df6e52c01dd97e855a7c4c11c751bef0360))

# [1.2.0](https://github.com/Hertzole/gamejolt.net/compare/v1.1.0...v1.2.0) (2024-04-26)


### Bug Fixes

* data store converters skip unknown properties ([273ecd8](https://github.com/Hertzole/gamejolt.net/commit/273ecd8340d86e99d02fc791f9cdb6e3247ef82a))
* data store responses can no longer have null data ([9aee9ae](https://github.com/Hertzole/gamejolt.net/commit/9aee9ae2283e12b8852cd0a904a2dbbddabf058e))
* friend converters skip unknown properties ([5be3fc2](https://github.com/Hertzole/gamejolt.net/commit/5be3fc2cc65956b70d6a35676f32b6e461f2a86d))
* int converter not being able to handle doubles ([915e147](https://github.com/Hertzole/gamejolt.net/commit/915e1474d131d2043416d648c4708247ffdda401))
* long converter not being able to handle doubles ([bb3f2f9](https://github.com/Hertzole/gamejolt.net/commit/bb3f2f949167df3176bee9a4a6fcaf711bb9fb9e))
* nullability issues ([#3](https://github.com/Hertzole/gamejolt.net/issues/3)) ([54c7ba4](https://github.com/Hertzole/gamejolt.net/commit/54c7ba492798d4f87d6fe4917c57190b2c8e9725))
* response converter handling nulls better ([812f516](https://github.com/Hertzole/gamejolt.net/commit/812f516d11be9d1847a42dbc858a70ac4c0cb75f))
* response converter not reading properly if unknown data is present ([af70b51](https://github.com/Hertzole/gamejolt.net/commit/af70b518e44e5806dc58abcb6dbbfd081ac23b2e))
* score converters skip unknown properties ([ec88014](https://github.com/Hertzole/gamejolt.net/commit/ec8801452484bb5a1bb9327fa27c5984c76dbaae))
* session events being static ([f2a7284](https://github.com/Hertzole/gamejolt.net/commit/f2a728444bdf634fa2f072b53ae2e991c1186cc0))
* StringOrNumberConverter being able to return null response ([7234c71](https://github.com/Hertzole/gamejolt.net/commit/7234c71dbbdbf212e7013663c8561e13a378d6d9))
* StringOrNumberConverter handling numbers better ([6c16d51](https://github.com/Hertzole/gamejolt.net/commit/6c16d5101b4ee8a9f2ec013789e0c1e29ac00e81))
* StringOrNumberConverter nullability warning ([506863c](https://github.com/Hertzole/gamejolt.net/commit/506863c59f359e479a26df6d7c99c4b5fc411741))
* system json boolean converters not throwing exception with invalid number ([3e2f54b](https://github.com/Hertzole/gamejolt.net/commit/3e2f54b93969063ba44f185e85e620e61f0d8627))
* time converters skip unknown properties ([108af87](https://github.com/Hertzole/gamejolt.net/commit/108af87dede40d7b425257e3f9053af257dbba77))
* trophies converters skip unknown properties ([2aeeaad](https://github.com/Hertzole/gamejolt.net/commit/2aeeaad5f10669e25d8136b3717774964b30ab29))
* user converters skip unknown properties ([d398007](https://github.com/Hertzole/gamejolt.net/commit/d39800720c00b9a553ce96edb591a6829971bae2))
* credentials split is the same on UNIX systems ([a7c9568](https://github.com/Hertzole/gamejolt.net/commit/a7c95685ea37d7e4603ba6437f48c20ae5c3c495))

## [1.1.2](https://github.com/Hertzole/gamejolt.net/compare/v1.1.0...v1.1.2) (2024-04-26)


### Bug Fixes

* data store converters skip unknown properties ([273ecd8](https://github.com/Hertzole/gamejolt.net/commit/273ecd8340d86e99d02fc791f9cdb6e3247ef82a))
* data store responses can no longer have null data ([9aee9ae](https://github.com/Hertzole/gamejolt.net/commit/9aee9ae2283e12b8852cd0a904a2dbbddabf058e))
* friend converters skip unknown properties ([5be3fc2](https://github.com/Hertzole/gamejolt.net/commit/5be3fc2cc65956b70d6a35676f32b6e461f2a86d))
* int converter not being able to handle doubles ([915e147](https://github.com/Hertzole/gamejolt.net/commit/915e1474d131d2043416d648c4708247ffdda401))
* long converter not being able to handle doubles ([bb3f2f9](https://github.com/Hertzole/gamejolt.net/commit/bb3f2f949167df3176bee9a4a6fcaf711bb9fb9e))
* nullability issues ([#3](https://github.com/Hertzole/gamejolt.net/issues/3)) ([54c7ba4](https://github.com/Hertzole/gamejolt.net/commit/54c7ba492798d4f87d6fe4917c57190b2c8e9725))
* response converter handling nulls better ([812f516](https://github.com/Hertzole/gamejolt.net/commit/812f516d11be9d1847a42dbc858a70ac4c0cb75f))
* response converter not reading properly if unknown data is present ([af70b51](https://github.com/Hertzole/gamejolt.net/commit/af70b518e44e5806dc58abcb6dbbfd081ac23b2e))
* score converters skip unknown properties ([ec88014](https://github.com/Hertzole/gamejolt.net/commit/ec8801452484bb5a1bb9327fa27c5984c76dbaae))
* session events being static ([f2a7284](https://github.com/Hertzole/gamejolt.net/commit/f2a728444bdf634fa2f072b53ae2e991c1186cc0))
* StringOrNumberConverter being able to return null response ([7234c71](https://github.com/Hertzole/gamejolt.net/commit/7234c71dbbdbf212e7013663c8561e13a378d6d9))
* StringOrNumberConverter handling numbers better ([6c16d51](https://github.com/Hertzole/gamejolt.net/commit/6c16d5101b4ee8a9f2ec013789e0c1e29ac00e81))
* StringOrNumberConverter nullability warning ([506863c](https://github.com/Hertzole/gamejolt.net/commit/506863c59f359e479a26df6d7c99c4b5fc411741))
* system json boolean converters not throwing exception with invalid number ([3e2f54b](https://github.com/Hertzole/gamejolt.net/commit/3e2f54b93969063ba44f185e85e620e61f0d8627))
* time converters skip unknown properties ([108af87](https://github.com/Hertzole/gamejolt.net/commit/108af87dede40d7b425257e3f9053af257dbba77))
* trophies converters skip unknown properties ([2aeeaad](https://github.com/Hertzole/gamejolt.net/commit/2aeeaad5f10669e25d8136b3717774964b30ab29))
* user converters skip unknown properties ([d398007](https://github.com/Hertzole/gamejolt.net/commit/d39800720c00b9a553ce96edb591a6829971bae2))

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

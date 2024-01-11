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

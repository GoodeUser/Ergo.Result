<p align="center"><img src="/logo.png?raw=true" width="150"></p>
<h1 align="center">Result type with the ability to chain computations</h1>
 
<div align="center">

![build](https://github.com/GoodeUser/Ergo/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/GoodeUser/Ergo/branch/master/graph/badge.svg?token=8XDVMVSNIC)](https://codecov.io/gh/GoodeUser/Ergo)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/GoodeUser/Ergo/blob/master/LICENSE)
[![nuget](https://img.shields.io/nuget/v/Ergo)](https://www.nuget.org/packages/Ergo/)

</div>

## ✨ Features
* A Result type that lets you wrap a return type `Result<T>`
* Chainable validations/computations (handles the [Defensive Programming](https://en.wikipedia.org/wiki/Defensive_programming))
* First class `async` support - lets you chain validations on async methods
* Inspired by functional languages but designed for Object Oriented Programming.
* Lets you fall back to a base `Result` class if you don't care about the response held inside (much like `Task<T>` does)
* 100% test coverage

## ⚙️ Use

Use `Result` whenever an operation has the possibility of failure. Consider the following example of a method that loads a user from a database when accessing an API.

```cs
using static Ergo.Result;

public Result<User> GetUserFromSecretKey(string secretKey)
{
    var user = _userRepository.GetBySecretKey(secretKey);

    if (user == null)
        return Failure<User>("Secret Key Invalid.");

    if (!user.IsEnabled)
        return Failure<User>("User isn't allowed access");

    return Success(user);
}
```
And the consuming code might look something like this:
```cs
public async Task<Result<User>> GetAuthorizedUser()
{
    // Did I mention that you can use async computations?
    return await GetAuthorizationHeader(Context)
        .OnSuccess(GetSecretKeyFromAuthorizationHeader) // Only runs if the header is found
        .OnSuccess(GetUserFromSecretKey) // only runs if retrieving the "secret key" was successful
        .OnFailure((failures) => FormatMessages(failures, CultureInfo.InvariantCulture));
}
```

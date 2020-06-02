<p align="center"><img src="/logo.png?raw=true" width="150"></p>
<h1 align="center">Result type with the ability to chain computations</h1>
 
<div align="center">

![build](https://github.com/GoodeUser/Ergo/workflows/build/badge.svg) [![codecov](https://codecov.io/gh/GoodeUser/Ergo/branch/master/graph/badge.svg?token=8XDVMVSNIC)](https://codecov.io/gh/GoodeUser/Ergo) ![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg) ![nuget](https://img.shields.io/nuget/v/Ergo)

</div>

## Use

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
    return await GetAuthorizationHeader(Context)
        .OnSuccess(GetSecretKeyFromAuthorizationHeader) // Only runs if the header is found
        .OnSuccess(GetUserFromSecretKey) // only runs if retrieving the "secret key" was successful
        .OnFailure((failures) => FormatMessages(failures, CultureInfo.InvariantCulture));
}
```

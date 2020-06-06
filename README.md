<p align="center"><img src="/logo.png?raw=true" width="150"></p>
<h1 align="center">Result type with the ability to chain computations and first class Task/async support.</h1>
 
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
* Falls back to a base `Result` class if you don't care about the contained response (much like `Task<T>` does)
* Well tested

## ⚙️ Use
Return a `Result` class from a method that can fail:
```cs
using static Ergo.Result;

public Result<string> GetName(Dictionary<string, object> input)
{
    return input.HasKey("full_name")
        ? Success((string)input["full_name"])
        : Failure<string>("There was no 'full_name' key available in the dictionary");
}

public Result<string> GetFirstName(string fullName)
{
    var nameParts = fullName.Split(' ');

    return nameParts.Length > 1
        ? Success(nameParts.First())
        : Failure<string>("Ensure both a first and last name were submitted");
}
```
Once you have a `Result`, you can chain other methods that also return a `Result` class.
```cs
public Result<string> GetNameFromJson(string json)
{
    return SerializeDictionaryFromJson(json)
        .OnSuccess((jsonDictionary) => GetName(jsonDictionary))
        .OnSuccess((fullName) => GetFirstName(fullName));
        .OnFailure((failures) => FormatFailureMessages(failures))
}
```
### Async support
`Ergo` was designed to work well even in Async situations. For instance, continuing from our previous example, any method can return a `Task<Result<T>>` can be chained and used just like any other method. The only thing that changes is that you would then call `await` on the resulting object.

```cs
using static Ergo.Result;

public async Task<Result<User>> GetUserFromSecretKey(string secretKey)
{
    var user = await _userRepository.GetBySecretKey(secretKey);
    
    if (user is null)
        return Failure<User>("User isn't allowed access");

    return Success(user);
}
```
Any method that returns a `Result` from an async method can be chained like any other method that returns a `Result`. The consuming code would look something like this:
```cs
public async Task<Result<User>> GetAuthorizedUser()
{
    var asyncResult = GetAuthorizationHeader(Context)
        .OnSuccess(GetSecretKeyFromAuthorizationHeader)
        .OnSuccess(GetUserFromSecretKey) // this is the only method that is async - it returns a Task<Result<User>>
        .OnSuccess(ValidateUserIsActive);
        
    var result = await asyncResult; // we can await the response just like a Task!!!
    
    if (result.IsSuccessful)
        Console.WriteLine("It was successful");
    
    return result;
}
```
The ability to have the same methods available when a method returns a Task is achieved by converting the `Task<Result>` to a class `AsyncResult` behind the scenes. `AsyncResult` is designed to be fully compatible with a normal `Result` class and has the exact same methods and usage. Once you have an `AsyncResult` class, you just await it to pull out the `Result` object.

If you start out with a `Task<Result>` or `Task<Result<T>>` and want to convert it to an `AsyncResult`, there are constructors designed to help with the conversion.
```cs
var taskResult = Task.FromResult(Result.Success(""));
var asyncResult = new AsyncResult(taskResult);
// Now you can use all of the methods that `Result` has like `OnSuccess`, etc.
```

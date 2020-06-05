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
* 100% test coverage

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
        ? Success("")
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

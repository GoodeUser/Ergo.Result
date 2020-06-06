using System;
using System.Collections.Generic;
using System.Linq;
using Ergo;
using static Ergo.Result;

namespace ArgumentParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryStringResult = ParseArguments(args)
                .OnSuccess(GetUrl)
                .OnSuccess(GetQueryFromUrl);

            if (queryStringResult.IsFailure)
            {
                Console.WriteLine(string.Join("\n", queryStringResult.Messages));
            }

            Console.WriteLine($"The query string from the URL passed in is: {queryStringResult.Value}");
        }

        static Result<Dictionary<string, string>> ParseArguments(string[] arguments)
        {
            if (!arguments.Any())
            {
                return Failure<Dictionary<string, string>>("No arguments were provided.");
            }

            var argumentDictionary = arguments
                .Select(ParseArgument)
                .Where(result => result.IsSuccessful)
                .ToDictionary(r => r.Value.Item1, r => r.Value.Item2);

            return Success(argumentDictionary);
        }

        static Result<(string, string)> ParseArgument(string arg)
        {
            if (arg.StartsWith("--"))
            {
                var argSplit = arg.Split('=');
                var argName = argSplit.First().Substring(2);
                var keyValue = (argName, argSplit[1]);
                
                Success(keyValue);
            }

            return Failure<(string, string)>("Argument not in correct format. Argument should be in the format: --url=http://www.google.com?q=shoes");
        }

        static Result<string> GetUrl(Dictionary<string, string> arguments)
        {
            return arguments.ContainsKey("url")
                ? Success(arguments["url"])
                : Failure<string>("No parameter 'url' was provided");
        }

        static Result<string> GetQueryFromUrl(string url)
        {
            try
            {
                var uriObject = new Uri(url);

                return uriObject.Query.Any()
                    ? Success(uriObject.Query)
                    : Failure<string>("The uri passed in has no query string.");
            }
            catch (Exception ex)
            {
                return Failure<string>($"Could not parse the URL that was passed in: {ex}");
            }
        }
    }
}

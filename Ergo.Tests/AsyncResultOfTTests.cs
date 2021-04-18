// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;

// namespace Ergo.Tests
// {
//     public class AsyncResultOfTTests
//     {
//         [Fact]
//         public async Task OnSuccess_Result_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnSuccess((_) => Result.Success());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnSuccess_Result_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Result.Success());

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public async Task OnSuccess_ResultOfT_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnSuccess((_) => Result.Success(2));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(2, result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_ResultOfT_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Result.Success(3));

//             Assert.True(result.IsFailure);
//             Assert.Equal(default(int), result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_ResultOfT_CanPassArgument_Failure_DifferentType()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Result.Success("hello"));

//             Assert.True(result.IsFailure);
//             Assert.Equal(default(string), result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_TaskResultOfT_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnSuccess((_) => Task.FromResult(Result.Success(2)));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(2, result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_TaskResultOfT_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Task.FromResult(Result.Success(3)));

//             Assert.True(result.IsFailure);
//             Assert.Equal(default(int), result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_TaskResultOfT_CanPassArgument_Failure_DifferentType()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Task.FromResult(Result.Success("hello")));

//             Assert.True(result.IsFailure);
//             Assert.Equal(default(string), result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_Task_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnSuccess((_) => Task.FromResult(Result.Success()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnSuccess_Task_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnSuccess((_) => Task.FromResult(Result.Success()));

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public async Task OnFailure_ResultOfT_ChainedFailure_()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>("a"))
//                 .OnFailure((messages) => Result.Failure(messages.ToArray()));

//             Assert.Equal("a", result.Messages.First());
//         }

//         [Fact]
//         public async Task OnFailure_Result_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnFailure((_) => Result.Success());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnFailure_Result_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Result.Failure());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnFailure_ResultOfT_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Result.Success(2));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(1, result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_ResultOfT_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnFailure((_) => Result.Success(3));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(3, result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_ResultOfT_CanPassArgument_Failure_DifferentType()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Result.Success("hello"));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(default(string), result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_TaskResultOfT_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Task.FromResult(Result.Success(2)));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(1, result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_TaskResultOfT_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnFailure((_) => Task.FromResult(Result.Success(3)));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(3, result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_TaskResultOfT_CanPassArgument_Failure_DifferentType()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Task.FromResult(Result.Success("hello")));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal(default(string), result.Value);
//         }

//         [Fact]
//         public async Task OnFailure_Task_CanPassArgument_Failure()
//         {
//             var result = await ((AsyncResult<int>)Result.Failure<int>())
//                 .OnFailure((_) => Task.FromResult(Result.Success()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnFailure_Task_CanPassArgument_Success()
//         {
//             var result = await ((AsyncResult<int>)Result.Success(1))
//                 .OnFailure((_) => Task.FromResult(Result.Success()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void ConstructorDisallowsNulls()
//         {
//             Assert.Throws<ArgumentNullException>(() => {
//                 new AsyncResult<string>(null);
//             });
//         }

//         [Fact]
//         public async Task CanAddMessages()
//         {
//             AsyncResult<string> implicitValue = "test";

//             var resultWithMessage = await implicitValue
//                 .WithMessages("jon snow")
//                 .WithMessages("is a bastard");
                
//             Assert.Equal("jon snow", resultWithMessage.Messages.First());
//             Assert.Equal("is a bastard", resultWithMessage.Messages.ElementAt(1));
//         }

//         public Result<string> Tr()
//         {
//             return true ?  Result.Success("true") : Result.Failure<string>();
//         }
//     }
// }
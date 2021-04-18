// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;

// namespace Ergo.Tests
// {
//     public class ResultTests
//     {
//         [Fact]
//         public void CanCreateSuccess()
//         {
//             var result = Result.Success();
//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void CanCreateFailure()
//         {
//             var result = Result.Failure();
//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void CanCreateFailureWithMessage()
//         {
//             var result = Result.Failure("message");
//             Assert.True(result.Messages.First() == "message");
//         }

//         [Fact]
//         public void CanCreateFailureWithMessages()
//         {
//             var result = Result.Failure("message", "message1", "message2");
//             var intersectionCount = result.Messages.Intersect(new [] {"message", "message1", "message2"}).Count();
            
//             Assert.Equal(3, intersectionCount);
//         }

//         [Fact]
//         public void CanCreateFailureWithMessageList()
//         {
//             var result = Result.Failure(new [] { "message", "message1", "message2" });
//             var intersectionCount = result.Messages.Intersect(new [] {"message", "message1", "message2"}).Count();
            
//             Assert.Equal(3, intersectionCount);
//         }

//         [Fact]
//         public void Can2SuccessesBeCombinedForSuccess()
//         {
//             var result = Result.Success() + Result.Success();
//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void Can1FailureMakeSuccessFailure()
//         {
//             var result = Result.Success() + Result.Failure();
//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void Can2FailuresBeCombinedForFailure()
//         {
//             var result = Result.Failure() + Result.Failure();
//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void Can2FailuresBeCombinedForFailureWithMessages()
//         {
//             var result = Result.Failure("a", "b") + Result.Failure("c", "d");

//             var intersectionCount = result.Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
//             Assert.Equal(4, intersectionCount);
//         }

//         [Fact]
//         public async Task CanBeJoinedWithTaskResult()
//         {
//             var asyncResult = Result.Failure("a", "b") + Task.FromResult(Result.Failure("c", "d"));

//             var intersectionCount = (await asyncResult).Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
//             Assert.Equal(4, intersectionCount);
//         }

//         [Fact]
//         public async Task CanBeJoinedWithAsyncResult()
//         {
//             var asyncResult = Result.Failure("a", "b") + ((AsyncResult)Task.FromResult(Result.Failure("c", "d")));

//             var intersectionCount = (await asyncResult).Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
//             Assert.Equal(4, intersectionCount);
//         }

//         [Fact]
//         public void OnSuccess_Result_ChainedSuccess()
//         {
//             var result = Result.Success()
//                 .OnSuccess(() => Result.Success());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void OnSuccess_Result_FailureStopsPipeline()
//         {
//             var result = Result.Success()
//                 .OnSuccess(() => Result.Failure())
//                 .OnSuccess(() => Result.Success());

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void OnSuccess_ResultOfT_ChainedSuccess()
//         {
//             var result = Result.Success()
//                 .OnSuccess(() => Result.Success("string"));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal("string", result.Value);
//         }

//         [Fact]
//         public void OnSuccess_ResultOfT_FailureStopsPipeline()
//         {
//             var result = Result.Success()
//                 .OnSuccess(() => Result.Failure())
//                 .OnSuccess(() => Result.Success(""));

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public async Task OnSuccess_TaskResult_ChainedSuccess()
//         {
//             var result = await Result.Success()
//                 .OnSuccess(() => Task.FromResult(Result.Success()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnSuccess_TaskResult_FailureStopsPipeline()
//         {
//             var result = await Result.Success()
//                 .OnSuccess(() => Result.Failure())
//                 .OnSuccess(() => Task.FromResult(Result.Success()));

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public async Task OnSuccess_Task_ResultOfT_ChainedSuccess()
//         {
//             var result = await Result.Success()
//                 .OnSuccess(() => Task.FromResult(Result.Success("string")));

//             Assert.True(result.IsSuccessful);
//             Assert.Equal("string", result.Value);
//         }

//         [Fact]
//         public async Task OnSuccess_Task_ResultOfT_FailureStopsPipeline()
//         {
//             var result = await Result.Success()
//                 .OnSuccess(() => Result.Failure())
//                 .OnSuccess(() => Task.FromResult(Result.Success("")));

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void OnFailure_Result_ChainedFailure()
//         {
//             var result = Result.Failure()
//                 .OnFailure(() => Result.Failure());

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public void OnFailure_Result_SuccessStopsPipeline()
//         {
//             var result = Result.Failure()
//                 .OnFailure(() => Result.Success())
//                 .OnFailure(() => Result.Failure());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void OnFailure_ResultOfT_ChainedFailure()
//         {
//             var result = Result.Failure()
//                 .OnFailure(() => Result.Failure<string>("string"));

//             Assert.True(result.IsFailure);
//             Assert.Equal("string", result.Messages.First());
//         }

//         [Fact]
//         public void OnFailure_ResultOfT_SuccessStopsPipeline()
//         {
//             var result = Result.Failure()
//                 .OnFailure(() => Result.Success())
//                 .OnFailure(() => Result.Failure<string>());

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnFailure_TaskResult_ChainedFailure()
//         {
//             var result = await Result.Failure()
//                 .OnFailure(() => Task.FromResult(Result.Failure()));

//             Assert.True(result.IsFailure);
//         }

//         [Fact]
//         public async Task OnFailure_TaskResult_SuccessStopsPipeline()
//         {
//             var result = await Result.Failure()
//                 .OnFailure(() => Result.Success())
//                 .OnFailure(() => Task.FromResult(Result.Failure()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public async Task OnFailure_Task_ResultOfT_ChainedFailures()
//         {
//             var result = await Result.Failure()
//                 .OnFailure(() => Task.FromResult(Result.Failure<string>("string")));

//             Assert.True(result.IsFailure);
//             Assert.Equal("string", result.Messages.First());
//         }

//         [Fact]
//         public async Task OnFailure_Task_ResultOfT_SuccessStopsPipeline()
//         {
//             var result = await Result.Failure()
//                 .OnFailure(() => Result.Success())
//                 .OnFailure(() => Task.FromResult(Result.Failure<string>()));

//             Assert.True(result.IsSuccessful);
//         }

//         [Fact]
//         public void Join()
//         {
//             var r1 = Result.Success();
//             var r2 = Result.Success();
//             var r3 = r1.Join(r2);

//             Assert.True(r3.IsSuccessful);
//         }

//         [Fact]
//         public void CanAddMessages()
//         {
//             var result = Result.Success();
            
//             var resultWithMessage = result
//                 .WithMessages("jon snow")
//                 .WithMessages("is a bastard");
                
//             Assert.Equal("jon snow", resultWithMessage.Messages.First());
//             Assert.Equal("is a bastard", resultWithMessage.Messages.ElementAt(1));
//         }
//     }
// }
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ergo.Tests
{
    public class ResultOfTTests
    {
        [Fact]
        public void CanCreateSuccess()
        {
            var result = Result.Success("string");
            Assert.True(result.IsSuccessful);
            Assert.Equal("string", result.Value);
        }

        [Fact]
        public void CanCreateFailure()
        {
            var result = Result.Failure<string>("string");
            Assert.True(result.IsFailure);
            Assert.Equal("string", result.Messages.First());
        }

        [Fact]
        public void CanCreateFailureWithMessage()
        {
            var result = Result.Failure<int>("message");
            Assert.True(result.Messages.First() == "message");
        }

        [Fact]
        public void CanCreateFailureWithMessages()
        {
            var result = Result.Failure<int>("message", "message1", "message2");
            var intersectionCount = result.Messages.Intersect(new [] {"message", "message1", "message2"}).Count();
            
            Assert.Equal(3, intersectionCount);
        }

        [Fact]
        public void CanCreateFailureWithMessageList()
        {
            var result = Result.Failure<int>(new [] { "message", "message1", "message2" });
            var intersectionCount = result.Messages.Intersect(new [] {"message", "message1", "message2"}).Count();
            
            Assert.Equal(3, intersectionCount);
        }

        [Fact]
        public void Can2SuccessesBeCombinedForSuccess()
        {
            var result = Result.Success(1) + Result.Success(2);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Can1FailureMakeSuccessFailure()
        {
            var result = Result.Success(1) + Result.Failure<int>();
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void Can2FailuresBeCombinedForFailure()
        {
            var result = Result.Failure<int>() + Result.Failure<int>();
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void Can2FailuresBeCombinedForFailureWithMessages()
        {
            var result = Result.Failure<int>("a", "b") + Result.Failure<string>("c", "d");

            var intersectionCount = result.Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
            Assert.Equal(4, intersectionCount);
        }

        [Fact]
        public async Task CanBeJoinedWithTaskResult()
        {
            var asyncResult = Result.Failure<int>("a", "b") + Task.FromResult(Result.Failure<int>("c", "d"));

            var intersectionCount = (await asyncResult).Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
            Assert.Equal(4, intersectionCount);
        }

        [Fact]
        public async Task CanBeJoinedWithAsyncResult()
        {
            var asyncResult = Result.Failure<string>("a", "b") + ((AsyncResult<string>)Task.FromResult(Result.Failure<string>("c", "d")));

            var intersectionCount = (await asyncResult).Messages.Intersect(new [] {"a", "b", "c", "d"}).Count();
            Assert.Equal(4, intersectionCount);
        }

        [Fact]
        public void OnSuccess_Result_CanPassArgument_Success()
        {
            var result = Result.Success(1)
                .OnSuccess((_) => Result.Success());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void OnSuccess_Result_CanPassArgument_Failure()
        {
            var result = Result.Failure<int>()
                .OnSuccess((_) => Result.Success());

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void OnSuccess_ResultOfT_CanPassArgument_Success()
        {
            var result = Result.Success(1)
                .OnSuccess((_) => Result.Success(2));

            Assert.True(result.IsSuccessful);
            Assert.Equal(2, result.Value);
        }

        [Fact]
        public void OnSuccess_ResultOfT_CanPassArgument_Failure()
        {
            var result = Result.Failure<int>()
                .OnSuccess((_) => Result.Success(3));

            Assert.True(result.IsFailure);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void OnSuccess_ResultOfT_CanPassArgument_Failure_DifferentType()
        {
            var result = Result.Failure<int>()
                .OnSuccess((_) => Result.Success("hello"));

            Assert.True(result.IsFailure);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public async Task OnSuccess_TaskResultOfT_CanPassArgument_Success()
        {
            var result = await Result.Success(1)
                .OnSuccess((_) => Task.FromResult(Result.Success(2)));

            Assert.True(result.IsSuccessful);
            Assert.Equal(2, result.Value);
        }

        [Fact]
        public async Task OnSuccess_TaskResultOfT_CanPassArgument_Failure()
        {
            var result = await Result.Failure<int>()
                .OnSuccess((_) => Task.FromResult(Result.Success(3)));

            Assert.True(result.IsFailure);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public async Task OnSuccess_TaskResultOfT_CanPassArgument_Failure_DifferentType()
        {
            var result = await Result.Failure<int>()
                .OnSuccess((_) => Task.FromResult(Result.Success("hello")));

            Assert.True(result.IsFailure);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public async Task OnSuccess_Task_CanPassArgument_Success()
        {
            var result = await Result.Success(1)
                .OnSuccess((_) => Task.FromResult(Result.Success()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnSuccess_Task_CanPassArgument_Failure()
        {
            var result = await Result.Failure<int>()
                .OnSuccess((_) => Task.FromResult(Result.Success()));

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void OnFailure_ResultOfT_ChainedFailure_()
        {
            var result = Result.Failure<int>()
                .OnFailure((_) => Result.Failure());

            Assert.True(result.IsFailure);
        }

        [Fact]
        public void OnFailure_Result_CanPassArgument_Failure()
        {
            var result = Result.Failure<int>()
                .OnFailure((_) => Result.Success());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void OnFailure_Result_CanPassArgument_Success()
        {
            var result = Result.Success(1)
                .OnFailure((_) => Result.Failure());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void OnFailure_ResultOfT_CanPassArgument_Success()
        {
            var result = Result.Success(1)
                .OnFailure((_) => Result.Success(2));

            Assert.True(result.IsSuccessful);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void OnFailure_ResultOfT_CanPassArgument_Failure()
        {
            var result = Result.Failure<int>()
                .OnFailure((_) => Result.Success(3));

            Assert.True(result.IsSuccessful);
            Assert.Equal(3, result.Value);
        }

        [Fact]
        public void OnFailure_ResultOfT_CanPassArgument_Failure_DifferentType()
        {
            var result = Result.Success(1)
                .OnFailure((_) => Result.Success("hello"));

            Assert.True(result.IsSuccessful);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public async Task OnFailure_TaskResultOfT_CanPassArgument_Success()
        {
            var result = await Result.Success(1)
                .OnFailure((_) => Task.FromResult(Result.Success(2)));

            Assert.True(result.IsSuccessful);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public async Task OnFailure_TaskResultOfT_CanPassArgument_Failure()
        {
            var result = await Result.Failure<int>()
                .OnFailure((_) => Task.FromResult(Result.Success(3)));

            Assert.True(result.IsSuccessful);
            Assert.Equal(3, result.Value);
        }

        [Fact]
        public async Task OnFailure_TaskResultOfT_CanPassArgument_Failure_DifferentType()
        {
            var result = await Result.Success(1)
                .OnFailure((_) => Task.FromResult(Result.Success("hello")));

            Assert.True(result.IsSuccessful);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public async Task OnFailure_Task_CanPassArgument_Failure()
        {
            var result = await Result.Failure<int>()
                .OnFailure((_) => Task.FromResult(Result.Success()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_CanPassArgument_Success()
        {
            var result = await Result.Success(1)
                .OnFailure((_) => Task.FromResult(Result.Success()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ImplicitCast_T_ResultOfT()
        {
            Result<string> implicitValue = "test";

            Assert.True(implicitValue.IsSuccessful);
            Assert.Equal("test", implicitValue.Value);
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ergo.Tests
{
    public class AsyncResultTests
    {
        [Fact]
        public async Task OnSuccess_Result_ChainedSuccess()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Result.Success());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnSuccess_Result_FailureStopsPipeline()
        {
            var result = await ((AsyncResult<string>)Result.Success("d"))
                .OnSuccess((_) => Result.Failure())
                .OnSuccess(() => Result.Success());

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnSuccess_ResultOfT_ChainedSuccess()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Result.Success("string"));

            Assert.True(result.IsSuccessful);
            Assert.Equal("string", result.Value);
        }

        [Fact]
        public async Task OnSuccess_ResultOfT_FailureStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Result.Failure())
                .OnSuccess(() => Result.Success(""));

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnSuccess_TaskResult_ChainedSuccess()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Task.FromResult(Result.Success()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnSuccess_TaskResult_FailureStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Result.Failure())
                .OnSuccess(() => Task.FromResult(Result.Success()));

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnSuccess_Task_ResultOfT_ChainedSuccess()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Task.FromResult(Result.Success("string")));

            Assert.True(result.IsSuccessful);
            Assert.Equal("string", result.Value);
        }

        [Fact]
        public async Task OnSuccess_Task_ResultOfT_FailureStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Success())
                .OnSuccess(() => Result.Failure())
                .OnSuccess(() => Task.FromResult(Result.Success("")));

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnFailure_Result_ChainedFailure()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Failure());

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnFailure_Result_SuccessStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Success())
                .OnFailure(() => Result.Failure());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_ResultOfT_ChainedFailure()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Failure<string>("string"));

            Assert.True(result.IsFailure);
            Assert.Equal("string", result.Messages.First());
        }

        [Fact]
        public async Task OnFailure_ResultOfT_SuccessStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Success())
                .OnFailure(() => Result.Failure<string>());

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_TaskResult_ChainedFailure()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Task.FromResult(Result.Failure()));

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task OnFailure_TaskResult_SuccessStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Success())
                .OnFailure(() => Task.FromResult(Result.Failure()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_ChainedFailures()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Task.FromResult(Result.Failure<string>("string")));

            Assert.True(result.IsFailure);
            Assert.Equal("string", result.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_SuccessStopsPipeline()
        {
            var result = await ((AsyncResult)Result.Failure())
                .OnFailure(() => Result.Success())
                .OnFailure(() => Task.FromResult(Result.Failure<string>()));

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ConstructorDisallowsNulls()
        {
            Assert.Throws<ArgumentNullException>(() => {
                new AsyncResult(null);
            });
        }

        [Fact]
        public async Task ImplicitCast_T_ResultOfT()
        {
            AsyncResult<string> implicitValue = "test";

            var result = await implicitValue;
            Assert.True(result.IsSuccessful);
            Assert.Equal("test", result.Value);
        }

        [Fact]
        public async Task CanAddMessages()
        {
            AsyncResult result = Result.Success();
            
            var resultWithMessage = await result
                .WithMessages("jon snow")
                .WithMessages("is a bastard");
                
            Assert.Equal("jon snow", resultWithMessage.Messages.First());
            Assert.Equal("is a bastard", resultWithMessage.Messages.ElementAt(1));
        }
    }
}
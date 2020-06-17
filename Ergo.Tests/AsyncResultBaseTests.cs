using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Ergo.Tests
{
    public class AsyncResultBaseTests
    {
        [Fact]
        public async Task AddAsyncResults()
        {
            AsyncResult r1 = Result.Success();
            AsyncResult r2 = Result.Success();

            var r3 = await (r1 + r2);
            Assert.True(r3.IsSuccessful);
        }

        [Fact]
        public async Task AddAsyncResults_AND_Result()
        {
            AsyncResult r1 = Result.Success();
            Result r2 = Result.Success();

            var r3 = await (r1 + r2);
            Assert.True(r3.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_Result_Fail()
        {
            AsyncResult result = Result.Failure();
            var response = await result.OnFailure(() => Task.FromResult(Result.Success()));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_Result_Success()
        {
            AsyncResult result = Result.Success();
            var response = await result.OnFailure(() => Task.FromResult(Result.Success()));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_Fail()
        {
            AsyncResult result = Result.Failure();
            var response = await result.OnFailure(() => Task.FromResult(Result.Success("hi")));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_Success()
        {
            AsyncResult result = Result.Success();
            var response = await result.OnFailure(() => Task.FromResult(Result.Success("hi")));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Result_Fail()
        {
            AsyncResult result = Result.Failure();
            var response = await result.OnFailure(() => Result.Success());

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Result_Success()
        {
            AsyncResult result = Result.Success();
            var response = await result.OnFailure(() => Result.Success());

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_ResultOfT_Fail()
        {
            AsyncResult result = Result.Failure();
            var response = await result.OnFailure(() => Result.Success("hi"));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_ResultOfT_Success()
        {
            AsyncResult result = Result.Success();
            var response = await result.OnFailure(() => Result.Success("hi"));

            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public async Task OnFailure_Task_Result_Fail_WithMessages()
        {
            AsyncResult result = Result.Failure("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success();
                successResult = successResult.WithMessages(messages.First());

                return Task.FromResult(successResult);
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Task_Result_Success_WithMessages()
        {
            AsyncResult result = Result.Success().WithMessages("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success();
                successResult = successResult.WithMessages(messages.First());

                return Task.FromResult(successResult);
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_Fail_WithMessages()
        {
            AsyncResult result = Result.Failure("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success("hi");
                successResult = successResult.WithMessages(messages.First());

                return Task.FromResult(successResult);
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Task_ResultOfT_Success_WithMessages()
        {
            AsyncResult result = Result.Success().WithMessages("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success("hi");
                successResult = successResult.WithMessages(messages.First());

                return Task.FromResult(successResult);
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Result_Fail_WithMessages()
        {
            AsyncResult result = Result.Failure("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success();
                successResult = successResult.WithMessages(messages.First());

                return successResult;
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_Result_Success_WithMessages()
        {
            AsyncResult result = Result.Success().WithMessages("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success();
                successResult = successResult.WithMessages(messages.First());

                return successResult;
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_ResultOfT_Fail_WithMessages()
        {
            AsyncResult result = Result.Failure("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success(9);
                successResult = successResult.WithMessages(messages.First());

                return successResult;
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task OnFailure_ResultOfT_Success_WithMessages()
        {
            AsyncResult result = Result.Success().WithMessages("a");
            var response = await result.OnFailure((messages) => {
                var successResult = Result.Success(9);
                successResult = successResult.WithMessages(messages.First());

                return successResult;
            });

            Assert.True(response.IsSuccessful);
            Assert.Equal("a", response.Messages.First());
        }

        [Fact]
        public async Task JoinAsyncResultBase()
        {
            var result1 = (AsyncResult)Result.Success() as AsyncResultBase;
            var result2 = (AsyncResult)Result.Success() as AsyncResultBase;

            var result3 = AsyncResult.Join(result1, result2);
            Assert.True((await result3).IsSuccessful);
        }

        [Fact]
        public async Task JoinAsyncResultBase_OfT()
        {
            AsyncResult<string> result1 = Result.Success("a");
            AsyncResult<string> result2 = Result.Success("b");

            var result3 = result1 + result2;
            Assert.True((await result3).IsSuccessful);
        }
    }
}
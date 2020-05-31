using System.Threading.Tasks;
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
    }
}
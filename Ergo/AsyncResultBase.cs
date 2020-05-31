using System.Linq;
using System.Threading.Tasks;

namespace Ergo
{
    public abstract class AsyncResultBase
    {
        public abstract Task<Result> GetTaskResult();

        public static AsyncResult operator +(AsyncResultBase a, AsyncResultBase b) =>
            Join(a, b);

        public static AsyncResult operator +(AsyncResultBase a, Result b) =>
            Join(a, b);

        public static AsyncResult Join(AsyncResultBase a, AsyncResultBase b) => JoinA(a, b);
        private static async Task<Result> JoinA(AsyncResultBase a, AsyncResultBase b)
        {
            var aResult = await a.GetTaskResult();
            var bResult = await b.GetTaskResult();

            var messages = aResult.Messages.Concat(bResult.Messages);
            var isSuccessful = aResult.IsSuccessful && bResult.IsSuccessful;
            return new Result(messages, isSuccessful);
        }

        public static AsyncResult Join(AsyncResultBase a, Result b) => JoinA(a, b);
        private static async Task<Result> JoinA(AsyncResultBase a, Result b)
        {
            var aResult = await a.GetTaskResult();
            return Result.Join(aResult, b);
        }
    }
}
using System;
using System.Threading.Tasks;

namespace TestTask.ErrorHandlers
{
    public interface IErrorHandler
    {
        /// <summary></summary>
        /// <param name="exception"></param>
        /// <returns>true if exception is handled and doesn't need to be thrown</returns>
        Task<bool> HandleAsync(Exception exception);
    }
}

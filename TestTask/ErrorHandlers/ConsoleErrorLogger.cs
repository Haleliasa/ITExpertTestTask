using System;
using System.Threading.Tasks;

namespace TestTask.ErrorHandlers
{
    public class ConsoleErrorLogger : IErrorHandler
    {
        public Task<bool> HandleAsync(Exception exception)
        {
            Console.WriteLine(exception.Message);
            // blocking one thread just for a quick solution
            return Task.Run(() => {
                Console.ReadKey(true);
                return true;
            });
        }
    }
}

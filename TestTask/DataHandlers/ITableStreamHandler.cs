using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTask.DataHandlers
{
    /// <summary>
    /// Handles table data in a stream way
    /// </summary>
    public interface ITableStreamHandler : IDisposable
    {
        Task HandleKeysAsync(IEnumerable<string> keys);

        Task HandleRowAsync(IEnumerable values);

        Task CloseAsync();
    }
}

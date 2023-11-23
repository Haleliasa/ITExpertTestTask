using System.Threading.Tasks;
using TestTask.DataHandlers;

namespace TestTask.DataProviders
{
    public interface ITableDataProvider
    {
        /// <summary>
        /// Gets and handles table data in a stream way
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        Task GetDataAsync(ITableStreamHandler handler);
    }
}

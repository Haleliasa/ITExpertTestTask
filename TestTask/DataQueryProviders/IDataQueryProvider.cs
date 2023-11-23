using System.Threading.Tasks;

namespace TestTask.DataQueryProviders
{
    public interface IDataQueryProvider
    {
        Task<string> GetQueryAsync();
    }
}

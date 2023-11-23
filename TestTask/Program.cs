using System.Configuration;
using System.Threading.Tasks;
using TestTask.DataHandlers;
using TestTask.DataProviders;
using TestTask.DataQueryProviders;
using TestTask.ErrorHandlers;

namespace TestTask
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string connectionStringName =
                ConfigurationManager.AppSettings["ConnectionStringName"]
                ?? "DefaultLocalDb";
            ConnectionStringSettings connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName];

            IDataQueryProvider queryProvider = new FileDataQueryProvider("input.txt");
            ITableDataProvider dataProvider = new SqlDataProvider(
                connectionString.ConnectionString,
                queryProvider,
                errorHandler: new ConsoleErrorLogger());
            ITableStreamHandler handler = new CsvTableStreamExporter("output.csv");

            await ProcessTableDataAsync(dataProvider, handler);

            await handler.CloseAsync();
            handler.Dispose();
        }

        // Accepts any implementation of ITableDataProvider and ITableStreamHandler
        private static Task ProcessTableDataAsync(
            ITableDataProvider dataProvider,
            ITableStreamHandler handler)
        {
            return dataProvider.GetDataAsync(handler);
        }
    }
}

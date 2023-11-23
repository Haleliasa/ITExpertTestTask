using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestTask.DataQueryProviders;
using TestTask.DataHandlers;
using System.Collections;
using TestTask.ErrorHandlers;
using System;

namespace TestTask.DataProviders
{
    public class SqlDataProvider : ITableDataProvider
    {
        public SqlDataProvider(
            string connectionString,
            IDataQueryProvider sqlProvider,
            IErrorHandler errorHandler = null)
        {
            ConnectionString = connectionString;
            SqlProvider = sqlProvider;
            ErrorHandler = errorHandler;
        }

        public string ConnectionString { get; set; }

        public IDataQueryProvider SqlProvider { get; set; }

        public IErrorHandler ErrorHandler { get; set; }

        public async Task GetDataAsync(ITableStreamHandler handler)
        {
            try
            {
                await GetDataInternalAsync(handler);
            }
            catch (Exception e)
            {
                bool handled = false;
                if (ErrorHandler != null)
                {
                    handled = await ErrorHandler.HandleAsync(e);
                }
                if (!handled)
                {
                    throw;
                }
            }
        }

        private async Task GetDataInternalAsync(ITableStreamHandler handler)
        {
            string sql = await SqlProvider.GetQueryAsync();
            sql = SqlUtils.SelectOnly(sql);
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.HasRows)
                {
                    IEnumerable<int> colNums = Enumerable.Range(0, reader.FieldCount);
                    IEnumerable<string> keys = colNums.Select(i => reader.GetName(i));
                    await handler.HandleKeysAsync(keys);
                    while (await reader.ReadAsync())
                    {
                        IEnumerable values = colNums.Select(i => reader.GetValue(i));
                        await handler.HandleRowAsync(values);
                    }
                    await reader.NextResultAsync();
                }
            }
        }
    }
}

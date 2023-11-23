using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.DataQueryProviders
{
    public class FileDataQueryProvider : IDataQueryProvider
    {
        public FileDataQueryProvider(string fileName)
        {
            FileName = fileName;
        }

        public FileDataQueryProvider(string fileName, Encoding encoding)
            : this(fileName)
        {
            Encoding = encoding;
        }

        public string FileName { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public async Task<string> GetQueryAsync()
        {
            string query;
            using (StreamReader reader = new StreamReader(FileName, Encoding))
            {
                query = await reader.ReadToEndAsync();
            }
            return query;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.DataHandlers
{
    public class CsvTableStreamExporter : ITableStreamHandler
    {
        public const char DefaultDelimeter = ';';

        private readonly string _fileName;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly string _delimeter;
        private StreamWriter _writer;
        private int _fileCount = 0;
        private int _fieldCount = -1;

        public CsvTableStreamExporter(
            string fileName,
            char delimeter = DefaultDelimeter)
        {
            _fileName = fileName;
            _delimeter = delimeter.ToString();
        }

        public CsvTableStreamExporter(
            string fileName,
            Encoding encoding,
            char delimeter = DefaultDelimeter)
            : this(fileName, delimeter)
        {
            _encoding = encoding;
        }

        public Task HandleKeysAsync(IEnumerable<string> keys)
        {
            CreateWriter(); // forcibly create new writer
            List<string> keysBuffer = keys.ToList();
            _fieldCount = keysBuffer.Count;
            if (_fieldCount == 0)
            {
                return Task.CompletedTask;
            }
            return _writer.WriteLineAsync(
                string.Join(_delimeter, keysBuffer));
        }

        public Task HandleRowAsync(IEnumerable values)
        {
            if (_fieldCount == 0)
            {
                return Task.CompletedTask;
            }
            if (_writer == null)
            {
                CreateWriter();
            }
            IEnumerable<string> valueStrings = values.CastToString();
            if (_fieldCount != -1)
            {
                valueStrings = valueStrings.TakeOrAppend(_fieldCount);
            }
            else
            {
                List<string> valueStringsBuffer = valueStrings.ToList();
                _fieldCount = valueStringsBuffer.Count;
                if (_fieldCount == 0)
                {
                    return Task.CompletedTask;
                }
                valueStrings = valueStringsBuffer;
            }
            return _writer.WriteLineAsync(
                string.Join(_delimeter, valueStrings));
        }

        public Task CloseAsync()
        {
            _writer?.Close();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }

        private void CreateWriter()
        {
            _writer?.Dispose();
            _fileCount++;
            string fileName =
                _fileCount == 1
                ? _fileName
                : $"{_fileName} ({_fileCount})";
            _writer = new StreamWriter(fileName, append: false, _encoding);
        }
    }
}

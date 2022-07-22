namespace Archaeopteryx.Initializer;
internal class CsvReader
{
		private readonly StreamReader csvFile;

		public CsvReader(string filename, bool hasHeader = false)
		{
				csvFile = new StreamReader(new FileStream(filename, FileMode.Open));

				if (hasHeader)
				{
						ReadLineAsync().Wait();
				}
		}

		public bool HasNext => !csvFile.EndOfStream;

		public async Task<string[]?> ReadLineAsync()
		{
				if (!HasNext) return null;

        var line = await csvFile.ReadLineAsync();

				// Ignore blank and comment lines
				while (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
				{
						if (!HasNext) return null;
            line = await csvFile.ReadLineAsync();
				}

				return line.Split(',');
		}

		public void Close()
		{
				csvFile.Close();
		}
}

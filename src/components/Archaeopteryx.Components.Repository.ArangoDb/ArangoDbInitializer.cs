using ArangoDBNetStandard;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.Transport.Http;
using Archaeopteryx.Components.Repository.Abstractions;

namespace Archaeopteryx.Components.Repository.ArangoDb;

public class ArangoDbInitializer : IDbInitializer
{
		private const string DatabaseName = "Archaeopteryx";
		private static readonly string[] DocumentCollections = { "Metadata", "Domain", "Product", "Capability", "Service", "Component", "Integration" };

		public async Task InitializeDbAsync()
		{
				await CreateDatabase();
				await CreateDocumentCollections();
		}

		private static async Task CreateDatabase()
		{
				using var transport = HttpApiTransport.UsingNoAuth(ArangoDbConstants.HostUri);
				using var db = new ArangoDBClient(transport);

				await db.Database.PostDatabaseAsync(new PostDatabaseBody
				{
						Name = DatabaseName
				});
		}

		private static async Task CreateDocumentCollections()
		{
				using var transport = HttpApiTransport.UsingNoAuth(ArangoDbConstants.HostUri, DatabaseName);
				using var db = new ArangoDBClient(transport);

				foreach (var entity in DocumentCollections)
				{
						await db.Collection.PostCollectionAsync(new PostCollectionBody
						{
								Type = CollectionType.Document,
								Name = entity
						});
				}
		}
}

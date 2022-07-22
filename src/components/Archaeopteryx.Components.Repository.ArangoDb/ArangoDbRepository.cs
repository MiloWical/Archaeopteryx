using ArangoDBNetStandard;
using ArangoDBNetStandard.CollectionApi.Models;
using ArangoDBNetStandard.DatabaseApi.Models;
using ArangoDBNetStandard.Serialization;
using ArangoDBNetStandard.Transport.Http;
using Archaeopteryx.Components.Repository.Abstractions;

namespace Archaeopteryx.Components.Repository.ArangoDb;
public class ArangoDbRepository : IDbInitializer
{
		private readonly JsonNetApiClientSerialization serializer;
		private ArangoDBClient? archaeopteryxDb;

		public ArangoDbRepository()
		{
				serializer = new JsonNetApiClientSerialization();
				serializer.DefaultOptions.UseCamelCasePropertyNames = true;
				serializer.DefaultOptions.IgnoreNullValues = true;
		}

		public async Task InitializeDbAsync()
		{
				using var transport = HttpApiTransport.UsingNoAuth(ArangoDbConstants.HostUri);
				using var db = new ArangoDBClient(transport);

				var databases = await db.Database.GetUserDatabasesAsync();

				if (!databases.Result.Contains(ArangoDbConstants.DatabaseName))
				{
						await db.Database.PostDatabaseAsync(new PostDatabaseBody
						{
								Name = ArangoDbConstants.DatabaseName
						});
				}

				var archaeopteryxTransport = HttpApiTransport.UsingNoAuth(ArangoDbConstants.HostUri, ArangoDbConstants.DatabaseName);
				archaeopteryxDb = new ArangoDBClient(archaeopteryxTransport, serializer);
		}

		public async Task InitializeCollectionAsync(string collectionName)
		{
				await InitializeCollectionAsync(collectionName, CollectionType.Document);
		}

		public async Task InitializeCollectionAsync(string collectionName, CollectionType collectionType)
		{
				var collections = await archaeopteryxDb.Collection.GetCollectionsAsync();
				if (collections.Result.Any(c => c.Name == collectionName)) return;

				await archaeopteryxDb.Collection.PostCollectionAsync(new PostCollectionBody
				{
						Type = collectionType,
						Name = collectionName
				});
		}

		public async Task InitializePropertyAsync<TProperty>(TProperty property)
		{
				await archaeopteryxDb.Document.PostDocumentAsync(ArangoDbConstants.PropertyCollectionName, property);
		}

		public async Task InitializeEntityAsync<TEntity>(string collectionName, TEntity entity)
		{
				await archaeopteryxDb.Document.PostDocumentAsync(collectionName, entity);
		}

		public async Task InitializeRelationAsync(string from, string type, string to)
		{
				await archaeopteryxDb.Document.PostDocumentAsync(ArangoDbConstants.RelationCollectionName,
						new
						{
								_from = from,
								type = type,
								_to = to
						});
		}
}

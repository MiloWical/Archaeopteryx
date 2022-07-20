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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		private ArangoDBClient archaeopteryxDb;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public ArangoDbRepository()
		{
				serializer = new JsonNetApiClientSerialization();
				serializer.DefaultOptions.UseCamelCasePropertyNames = true;
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
				archaeopteryxDb = new ArangoDBClient(archaeopteryxTransport);
		}

		public async Task InitializeCollectionAsync(string collectionName)
		{
				var collections = await archaeopteryxDb.Collection.GetCollectionsAsync();
				if (collections.Result.Any(c => c.Name == collectionName)) return;

				await archaeopteryxDb.Collection.PostCollectionAsync(new PostCollectionBody
				{
						Type = CollectionType.Document,
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
}

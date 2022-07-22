using ArangoDBNetStandard.CollectionApi.Models;
using Archaeopteryx.Components.Extensions;
using Archaeopteryx.Components.Repository.Abstractions;
using Archaeopteryx.Components.Repository.ArangoDb;
using Archaeopteryx.Domains.Models;
using Archaeopteryx.Initializer;

var initializer = new ArangoDbRepository();

await InitializeDatabase(initializer);
await LoadEntityTypesAsync(initializer, args[0]);
await LoadPropertyTypesAsync(initializer, args[1]);
var relationTypes = await LoadRelationTypesAsync(initializer, args[2]);
await LoadEntitiesAsync(initializer, args[3]);
await LoadRelationsAsync(initializer, args[4], relationTypes);

static async Task InitializeDatabase(IDbInitializer initializer)
{
		Console.WriteLine("Initializing database...");

		await initializer.InitializeDbAsync();

		Console.WriteLine("Database initialized!");
}

static async Task LoadEntityTypesAsync(IDbInitializer initializer, string entityTypeCsvFilename)
{
		Console.WriteLine("Creating entity types...");

		await initializer.InitializeCollectionAsync(ArangoDbConstants.PropertyCollectionName);
		await ((ArangoDbRepository)initializer).InitializeCollectionAsync(ArangoDbConstants.RelationCollectionName, CollectionType.Edge);

		var typesReader = new CsvReader(entityTypeCsvFilename, true);

		while (typesReader.HasNext)
		{
				var type = (await typesReader.ReadLineAsync())![0];
				await initializer.InitializeCollectionAsync(type);
		}

		typesReader.Close();

		Console.WriteLine("Entity types created!");
}

static async Task LoadPropertyTypesAsync(IDbInitializer initializer, string propertyCsvFilename)
{
		Console.WriteLine("Loading property types...");

		var propertiesReader = new CsvReader(propertyCsvFilename, true);

		while (propertiesReader.HasNext)
		{
				var properties = await propertiesReader.ReadLineAsync();

				await initializer.InitializePropertyAsync(new Property
				{
						_key = properties![0].RemoveWhitespace(),
						Name = properties![0],
						Values = properties![1..]
				});
		}

		Console.WriteLine("Property types loaded!");
}

static async Task<Dictionary<string, Relation>> LoadRelationTypesAsync(IDbInitializer initializer, string relationCsvFilename)
{
		Console.WriteLine("Loading relationFields types...");

		var relationsReader = new CsvReader(relationCsvFilename, true);
		var relationLookup = new Dictionary<string, Relation>();

		while (relationsReader.HasNext)
		{
				var relationFields = await relationsReader.ReadLineAsync();

				var relation = new Relation
				{
						From = relationFields![0],
						Type = relationFields![1],
						To = relationFields![2]
				};

				relationLookup[relation.Type] = relation;

				await initializer.InitializePropertyAsync(relation);
		}

		Console.WriteLine("Relation types loaded!");
		return relationLookup;
}

static async Task LoadEntitiesAsync(IDbInitializer initializer, string entityCsvFilename)
{
		Console.WriteLine("Loading entities...");

		// Need the header line to define the property keys.
		var entityReader = new CsvReader(entityCsvFilename);

		var keys = await entityReader.ReadLineAsync();

		while (entityReader.HasNext)
		{
				var entityFields = await entityReader.ReadLineAsync();

				var entity = new Entity
				{
						_key = entityFields![0].RemoveWhitespace(),
						Name = entityFields![0]
				};

				var properties = new Dictionary<string, string>();
				for (var i = 2; i < keys!.Length; ++i)
				{
						properties[keys[i]] = entityFields[i];
				}

				entity.Properties = properties;

				await initializer.InitializeEntityAsync(entityFields![1], entity);
		}

		entityReader.Close();

		Console.WriteLine("Entities loaded!");
}

static async Task LoadRelationsAsync(IDbInitializer initializer, string relationCsvFilename, Dictionary<string, Relation> relationTypes)
{
		Console.WriteLine("Loading relations...");

		// Need the header line to define the property keys.
		var relationReader = new CsvReader(relationCsvFilename, true);

		while (relationReader.HasNext)
		{
				var relationFields = await relationReader.ReadLineAsync();

				var relationType = relationTypes[relationFields![1]];

				await initializer.InitializeRelationAsync(
						$"{relationType.From}/{relationFields![0]}".RemoveWhitespace(),
						relationFields![1],
						$"{relationType.To}/{relationFields![2]}".RemoveWhitespace());
		}

		relationReader.Close();

		Console.WriteLine("Relations loaded!");
}
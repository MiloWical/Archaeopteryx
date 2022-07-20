using Archaeopteryx.Components.Repository.Abstractions;
using Archaeopteryx.Components.Startup;
using Archaeopteryx.Domains.Models;
using Archaeopteryx.Initializer;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddArangoDb();

var serviceProvider = serviceCollection.BuildServiceProvider();

var initializer = serviceProvider.GetRequiredService<IDbInitializer>();

await InitializeDatabase(initializer);
await LoadCollectionsAsync(initializer, args[0]);
await LoadPropertiesAsync(initializer, args[1]);
await LoadEntitiesAsync(initializer, args[2]);

static async Task InitializeDatabase(IDbInitializer initializer)
{
		Console.WriteLine("Initializing database...");

		await initializer.InitializeDbAsync();

		Console.WriteLine("Database initialized!");
}

static async Task LoadCollectionsAsync(IDbInitializer initializer, string entityTypeCsvFilename)
{
		Console.WriteLine("Creating collections...");

		await initializer.InitializeCollectionAsync("Metadata");

		var typesReader = new CsvReader(entityTypeCsvFilename, true);

		while (typesReader.HasNext)
		{
				var type = (await typesReader.ReadLineAsync())![0];
				await initializer.InitializeCollectionAsync(type);
		}

		typesReader.Close();

		Console.WriteLine("Collections created!");
}

static async Task LoadPropertiesAsync(IDbInitializer initializer, string propertyCsvFilename)
{
		Console.WriteLine("Loading properties...");

		var propertiesReader = new CsvReader(propertyCsvFilename, true);

		while (propertiesReader.HasNext)
		{
				var properties = await propertiesReader.ReadLineAsync();

				await initializer.InitializePropertyAsync(new Property
				{
						Name = properties![0],
						Values = properties![1..]
				});
		}

		Console.WriteLine("Properties loaded!");
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
using Archaeopteryx.Components.Repository.Abstractions;
using Archaeopteryx.Components.Repository.ArangoDb;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<IDbInitializer, ArangoDbInitializer>();

var serviceProvider = serviceCollection.BuildServiceProvider();

Console.WriteLine("Initializing database...");

await serviceProvider.GetRequiredService<IDbInitializer>().InitializeDbAsync();

Console.WriteLine("Database initialized!");
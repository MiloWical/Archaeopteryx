using Archaeopteryx.Components.Repository.Abstractions;
using Archaeopteryx.Components.Repository.ArangoDb;
using Microsoft.Extensions.DependencyInjection;

namespace Archaeopteryx.Components.Startup;

public static class DependencyInjection
{
		public static IServiceCollection AddArangoDb(this IServiceCollection serviceCollection)
		{
				var repository = new ArangoDbRepository();

				return serviceCollection
						.AddSingleton<IDbInitializer>(repository);
		}

		//private static IServiceCollection AddArangoDbDatabaseInitializer(this IServiceCollection serviceCollection)
		//{
		//		serviceCollection.AddSingleton<I>
		//}
}

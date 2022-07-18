using Microsoft.Extensions.DependencyInjection;

namespace Archaeopteryx.Components.Startup;

public static class DependencyInjection
{
		public static IServiceCollection AddArangoDb(this IServiceCollection serviceCollection)
		{
				return serviceCollection;
		}

		private static IServiceCollection AddArangoDbDatabaseInitializer(this IServiceCollection serviceCollection)
		{
				serviceCollection.AddSingleton<I>
		}
}

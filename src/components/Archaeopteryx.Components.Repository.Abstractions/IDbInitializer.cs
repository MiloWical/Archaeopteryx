namespace Archaeopteryx.Components.Repository.Abstractions;

public interface IDbInitializer
{
		Task InitializeDbAsync();
		Task InitializeCollectionAsync(string collectionName);
		Task InitializePropertyAsync<TProperty>(TProperty property);
		Task InitializeEntityAsync<TEntity>(string collectionName, TEntity entity);
		Task InitializeRelationAsync(string from, string type, string to);
}
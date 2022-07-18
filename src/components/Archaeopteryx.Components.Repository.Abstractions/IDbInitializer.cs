namespace Archaeopteryx.Components.Repository.Abstractions;

public interface IDbInitializer
{
		Task InitializeDbAsync();
}
using Archaeopteryx.Domains.Models;

namespace Archaeopteryx.Components.Repository.Abstractions;

public interface IDomainModelRepository
{
		public IEnumerable<string> RetrieveDomainModelList();
		public Domain RetrieveDomainModel(string domainModelId);
}

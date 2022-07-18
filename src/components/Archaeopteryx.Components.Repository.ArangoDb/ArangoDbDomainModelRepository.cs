using Archaeopteryx.Components.Repository.Abstractions;
using Archaeopteryx.Domains.Models;

namespace Archaeopteryx.Components.Repository.ArangoDb;

public class ArangoDbDomainModelRepository : IDomainModelRepository
{
		public ArangoDbDomainModelRepository()
		{

		}

		public Domain RetrieveDomainModel(string domainModelId)
		{
				throw new NotImplementedException();
		}

		public IEnumerable<string> RetrieveDomainModelList()
		{
				throw new NotImplementedException();
		}
}

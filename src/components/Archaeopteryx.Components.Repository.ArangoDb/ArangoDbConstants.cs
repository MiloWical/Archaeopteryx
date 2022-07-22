namespace Archaeopteryx.Components.Repository.ArangoDb;

public class ArangoDbConstants
{
		public static readonly Uri HostUri = new("http://localhost:8529");
		public const string DatabaseName = "Archaeopteryx";
		public const string PropertyCollectionName = "Metadata";
		public const string RelationCollectionName = "Relation";
}

namespace Archaeopteryx.Domains.Models;
public class Relation
{
		public string Is => nameof(Relation);
		public string From { get; set; }
		public string Type { get; set; }
		public string To { get; set; }
}

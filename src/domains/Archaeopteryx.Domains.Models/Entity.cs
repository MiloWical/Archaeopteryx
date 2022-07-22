namespace Archaeopteryx.Domains.Models;

public class Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public string? _key { get; set; }
		public string Is => nameof(Entity);
		public Version Version { get; set; } = new Version(1, 0, 0);
		public string Name { get; set; }

		public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

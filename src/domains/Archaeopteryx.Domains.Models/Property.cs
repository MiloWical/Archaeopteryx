namespace Archaeopteryx.Domains.Models;
public class Property
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public string? _key { get; set; } = null;
		public string Is => nameof(Property);
		public Version Version { get; set; } = new Version(1, 0, 0);
		public string Name { get; init; }
		public string[] Values { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

namespace Archaeopteryx.Components.Extensions;
public static class StringExtensions
{
		public static string RemoveWhitespace(this string source)
		{
				char[] nonWhitespaceChars = new char[source.Length];
				var charCount = 0;

				for (var i = 0; i < source.Length; i++)
				{
						if (!char.IsWhiteSpace(source[i]))
						{
								nonWhitespaceChars[charCount++] = source[i];
						}
				}

				return new string(nonWhitespaceChars, 0, charCount);
		}
}

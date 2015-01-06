namespace FluentBrowserAutomation.Controls
{
	internal static class StringExtensions
	{
		public static string EscapeForXpath(this string text, string prefix)
		{
			var query = text.Contains("'")
				? "(" + prefix + "='" + text.Replace("'", "&apos;") + "' or " + prefix + "=\"" + text + "\")"
				: prefix + "='" + text + "'";
			return query;
		}
	}
}
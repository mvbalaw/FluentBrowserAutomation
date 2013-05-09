namespace FluentBrowserAutomation.Accessors
{
	public class ReadOnlyText : TextBase
	{
		public ReadOnlyText(string howFound, string text)
			: base(text, howFound)
		{
		}

		public static implicit operator string(ReadOnlyText textBase)
		{
			return textBase.Text;
		}
	}
}
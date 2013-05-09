namespace FluentBrowserAutomation.Controls
{
	public class MissingInputWrapper : BasicInfoWrapper, IAmInputThatCanBeChanged
	{
		public MissingInputWrapper(string howFound, IBrowserContext browserContext)
			: base(null, howFound, browserContext)
		{
		}
	}
}
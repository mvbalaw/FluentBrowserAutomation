namespace FluentBrowserAutomation.Controls
{
	public class FileUploadWrapper : BasicInfoWrapper, IAmTextInput, INeedFocus, IAmInputThatCanBeChanged
	{
		public FileUploadWrapper(RemoteWebElementWrapper element, string howFound, IBrowserContext browserContext)
			: base(element, howFound, browserContext)
		{
		}

		public dynamic SetTo(string text)
		{
			this.ScrollToIt();
			Element.SendKeys(text);
			return this;
		}
	}
}
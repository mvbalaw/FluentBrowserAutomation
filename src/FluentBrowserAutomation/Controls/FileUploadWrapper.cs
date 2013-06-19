using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class FileUploadWrapper : BasicInfoWrapper, IAmTextInput, INeedFocus, IAmInputThatCanBeChanged
	{
		public FileUploadWrapper(IWebElement element, string howFound, IBrowserContext browserContext)
			: base(element, howFound, browserContext)
		{
		}

		public void SetTo(string text)
		{
			this.ScrollToIt();
			Element.SendKeys(text);
		}
	}
}
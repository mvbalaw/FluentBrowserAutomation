using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class PageWrapper
	{
		private readonly IBrowserContext _browserContext;

		public PageWrapper(IBrowserContext browserContext)
		{
			_browserContext = browserContext;
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText("Page", _browserContext.Browser.PageSource);
		}

		public string Title()
		{
			return _browserContext.Browser.Title;
		}
	}
}
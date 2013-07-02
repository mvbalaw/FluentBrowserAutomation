using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class PageWrapper
	{
		private readonly IBrowserContext _browserContext;

		public PageWrapper(IBrowserContext browserContext)
		{
			_browserContext = browserContext;
		}

		public void ScrollToBottom()
		{
			var browser = _browserContext.Browser;

			const string js = "window.scrollTo(0, document.body.scrollHeight);";
			((IJavaScriptExecutor)browser).ExecuteScript(js);
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
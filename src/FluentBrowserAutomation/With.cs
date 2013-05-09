using FluentBrowserAutomation.Framework;

using OpenQA.Selenium.Remote;

namespace FluentBrowserAutomation
{
	public static class With
	{
		public static BrowserContext Browser<T>(string initialUrl = null, bool uniqueBrowserEachTime = false) where T : RemoteWebDriver, new()
		{
			var browserManager = new BrowserManager<T>(initialUrl, uniqueBrowserEachTime);
			var browserContext = new BrowserContext(browserManager);
			if (initialUrl != null)
			{
				browserContext.GoToUrl(initialUrl);
			}
			return browserContext;
		}
	}
}
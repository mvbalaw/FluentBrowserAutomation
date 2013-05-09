using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class Children
	{
		public Children(IWebElement element, string howFound, IBrowserContext browserContext)
		{
			Element = element;
			HowFound = howFound;
			BrowserContext = browserContext;
		}

		public IBrowserContext BrowserContext { get; private set; }
		public IWebElement Element { get; private set; }
		public string HowFound { get; private set; }

		public IEnumerable<LinkWrapper> Links()
		{
			return Element
				.FindElements(By.TagName("a"))
				.Select(x => new LinkWrapper(x, HowFound + " .links", BrowserContext));
		}
	}
}
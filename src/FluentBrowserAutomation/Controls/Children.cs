using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Controls
{
	public class Children
	{
		public Children(RemoteWebElementWrapper element, string howFound, IBrowserContext browserContext)
		{
			Element = element;
			HowFound = howFound;
			BrowserContext = browserContext;
		}

		public IEnumerable<LinkWrapper> Links()
		{
			return Element.GetChildElementsByTagName("a")
				.Select((x, i) => new LinkWrapper(new RemoteWebElementWrapper(() => Element.GetChildElementsByTagName("a").ToArray()[i],x, BrowserContext), HowFound + " .links", BrowserContext));
		}

		public IBrowserContext BrowserContext { get; private set; }
		public RemoteWebElementWrapper Element { get; private set; }
		public string HowFound { get; private set; }
	}
}
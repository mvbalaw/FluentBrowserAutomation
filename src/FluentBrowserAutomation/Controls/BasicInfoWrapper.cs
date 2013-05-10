using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class BasicInfoWrapper : IHaveBasicInfo
	{
		public BasicInfoWrapper(IWebElement element, string howFound, IBrowserContext browserContext)
		{
			Element = element;
			HowFound = howFound;
			BrowserContext = browserContext;
		}

		public IBrowserContext BrowserContext { get; private set; }

		public IWebElement Element { get; private set; }
		public string Id
		{
			get
			{
				this.Exists().ShouldBeTrue();
				return Element.GetAttribute("id");
			}
		}
		
		public string Name
		{
			get
			{
				this.Exists().ShouldBeTrue();
				return Element.GetAttribute("name");
			}
		}
		
		public string HowFound { get; private set; }
	}
}
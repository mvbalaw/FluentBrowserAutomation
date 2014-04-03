using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class BasicInfoWrapper : IHaveBasicInfo
	{
		private string _id;
		private string _name;

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
				if (_id == null)
				{
					this.Exists().ShouldBeTrue();
					_id = Element.GetAttribute("id");
				}
				return _id;
			}
		}

		public string Name
		{
			get
			{
				if (_name == null)
				{
					this.Exists().ShouldBeTrue();
					_name = Element.GetAttribute("name");
				}
				return _name;
			}
		}

		public string HowFound { get; internal set; }
	}
}
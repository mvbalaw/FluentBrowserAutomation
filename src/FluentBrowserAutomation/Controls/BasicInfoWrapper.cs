namespace FluentBrowserAutomation.Controls
{
	public class BasicInfoWrapper : IHaveBasicInfo
	{
		private string _id;
		private string _name;

		public BasicInfoWrapper(RemoteWebElementWrapper element, string howFound, IBrowserContext browserContext)
		{
			Element = element;
			HowFound = howFound;
			BrowserContext = browserContext;
		}

		public IBrowserContext BrowserContext { get; private set; }

		public RemoteWebElementWrapper Element { get; private set; }

		public string Id
		{
			get
			{
				if (_id == null)
				{
					BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage: "wait for " + HowFound + " to exist in Id");
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
					BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage: "wait for " + HowFound + " to exist in Name");
					_name = Element.GetAttribute("name");
				}
				return _name;
			}
		}

		public string HowFound { get; internal set; }
	}
}
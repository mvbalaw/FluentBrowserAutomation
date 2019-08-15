using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class DivWrapper : BasicInfoWrapper, IAmVisualElement
	{
		public DivWrapper(RemoteWebElementWrapper div, string howFound, IBrowserContext browserContext)
			: base(div, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist in Text");

			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
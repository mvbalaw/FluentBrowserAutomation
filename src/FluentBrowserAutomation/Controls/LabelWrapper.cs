using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class LabelWrapper : BasicInfoWrapper, IAmVisualElement
	{
		public LabelWrapper(IWebElement label, string howFound, IBrowserContext browserContext)
			: base(label, howFound, browserContext)
		{
		}

		public string For
		{
			get { return Element.GetAttribute("for"); }
		}
	}
}
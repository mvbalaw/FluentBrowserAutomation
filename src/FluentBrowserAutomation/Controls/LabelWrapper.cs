using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class LabelWrapper : BasicInfoWrapper, IAmVisualElement
	{
		private string _for;

		public LabelWrapper(IWebElement label, string howFound, IBrowserContext browserContext)
			: base(label, howFound, browserContext)
		{
		}

		public string For
		{
			get { return _for ?? (_for = Element.GetAttribute("for")); }
		}
	}
}
using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium.Support.UI;

namespace FluentBrowserAutomation.Controls
{
	public class OptionWrapper : BasicInfoWrapper
	{
		public OptionWrapper(RemoteWebElementWrapper option, string howFound, DropDownListWrapper parentDropDown, IBrowserContext browserContext)
			: base(option, howFound, browserContext)
		{
			_parentDropDown = parentDropDown;
		}

		private readonly DropDownListWrapper _parentDropDown;

		public void Select()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			_parentDropDown.ScrollToIt();
			var select = new SelectElement(_parentDropDown.Element.RemoteWebElement);
			select.SelectByText(Text);
		}

		public ReadOnlyText Text
		{
			get { return new ReadOnlyText("text of " + HowFound, Element.Text); }
		}

		public ReadOnlyText Value
		{
			get { return new ReadOnlyText("value of " + HowFound, Element.GetAttribute("value")); }
		}
	}
}
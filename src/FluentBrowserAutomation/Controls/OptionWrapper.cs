using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FluentBrowserAutomation.Controls
{
	public class OptionWrapper : BasicInfoWrapper
	{
		public OptionWrapper(IWebElement option, string howFound, DropDownListWrapper parentDropDown, IBrowserContext browserContext)
			: base(option, howFound, browserContext)
		{
			_parentDropDown = parentDropDown;
		}

		private readonly DropDownListWrapper _parentDropDown;

		public void Select()
		{
			this.Exists().ShouldBeTrue();
			_parentDropDown.ScrollToIt();
			var select = new SelectElement(_parentDropDown.Element);
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
using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class CheckBoxWrapper : BasicInfoWrapper, ICanBeClicked, IAmInputThatCanBeChanged, IAmToggleableInput, INeedFocus
	{
		public CheckBoxWrapper(IWebElement checkBox, string howFound, IBrowserContext browserContext)
			: base(checkBox, howFound, browserContext)
		{
		}

		public void Toggle()
		{
			this.Click();
		}

		public BooleanState CheckedState()
		{
			this.Exists().ShouldBeTrue();
			return new BooleanState(HowFound + " should have been checked but was not",
				HowFound + " should not have been checked but was",
				() => Element.Selected, value =>
				{
					if (Element.Selected != value)
					{
						this.Click();
					}
				} );
		}
	}
}
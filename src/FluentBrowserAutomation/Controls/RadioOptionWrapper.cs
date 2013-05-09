using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class RadioOptionWrapper : BasicInfoWrapper, ICouldBeDisabled, ICanBeClicked, IAmInputThatCanBeChanged, IAmToggleableInput, INeedFocus
	{
		public RadioOptionWrapper(IWebElement radioButton, string howFound, IBrowserContext browserContext)
			: base(radioButton, howFound, browserContext)
		{
		}

		public void Toggle()
		{
			this.Click();
		}

		public BooleanState SelectedState()
		{
			this.Exists().ShouldBeTrue();
			return new BooleanState(HowFound + " should have been selected but was not",
				HowFound + " should not have been selected but was",
				() => Element.Selected, value => this.Click());
		}
	}
}
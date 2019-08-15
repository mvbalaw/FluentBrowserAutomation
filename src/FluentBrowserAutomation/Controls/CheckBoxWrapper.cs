using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Controls
{
	public class CheckBoxWrapper : BasicInfoWrapper, ICanBeClicked, IAmGenericInputThatCanBeChanged, INeedFocus
	{
		public CheckBoxWrapper(RemoteWebElementWrapper checkBox, string howFound, IBrowserContext browserContext)
			: base(checkBox, howFound, browserContext)
		{
		}

		public void CheckIt()
		{
			Toggle(true);
		}

		public BooleanState CheckedState()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage: "wait for " + HowFound + " to exist in CheckedState");
			return new BooleanState(HowFound + " should have been checked but was not",
				HowFound + " should not have been checked but was",
				() => Element.Selected, value =>
				{
					if (Element.Selected != value)
					{
						this.Click();
					}
				});
		}

		public bool HasTextValue(string expected)
		{
			BrowserContext.WaitUntil(x => this.IsVisible().IsTrue, errorMessage: "wait for " + HowFound + " to be visible in HasTextValue");
			return Element.ValueAttributeHasValue(expected);
		}

		public void ShouldBeChecked()
		{
			BrowserContext.WaitUntil(x => CheckedState().IsTrue, errorMessage: "wait for " + HowFound + " to be checked in ShouldBeChecked");
			CheckedState().ShouldBeTrue();
		}

		public void ShouldNotBeChecked()
		{
			BrowserContext.WaitUntil(x => CheckedState().IsFalse, errorMessage:"wait for " + HowFound + " to be unchecked in ShouldNotBeChecked");
			CheckedState().ShouldBeFalse();
		}

		private void Toggle(bool shouldBeSelected)
		{
			BrowserContext.WaitUntil(x =>
			{
				this.Click();
				return CheckedState().IsTrue == shouldBeSelected;
			}, errorMessage:"wait for " + HowFound + " to be " + (shouldBeSelected ? "checked" : "unchecked")+" in Toggle");
		}

		public void UncheckIt()
		{
			Toggle(false);
		}
	}
}
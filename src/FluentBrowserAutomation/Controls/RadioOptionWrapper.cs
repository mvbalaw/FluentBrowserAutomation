using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class RadioOptionWrapper : BasicInfoWrapper, ICouldBeDisabled, ICanBeClicked, IAmGenericInputThatCanBeChanged, INeedFocus
	{
		public RadioOptionWrapper(RemoteWebElementWrapper radioButton, string howFound, IBrowserContext browserContext)
			: base(radioButton, howFound, browserContext)
		{
		}

		public void SelectIt()
		{
			Toggle(true);
		}

		public BooleanState SelectedState()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist");
			return new BooleanState(HowFound + " should have been selected but was not",
				HowFound + " should not have been selected but was",
				() => Element.Selected, value => this.Click());
		}

		public void ShouldBeSelected()
		{
			BrowserContext.WaitUntil(x => SelectedState().IsTrue, errorMessage:"wait for " + HowFound + " to be selected");
			SelectedState().ShouldBeTrue();
		}

		public void ShouldNotBeSelected()
		{
			BrowserContext.WaitUntil(x => SelectedState().IsFalse, errorMessage:"wait for " + HowFound + " to be unselected");
			SelectedState().ShouldBeFalse();
		}

		private void Toggle(bool shouldBeSelected)
		{
			BrowserContext.WaitUntil(x =>
			{
				this.Click();
				return SelectedState().IsTrue == shouldBeSelected;
			}, errorMessage:"wait for " + HowFound + " to be " + (shouldBeSelected ? "selected" : "unselected"));
		}

		public void UnselectIt()
		{
			Toggle(false);
		}
	}
}
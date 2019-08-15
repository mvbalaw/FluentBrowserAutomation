using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class SpanWrapper : BasicInfoWrapper, IAmVisualElement, ICanBeClicked
	{
		public SpanWrapper(RemoteWebElementWrapper span, string howFound, IBrowserContext browserContext)
			: base(span, howFound, browserContext)
		{
		}

		public void ShouldContainText(string text)
		{
			BrowserContext.WaitUntil(x => Text().Contains(text).IsTrue, errorMessage:"wait for " + HowFound + " to contain '" + text + "' in ShouldContainText");
			Text().Contains(text).ShouldBeTrue();
		}

		public void ShouldNotContainText(string text)
		{
			BrowserContext.WaitUntil(x => Text().Contains(text).IsFalse, errorMessage:"wait for " + HowFound + " to NOT contain '" + text + "' in ShouldNotContainText");
			Text().Contains(text).ShouldBeFalse();
		}

		public ReadOnlyText Text()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist in Text");

			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
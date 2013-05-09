using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class SpanWrapper : BasicInfoWrapper, IAmVisualElement, ICanBeClicked
	{
		public SpanWrapper(IWebElement span, string howFound, IBrowserContext browserContext)
			: base(span, howFound, browserContext)
		{
		}

		public void ShouldContainText(string text)
		{
			Text().Contains(text).ShouldBeTrue();
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public interface INavigationControl : ICanBeClicked, IAmVisualElement, ICouldBeDisabled
	{
		string Text { get; }
	}
}
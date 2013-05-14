namespace FluentBrowserAutomation.Controls
{
	public interface INavigationControl : ICanBeClicked, IAmVisualElement, ICouldBeDisabled
	{
		string Text { get; }
	}
}
// ReSharper disable once CheckNamespace
namespace FluentBrowserAutomation
{
	public interface IAmSelectionInput
	{
		bool HasOption(string expected);
		void Select(string text);
	}
}
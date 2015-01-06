using FluentBrowserAutomation.Controls;

namespace FluentBrowserAutomation.Declarative
{
	public class GenericInputThatCanBeChangedWrapper : BasicInfoWrapper, IAmGenericInputThatCanBeChanged
	{
		public GenericInputThatCanBeChangedWrapper(RemoteWebElementWrapper control, string howFound, IBrowserContext browserContext)
			: base(control, howFound, browserContext)
		{
		}
	}
}
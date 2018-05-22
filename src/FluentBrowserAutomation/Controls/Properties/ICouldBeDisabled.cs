using FluentBrowserAutomation.Accessors;

//// ReSharper disable CheckNamespace
// ReSharper disable CheckNamespace
namespace FluentBrowserAutomation
// ReSharper restore CheckNamespace
//// ReSharper restore CheckNamespace
{
	public interface ICouldBeDisabled : IHaveBasicInfo
	{
	}

	public static class ICouldBeDisabledExtensions
	{
		public static IReadOnlyBooleanState IsEnabled(this ICouldBeDisabled element)
		{
			const string unexpectedlyFalse = "{0} is not enabled but should be.";
			const string unexpectedlyTrue = "{0} is enabled but should not be.";
			var unexpectedlyTrueMessage = string.Format(unexpectedlyTrue, element.HowFound);
			var unexpectedlyFalseMessage = string.Format(unexpectedlyFalse, element.HowFound);
			var result = new BooleanState(unexpectedlyFalseMessage,
				unexpectedlyTrueMessage,
				() => element.Element.Enabled);
			return result;
		}
	}
}
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Controls
{
	public interface IInputControl : IHaveBasicInfo
	{
	}

	public static class IFieldControlExtensions
	{
		public static CheckBoxWrapper AsCheckBox(this IInputControl element)
		{
			var elem = element.Element.IsCheckBox() ? element.Element : null;
			return new CheckBoxWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static DropDownListWrapper AsDropDownList(this IInputControl element)
		{
			var elem = element.Element.IsDropDownList() ? element.Element : null;
			return new DropDownListWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static RadioOptionWrapper AsRadioOption(this IInputControl element)
		{
			var elem = element.Element.IsRadioOption() ? element.Element : null;
			return new RadioOptionWrapper(elem, element.HowFound, element.BrowserContext);
		}

		public static TextBoxWrapper AsTextBox(this IInputControl element)
		{
			var elem = element.Element.IsTextBox() ? element.Element : null;
			return new TextBoxWrapper(elem, element.HowFound, element.BrowserContext);
		}
	}
}
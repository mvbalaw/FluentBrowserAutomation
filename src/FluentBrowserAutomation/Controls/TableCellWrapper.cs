using FluentBrowserAutomation.Accessors;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TableCellWrapper : BasicInfoWrapper
	{
		public TableCellWrapper(IWebElement tableCell, string howFound, IBrowserContext browserContext)
			: base(tableCell, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}

	public class TableHeaderCellWrapper : BasicInfoWrapper
	{
		public TableHeaderCellWrapper(IWebElement tableCell, string howFound, IBrowserContext browserContext)
			: base(tableCell, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
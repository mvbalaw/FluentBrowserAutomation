using FluentBrowserAutomation.Accessors;

namespace FluentBrowserAutomation.Controls
{
	public class TableCellWrapper : BasicInfoWrapper
	{
		public TableCellWrapper(RemoteWebElementWrapper tableCell, string howFound, IBrowserContext browserContext)
			: base(tableCell, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist in Text");
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}

	public class TableHeaderCellWrapper : BasicInfoWrapper
	{
		public TableHeaderCellWrapper(RemoteWebElementWrapper tableCell, string howFound, IBrowserContext browserContext)
			: base(tableCell, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage:"wait for " + HowFound + " to exist in Text");
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
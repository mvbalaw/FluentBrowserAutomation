using System;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TableRowWrapper : BasicInfoWrapper
	{
		public TableRowWrapper(IWebElement tableRow, string howFound, IBrowserContext browserContext)
			: base(tableRow, howFound, browserContext)
		{
		}

		public TableCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			var elements = Element.FindElements(By.TagName("td"));
			var cell = elements.Count > zeroBasedIndex ? elements[zeroBasedIndex] : null;
			var tableCellWrapper = new TableCellWrapper(cell, String.Format("{0}, table cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
			tableCellWrapper.Exists().ShouldBeTrue();
			return tableCellWrapper;
		}
	}

	public class TableHeaderRowWrapper : BasicInfoWrapper
	{
		public TableHeaderRowWrapper(IWebElement tableRow, string howFound, IBrowserContext browserContext)
			: base(tableRow, howFound, browserContext)
		{
		}

		public TableHeaderCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			var elements = Element.FindElements(By.TagName("th"));
			var cell = elements.Count > zeroBasedIndex ? elements[zeroBasedIndex] : null;
			return new TableHeaderCellWrapper(cell, String.Format("{0}, table header cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
		}
	}
}
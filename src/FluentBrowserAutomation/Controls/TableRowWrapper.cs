using System;
using System.Linq;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TableRowWrapper : BasicInfoWrapper, IAmVisualElement
	{
		public TableRowWrapper(IWebElement tableRow, string howFound, IBrowserContext browserContext)
			: base(tableRow, howFound, browserContext)
		{
		}

		public string[] CellValues()
		{
			var elements = Element.FindElements(By.TagName("td"));
			return elements.Select((cell, index) => new TableCellWrapper(cell, String.Format("{0}, table cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
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

		public string[] CellValues()
		{
			var elements = Element.FindElements(By.TagName("th"));
			return elements.Select((cell, index) => new TableHeaderCellWrapper(cell, String.Format("{0}, table header cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
		}

		public TableHeaderCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			var elements = Element.FindElements(By.TagName("th"));
			var cell = elements.Count > zeroBasedIndex ? elements[zeroBasedIndex] : null;
			return new TableHeaderCellWrapper(cell, String.Format("{0}, table header cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
		}
	}
}
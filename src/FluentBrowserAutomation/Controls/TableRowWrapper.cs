using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Extensions;

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
			var elements = GetCells();
			return elements.Select((cell, index) => new TableCellWrapper(cell, String.Format("{0}, table cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
		}

		public TableCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			var cell = GetCells()
				.Select((x, i) => new
				                  {
					                  Element = x,
					                  Index = i
				                  })
				.FirstOrDefault(x => x.Index == zeroBasedIndex);
			var tableCellWrapper = new TableCellWrapper(cell == null ? null : cell.Element, String.Format("{0}, table cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
			tableCellWrapper.Exists().ShouldBeTrue();
			return tableCellWrapper;
		}

		private IEnumerable<IWebElement> GetCells()
		{
			return Element.GetChildElementsByTagName("td");
		}
        
        public void Click()
		{
			Element.Click();
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
			var elements = GetCells();
			return elements.Select((cell, index) => new TableHeaderCellWrapper(cell, String.Format("{0}, table header cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
		}

		public TableHeaderCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			var cell = GetCells()
				.Select((x, i) => new
				                  {
					                  Element = x,
					                  Index = i
				                  })
				.FirstOrDefault(x => x.Index == zeroBasedIndex);
			return new TableHeaderCellWrapper(cell == null ? null : cell.Element, String.Format("{0}, table header cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
		}

		private IEnumerable<IWebElement> GetCells()
		{
			return Element.GetChildElementsByTagName("th");
		}

        public void Click()
        {
            Element.Click();
        }
	}
}
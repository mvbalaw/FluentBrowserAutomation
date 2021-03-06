using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Extensions;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TableRowWrapper : BasicInfoWrapper, IAmVisualElement, ICanBeClicked
	{
		public TableRowWrapper(RemoteWebElementWrapper tableRow, string howFound, IBrowserContext browserContext)
			: base(tableRow, howFound, browserContext)
		{
		}

		public string[] CellValues()
		{
			var elements = GetCells();
			return elements.Select((cell, index) => new TableCellWrapper(new RemoteWebElementWrapper(()=>GetCells().ToArray()[index], cell, BrowserContext), string.Format("{0}, table cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
		}

		public TableCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			Func<IWebElement> f = ()=>
			{
				var cell = GetCells()
					.Select((x, i) => new
					                  {
						                  Element = x,
						                  Index = i
					                  })
					.FirstOrDefault(x => x.Index == zeroBasedIndex);
			return cell == null ? null : cell.Element;
			};
			var tableCellWrapper = new TableCellWrapper(new RemoteWebElementWrapper(f, f(), BrowserContext), string.Format("{0}, table cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
			tableCellWrapper.BrowserContext.WaitUntil(x => tableCellWrapper.Exists().IsTrue, errorMessage: "wait for " + tableCellWrapper.HowFound + " to exist in CellWithIndex");
			return tableCellWrapper;
		}

		public TableCellWrapper CellWithClassName(string className)
		{
			Func<IWebElement> f = ()=>
			{
				var cell = GetCells()
					.FirstOrDefault(x =>
					{
						var @class = x.GetAttribute("class") ?? "";
						return @class.Split(' ').Contains(className);
					});
				return cell;
			};
			var tableCellWrapper = new TableCellWrapper(new RemoteWebElementWrapper(f, f(), BrowserContext), string.Format("{0}, table cell with class name {1}", HowFound, className), BrowserContext);
			return tableCellWrapper;
		}

		private IEnumerable<IWebElement> GetCells()
		{
			return Element.GetChildElementsByTagName("td");
		}
        
        public void Click()
		{
			ICanBeClickedExtensions.Click(this);
		}
	}

	public class TableHeaderRowWrapper : BasicInfoWrapper, ICanBeClicked
	{
		public TableHeaderRowWrapper(RemoteWebElementWrapper tableRow, string howFound, IBrowserContext browserContext)
			: base(tableRow, howFound, browserContext)
		{
		}

		public string[] CellValues()
		{
			var elements = GetCells();
			return elements.Select((cell, index) => new TableHeaderCellWrapper(new RemoteWebElementWrapper(()=>GetCells().ToArray()[index],cell, BrowserContext), string.Format("{0}, table header cell with index {1}", HowFound, index), BrowserContext))
				.Select(x => (string)x.Text())
				.ToArray();
		}

		public TableHeaderCellWrapper CellWithIndex(int zeroBasedIndex)
		{
			Func<IWebElement> f = () =>
			{
				var cell = GetCells()
					.Select((x, i) => new
					                  {
						                  Element = x,
						                  Index = i
					                  })
					.FirstOrDefault(x => x.Index == zeroBasedIndex);
				return cell == null ? null : cell.Element;
			};
			return new TableHeaderCellWrapper(new RemoteWebElementWrapper(f,f(), BrowserContext), string.Format("{0}, table header cell with index {1}", HowFound, zeroBasedIndex), BrowserContext);
		}

		public TableCellWrapper CellWithClassName(string className)
		{
			Func<IWebElement> f = ()=>
			{
				var cell = GetCells()
					.FirstOrDefault(x =>
					{
						var @class = x.GetAttribute("class") ?? "";
						return @class.Split(' ').Contains(className);
					});
				return cell;
			};
			var tableCellWrapper = new TableCellWrapper(new RemoteWebElementWrapper(f, f(), BrowserContext), string.Format("{0}, table header cell with class name {1}", HowFound, className), BrowserContext);
			return tableCellWrapper;
		}

		private IEnumerable<IWebElement> GetCells()
		{
			return Element.GetChildElementsByTagName("th");
		}

        public void Click()
        {
			ICanBeClickedExtensions.Click(this);
        }
	}
}
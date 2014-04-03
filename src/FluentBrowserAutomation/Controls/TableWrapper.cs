using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Extensions;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class TableWrapper : BasicInfoWrapper
	{
		public TableWrapper(IWebElement table, string howFound, IBrowserContext browserContext)
			: base(table, howFound, browserContext)
		{
		}

		private static IEnumerable<IWebElement> GetBodyRows(IEnumerable<IWebElement> allRows)
		{
			return allRows.Where(x => !IsHeaderRow(x) && !IsFooterRow(x));
		}

		private IEnumerable<IWebElement> GetRows()
		{
			return Element.GetChildElementsByTagName("tr");
		}

		public IEnumerable<TableHeaderRowWrapper> Headers()
		{
			var allRows = GetRows();
			var headers = allRows.Where(IsHeaderRow)
				.Select((x, i) =>
					new TableHeaderRowWrapper(x, String.Format("{0}, header row with index {1}", HowFound, i), BrowserContext));
			return headers;
		}

		private static bool IsFooterRow(IWebElement row)
		{
			var footer = row.GetParent().TagName.Equals("tfoot");
			return footer;
		}

		private static bool IsHeaderRow(IWebElement row)
		{
			var theadOrInline = row.GetParent().TagName.Equals("thead") ||
				row.GetChildElementsByTagName("th").Any();
			return theadOrInline;
		}

		public IEnumerable<TableRowWrapper> Rows()
		{
			var allRows = GetRows();
			var rows = GetBodyRows(allRows)
				.Select((x, i) =>
					new TableRowWrapper(x, String.Format("{0}, row with index {1}", HowFound, i), BrowserContext));

			return rows;
		}
	}
}
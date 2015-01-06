using System;
using System.Collections.Generic;
using System.Linq;
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Controls
{
    public class TableWrapper : BasicInfoWrapper
    {
		public TableWrapper(RemoteWebElementWrapper table, string howFound, IBrowserContext browserContext)
            : base(table, howFound, browserContext)
        {
        }

		private static IEnumerable<RemoteWebElementWrapper> GetBodyRows(IEnumerable<RemoteWebElementWrapper> allRows)
        {
            return allRows.Where(x => !IsHeaderRow(x) && !IsFooterRow(x));
        }

        private IEnumerable<RemoteWebElementWrapper> GetRows()
        {
            return Element.GetChildElementsByTagName("tr").Select(x=>new RemoteWebElementWrapper(null,x));
        }

        public IEnumerable<TableHeaderRowWrapper> Headers()
        {
            var allRows = GetRows();
            var headers = allRows.Where(IsHeaderRow)
                .Select((x, i) =>
                    new TableHeaderRowWrapper(x, String.Format("{0}, header row with index {1}", HowFound, i),
                        BrowserContext));
            return headers;
        }

        private static bool IsFooterRow(RemoteWebElementWrapper row)
        {
            var footer = row.GetParent().TagName.Equals("tfoot");
            return footer;
        }

        private static bool IsHeaderRow(RemoteWebElementWrapper row)
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

        public TableRowWrapper RowWithId(string tableRowId)
        {
            var row = GetRows().FirstOrDefault(x => x.GetAttribute("id") == tableRowId);
			return new TableRowWrapper(row, String.Format("{0}, row with id {1}", HowFound, tableRowId), BrowserContext);
        }
    }
}
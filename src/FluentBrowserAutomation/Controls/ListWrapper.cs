using System;
using System.Collections.Generic;
using System.Linq;
using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Extensions;

using OpenQA.Selenium;

namespace FluentBrowserAutomation.Controls
{
	public class ListWrapper : BasicInfoWrapper
	{
		public ListWrapper(IWebElement list, string howFound, IBrowserContext browserContext)
			: base(list, howFound, browserContext)
		{
		}

		public IEnumerable<ListItemWrapper> Rows()
		{
			var items = Element.GetChildElementsByTagName("li")
				.Select((x, i) =>
				        new ListItemWrapper(x, String.Format("{0}, item with index {1}", HowFound, i), BrowserContext));

			return items;
		}
	}

	public class ListItemWrapper : BasicInfoWrapper
	{
		public ListItemWrapper(IWebElement listItem, string howFound, IBrowserContext browserContext)
			: base(listItem, howFound, browserContext)
		{
		}

		public ReadOnlyText Text()
		{
			return new ReadOnlyText(HowFound, Element.Text);
		}
	}
}
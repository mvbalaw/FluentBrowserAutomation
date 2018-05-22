using System;
using System.Collections.Generic;
using System.Linq;

using FluentBrowserAutomation.Accessors;
using FluentBrowserAutomation.Extensions;

namespace FluentBrowserAutomation.Controls
{
    public class ListWrapper : BasicInfoWrapper
    {
		public ListWrapper(RemoteWebElementWrapper list, string howFound, IBrowserContext browserContext)
            : base(list, howFound, browserContext)
        {
        }

        public IEnumerable<ListItemWrapper> Rows()
        {
            var items = Element.GetChildElementsByTagName("li")
                .Select((x, i) =>
					new ListItemWrapper(new RemoteWebElementWrapper(() => Element.GetChildElementsByTagName("li").ToArray()[i],x, BrowserContext.Browser), String.Format("{0}, item with index {1}", HowFound, i), BrowserContext));

            return items;
        }

        public ListItemWrapper RowWithId(string listItemId)
        {
            var item = BrowserContext.TryGetElementById(listItemId);

            return new ListItemWrapper(item, String.Format("{0}, item with id {1}", HowFound, listItemId), BrowserContext);
        }
    }

    public class ListItemWrapper : BasicInfoWrapper, ICanBeClicked
    {
		public ListItemWrapper(RemoteWebElementWrapper listItem, string howFound, IBrowserContext browserContext)
            : base(listItem, howFound, browserContext)
        {
        }

        public ReadOnlyText Text()
        {
			BrowserContext.WaitUntil(x => this.Exists().IsTrue, errorMessage: "wait for " + HowFound + " to exist");
            return new ReadOnlyText(HowFound, Element.Text);
        }
        
        public void Click()
        {
			ICanBeClickedExtensions.Click(this);
        }
    }
}
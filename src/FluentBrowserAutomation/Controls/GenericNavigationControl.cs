namespace FluentBrowserAutomation.Controls
{
	public class GenericNavigationControl : BasicInfoWrapper, INavigationControl
	{
		public GenericNavigationControl(RemoteWebElementWrapper element, string howFound, IBrowserContext browserContext)
			: base(element, howFound, browserContext)
		{
		}

		public string Text
		{
			get { return this.IsLink() ? this.AsLink().Text : this.AsButton().Text; }
		}
	}
}
namespace FluentBrowserAutomation.Controls
{
	public class ContainerWrapper : BasicInfoWrapper
	{
		public ContainerWrapper(RemoteWebElementWrapper div, string howFound, IBrowserContext browserContext)
			: base(div, howFound, browserContext)
		{
		}
	}
}
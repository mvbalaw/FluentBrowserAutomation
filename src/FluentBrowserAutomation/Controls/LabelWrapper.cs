namespace FluentBrowserAutomation.Controls
{
	public class LabelWrapper : BasicInfoWrapper, IAmVisualElement
	{
		public LabelWrapper(RemoteWebElementWrapper label, string howFound, IBrowserContext browserContext)
			: base(label, howFound, browserContext)
		{
		}

		private string _for;

		public string For
		{
			get { return _for ?? (_for = Element.GetAttribute("for")); }
		}
	}
}
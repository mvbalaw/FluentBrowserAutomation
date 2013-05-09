A fluent DSL around Web Driver.

## Examples

	[Test]
	public void Should_verify_the_expiration_after_setting_the_name()
	{
		var With.Browser<ChromeDriver>(BaseUrl, false)
			.UiState(
				x => x.InputWithLabel("Name:").SetTo("Jones"),
				x => x.WaitUntil(y => y.InputWithId("selectAll").IsVisible().IsTrue),
				x => x.InputWithId("expiration").ShouldBeEqualTo("5/2/2013")
			);
	}

## Licenses

[MIT License][mitlicense]
[mitlicense]: http://www.opensource.org/licenses/mit-license.php

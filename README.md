FluentBrowserAutomation ReadMe
===
### Description

FluentBrowserAutomation is a fluent domain-specific language (DSL) around Web Driver.

### Examples

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

### How To Build:

The build script requires Ruby with rake installed.

1. Run `InstallGems.bat` to get the ruby dependencies (only needs to be run once per computer)
1. open a command prompt to the root folder and type `rake` to execute rakefile.rb

If you do not have ruby:

1. You need to create a src\CommonAssemblyInfo.cs file. Go.bat will copy src\CommonAssemblyInfo.cs.default to src\CommonAssemblyInfo.cs
1. open src\FluentBrowserAutomation.sln with Visual Studio and build the solution

### License

[MIT License][mitlicense]

This project is part of [MVBA's Open Source Projects][MvbaLawGithub].

If you have questions or comments about this project, please contact us at <mailto:opensource@mvbalaw.com>

[MvbaLawGithub]: http://mvbalaw.github.io/
[mitlicense]: http://www.opensource.org/licenses/mit-license.php

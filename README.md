EPPlus.Core
===========
`EPPlus.Core` is an **unofficial** port of the [EPPlus library](http://epplus.codeplex.com) to .NET Core.
It's based on the [5/24/2017, change set#fcded570d92e](http://epplus.codeplex.com/SourceControl/list/changesets).

Install via NuGet
-----------------
To install EPPlus.Core, run the following command in the Package Manager Console:

```
PM> Install-Package EPPlus.Core
```

You can also view the [package page](http://www.nuget.org/packages/EPPlus.Core/) on NuGet.


Usage
------
- [Functional Tests](/src/EPPlus.Core.FunctionalTests)
- [A sample ASP.NET Core App](/src/EPPlus.Core.SampleWebApp)


Notes:
-----------------
- To run this project on Linux, you will need to install `libgdiplus` too:
```
sudo apt-get update
sudo apt-get install libgdiplus
```

- The original EPPlus project has been [moved to Github](https://github.com/JanKallman/EPPlus/). Please post its development related issues/pull requests at there. The main purpose of the current project is just providing an existing EPPlus for .NET Standard. There will be no further developments here.


News:
-----------------
[EPPlus 4.5.0-beta Added .NET Core Support](https://github.com/VahidN/EPPlus.Core/issues/37)
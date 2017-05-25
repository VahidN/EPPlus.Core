EPPlus.Core
===========
`EPPlus.Core` is an **unofficial** port of the [EPPlus library](http://epplus.codeplex.com) to .NET Core. 
It's based on the [4/8/2017, change set#9818e8625288](http://epplus.codeplex.com/SourceControl/list/changesets).


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


Note:
-----------------
To run this project on Linux, you will need to install `libgdiplus` too:
```
sudo apt-get update
sudo apt-get install libgdiplus
```

# FritzBoxSoap
Uses SOAP Api of fritzbox devices to get information about dsl traffic and more

Also includes a small Tray-Icon indicator, to quickly view the current global network activity of your dsl connection.

![](https://raw.githubusercontent.com/F1ndus/FritzBoxSoap/master/dslauslastung.PNG)

Short Memo on how to build the Deskband:

* Delete possibly conflicting old assemblies from: %windir%\Microsoft.NET\assembly
* Install Dependencies with Nuget
	* Install-Package Microsoft.Xaml.Behaviors.Wpf
* Check if all Projects include the Certificate so that Strong Named Assemblies are getting created
* Build the Project for AnyCPU/Release
* Use the reload.bat Script to register the Deskband (make sure to use the vscode developer console)

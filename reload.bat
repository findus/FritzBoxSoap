
gacutil -u FritzboxDeskband\bin\ReleaseCSDeskBand.dll
gacutil -u FritzboxDeskband\bin\FritzBoxSoap.dll
gacutil -u FritzboxDeskband\bin\FritzboxDeskband.dll
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /unregister FritzboxDeskband.dll

gacutil -i FritzboxDeskband\bin\CSDeskBand.dll
gacutil -i FritzboxDeskband\bin\FritzBoxSoap.dll
gacutil -i FritzboxDeskband\bin\FritzboxDeskband.dll
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /codebase FritzboxDeskband.dll

taskkill /im explorer.exe /f
start explorer.exe
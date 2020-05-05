@echo off
SETLOCAL
call "%vs90comntools%\vsvars32.bat"
set SOLUTIONDIR=%~f1
set BUILDNUMBER=%2
set OUTPUTDIR=%~f3

pushd %solutiondir%

echo Updating assembly versions
for %%a in (Properties\AssemblyInfo.cs FFTacText\Properties\AssemblyInfo.cs FFTorgASM\Properties\AssemblyInfo.cs PatcherLib\Properties\AssemblyInfo.cs PatcherLib.Resources\Properties\AssemblyInfo.cs ShishiSpriteEditor\Properties\AssemblyInfo.cs) do ..\utils\updateversion.exe --inputfile %%a --outputfile %%a --version "Assembly" --pin 1.0.0.%BUILDNUMBER%
for %%a in (Properties\AssemblyInfo.cs FFTacText\Properties\AssemblyInfo.cs FFTorgASM\Properties\AssemblyInfo.cs PatcherLib\Properties\AssemblyInfo.cs PatcherLib.Resources\Properties\AssemblyInfo.cs ShishiSpriteEditor\Properties\AssemblyInfo.cs) do ..\utils\updateversion.exe --inputfile %%a --outputfile %%a --version "File" --pin 1.0.0.%BUILDNUMBER%
copy /y TextCompression\FFTTextCompression.rc TextCompression\FFTTextCompression.rc.old > nul
..\utils\sed -r "s/[0-9]+([,\.])[0-9]+([,\.])[[0-9]+([,\.])[[0-9]+/1\10\20\3%BUILDNUMBER%/" TextCompression\FFTTextCompression.rc.old > TextCompression\FFTTextCompression.rc
del TextCompression\FFTTextCompression.rc.old

rem echo Building FFTTextCompression.dll
rem cd %solutiondir%
rem cd TextCompression
rem vcbuild /clean TextCompression.vcproj Release > nul
rem vcbuild TextCompression.vcproj Release > nul
rem copy /y %solutiondir%\TextCompression\Release\FFTTextCompression.dll %solutiondir%\FFTacText\Resources

rem build solution
cd ..
echo Cleaning solution
devenv /clean release FFTPatcher.sln > nul
echo Building solution
devenv /build release FFTPatcher.sln > nul

echo Copying FFTPatcher.exe
copy /y bin\x86\Release\FFTPatcher.exe %OUTPUTDIR% > nul
echo Copying FFTactext.exe
copy /y FFTacText\bin\x86\Release\FFTactext.exe %OUTPUTDIR% > nul
echo Copying FFTorgASM.exe
copy /y FFTorgASM\bin\x86\Release\FFTorgASM.exe %OUTPUTDIR% > nul
echo Copying Example.xml
copy /y FFTorgASM\bin\x86\Release\Example.xml %OUTPUTDIR% > nul
echo Copying Razele.xml
copy /y FFTorgASM\bin\x86\Release\Razele.xml %OUTPUTDIR% > nul
echo Copying Xifanie.xml
copy /y FFTorgASM\bin\x86\Release\Xifanie.xml %OUTPUTDIR% > nul
echo Copying nates1984.xml
copy /y FFTorgASM\bin\x86\Release\nates1984.xml %OUTPUTDIR% > nul
echo Copying PatcherLib.dll
copy /y PatcherLib\bin\x86\Release\PatcherLib.dll %OUTPUTDIR% > nul
echo Copying PatcherLib.Resources.dll
copy /y PatcherLib.Resources\bin\x86\Release\PatcherLib.Resources.dll %OUTPUTDIR% > nul
echo Copying ShishiSpriteEditor.exe
copy /y ShishiSpriteEditor\bin\x86\Release\ShishiSpriteEditor.exe %OUTPUTDIR% > nul
echo Copying ICSharpCode.SharpZipLib.dll
copy /y ..\utils\ICSharpCode.SharpZipLib.dll %OUTPUTDIR% > nul
rem echo Copying FFTTextCompression.dll
rem copy /y TextCompression\Release\FFTTextCompression.dll %OUTPUTDIR% > nul

echo Copying COPYING
copy /y COPYING %outputdir% > nul
echo Copying changelog.txt
copy /y changelog.txt  %OUTPUTDIR% > nul
echo Copying resources.txt
copy /y resources.txt  %OUTPUTDIR% > nul
echo Copying readme.txt
copy /y readme.txt  %OUTPUTDIR% > nul
echo Copying sprites.txt
copy /y sprites.txt  %OUTPUTDIR% > nul
echo Copying text.txt
copy /y text.txt %OUTPUTDIR% > nul

popd
:EOF
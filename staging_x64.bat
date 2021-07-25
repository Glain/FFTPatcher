"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.com" FFTPatcher.sln /rebuild "Release|x64"
rmdir /s /q staging
mkdir staging
xcopy /s /Y bin\x64\Release staging
xcopy /s /Y FFTacText\bin\x64\Release\*.* staging
xcopy /s /Y FFTorgASM\bin\x64\Release\*.* staging
xcopy /s /Y ShishiSpriteEditor\bin\x64\Release\*.* staging
xcopy /s /Y MassHexASM\bin\x64\Release\*.* staging
xcopy /s /Y EntryEdit\bin\x64\Release\*.* staging
del staging\*.pdb
del staging\*.vshost.exe
del staging\*.vshost.exe.config
del staging\*.manifest

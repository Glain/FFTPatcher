rmdir /s /q staging
mkdir staging
xcopy /s /Y bin\x86\Release staging
xcopy /s /Y FFTacText\bin\x86\Release\*.* staging
xcopy /s /Y FFTorgASM\bin\x86\Release\*.* staging
xcopy /s /Y ShishiSpriteEditor\bin\x86\Release\*.* staging
del staging\*.pdb
del staging\*.vshost.exe
del staging\*.vshost.exe.config
del staging\*.manifest

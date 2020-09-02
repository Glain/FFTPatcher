rmdir /s /q staging
mkdir staging
xcopy /s /Y bin\Release staging
xcopy /s /Y FFTacText\bin\Release\*.* staging
xcopy /s /Y FFTorgASM\bin\Release\*.* staging
xcopy /s /Y ShishiSpriteEditor\bin\Release\*.* staging
del staging\*.pdb
del staging\*.vshost.exe
del staging\*.vshost.exe.config
del staging\*.manifest

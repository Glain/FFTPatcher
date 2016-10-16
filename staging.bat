del staging\*.*
xcopy /s bin\x86\Release staging
xcopy /s FFTacText\bin\x86\Release\*.* staging
xcopy /s FFTorgASM\bin\x86\Release\*.* staging
xcopy /s ShishiSpriteEditor\bin\x86\Release\*.* staging
del staging\*.pdb
del staging\*.vshost.exe
del staging\*.vshost.exe.config
del staging\*.manifest
pause

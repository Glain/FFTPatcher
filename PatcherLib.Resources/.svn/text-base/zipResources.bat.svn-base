rem @for %%i in (%1\Resources\PSP\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSP\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSP\Abilities\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSP\Abilities\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSP\Items\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSP\Items\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSP\bin\*.bin) do %1\gzip -c "%%i" > %1\Resources\PSP\bin\%%~ni.bin.gz

rem @for %%i in (%1\Resources\PSX-US\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSX-US\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSX-US\Abilities\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSX-US\Abilities\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSX-US\Items\*.xml) do %1\gzip -c "%%i" > %1\Resources\PSX-US\Items\%%~ni.xml.gz
rem @for %%i in (%1\Resources\PSX-US\bin\*.bin) do %1\gzip -c "%%i" > %1\Resources\PSX-US\bin\%%~ni.bin.gz
rem @for %%i in (%1\Resources\*.ENT) do %1\gzip -c "%%i" > %1\Resources\%%~ni.ENT.gz
rem @for %%i in (%1\Resources\*.xml) do %1\gzip -c "%%i" > %1\Resources\%%~ni.xml.gz

rem @for %%i in (%1\FFTacText\Resources\*.xml) do %1\..\gzip -c "%%i" > %1\FFTacText\Resources\%%~ni.xml.gz

rem @for %%i in (%1\FFTacText\Resources\PSP\*.LZW) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSP\%%~ni.lzw.gz
rem @for %%i in (%1\FFTacText\Resources\PSP\*.BIN) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSP\%%~ni.bin.gz
rem @for %%i in (%1\FFTacText\Resources\PSP\*.ffttext) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSP\%%~ni.ffttext.gz
rem 
rem @for %%i in (%1\FFTacText\Resources\PSX\*.LZW) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSX\%%~ni.lzw.gz
rem @for %%i in (%1\FFTacText\Resources\PSX\*.BIN) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSX\%%~ni.bin.gz
rem @for %%i in (%1\FFTacText\Resources\PSX\*.partial) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSX\%%~ni.partial.gz
rem @for %%i in (%1\FFTacText\Resources\PSX\*.out) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSX\%%~ni.out.gz
rem @for %%i in (%1\FFTacText\Resources\PSX\*.ffttext) do %1\gzip -c "%%i" > %1\FFTacText\Resources\PSX\%%~ni.ffttext.gz

cd %1\Resources
del Resources.tar
del Resources.tar.gz
rem del Resources.7z
rem %1\..\7z a -r -mx9 -x!_svn -x!.svn Resources.7z *
..\..\tar -c -v -f Resources.tar --exclude *[._]svn --exclude *.xls *
%1\..\gzip -9 Resources.tar
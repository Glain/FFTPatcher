cd %1\Resources
del Resources.tar
del Resources.tar.gz
rem del Resources.7z
rem %1\..\7z a -r -mx9 -x!_svn -x!.svn Resources.7z *
..\..\tar -c -v -f Resources.tar --exclude *[._]svn *
%1\..\gzip -9 Resources.tar
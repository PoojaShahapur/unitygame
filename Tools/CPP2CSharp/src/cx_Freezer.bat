set base_dir=%~dp0
%base_dir:~0,2%

pushd %base_dir%
F:\Python34\python.exe cx_Freezer.py build
popd

pause
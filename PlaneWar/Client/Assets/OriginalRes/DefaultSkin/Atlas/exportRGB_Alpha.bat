set BaseDir=%~dp0
set Disk=%BaseDir:~0,2%

%Disk%
cd %BaseDir%

set path=%path%;D:\ProgramFiles\ImageMagick-7.0.4-Q16

set FileName="Avatar"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

set FileName="GameOption"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

set FileName="SettingSkin"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

set FileName="ShopSkin"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

set FileName="SignSkin"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

set FileName="StartGame"
magick.exe convert -alpha remove %FileName%.png %FileName%_RGB.png
magick.exe convert -alpha Extract %FileName%.png %FileName%_Alpha.png

pause
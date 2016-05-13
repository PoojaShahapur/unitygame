rem UNITY程序的路径
set UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
 
rem 游戏程序路径
set PROJECT_PATH=/Users/MOMO/commond

%UNITY_PATH% -projectPath %PROJECT_PATH% -executeMethod ProjectBuild.BuildForAndroid project-%1 -quit
rem UNITY�����·��
set UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
 
rem ��Ϸ����·��
set PROJECT_PATH=/Users/MOMO/commond

%UNITY_PATH% -projectPath %PROJECT_PATH% -executeMethod ProjectBuild.BuildForAndroid project-%1 -quit
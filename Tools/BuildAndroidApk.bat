rem UNITY程序的路径
set UNITY_PATH=D:\ProgramFiles\Unity\Editor\Unity.exe
 
rem 游戏程序路径
set PROJECT_PATH=E:\Self\Self\unity\unitygame\Client_Start

%UNITY_PATH% -quit -batchmode -nographics -logFile E:\Self\Self\unity\unitygame\Client_Start\BuildOut\EditorLog.txt -projectPath %PROJECT_PATH% -executeMethod EditorTool.CmdSys.cmdMain nihao=ceshi
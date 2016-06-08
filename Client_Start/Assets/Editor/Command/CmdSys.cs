using SDK.Lib;
using System;

namespace EditorTool
{
    /**
     * @brief 命令行
     */
    public class CmdSys
    {
        protected ProgramOptions mProgramOptions;

        /**
         * @brief 命令行参数入口
         */
        
        static public void cmdMain()
        {
            CmdSys cmdSys = new CmdSys();
            cmdSys.start();
        }

        public CmdSys()
        {
            mProgramOptions = new ProgramOptions();
        }

        public void start()
        {
            mProgramOptions.parseCmdline();

            string logPath = "E:/Self/Self/unity/unitygame/Client_Start/BuildOut/aaa/bbb";
            if (!UtilPath.existDirectory(logPath))
            {
                UtilPath.createDirectory(logPath);
            }
        }

        public void testLog()
        {
            string logPath = "E:/Self/Self/unity/unitygame/Client_Start/BuildOut";
            if (!UtilPath.existDirectory(logPath))
            {
                UtilPath.createDirectory(logPath);
            }

            string logFileName = "E:/Self/Self/unity/unitygame/Client_Start/BuildOut/Log.txt";
            if (UtilPath.existFile(logFileName))
            {
                UtilPath.deleteFile(logFileName);
            }

            MDataStream logFile = new MDataStream(logFileName);
            logFile.writeLine(Environment.CommandLine);
            string[] cmdArr = Environment.GetCommandLineArgs();
            foreach (string cmd in cmdArr)
            {
                logFile.writeLine(cmd);
            }

            logFile.writeLine(Environment.CurrentDirectory);
            logFile.dispose();
        }
    }
}
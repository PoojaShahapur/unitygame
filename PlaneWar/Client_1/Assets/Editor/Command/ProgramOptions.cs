using SDK.Lib;
using System;
using System.Collections.Generic;

namespace EditorTool
{
    /**
     * @brief 命令行选项，不同选项使用空格，同一个选项使用 KV 形式
     */
    public class ProgramOptions
    {
        protected Dictionary<string, string> mKVDic;

        public void parseCmdline()
        {
            //string cmdLine = Environment.CommandLine;
            string[] paramArr = Environment.GetCommandLineArgs();
            string[] splitArr = null;
            foreach (string param in paramArr)
            {
                if(param.IndexOf(UtilApi.SEPARATOR) != -1)
                {
                    splitArr = param.Split('=');
                    mKVDic[splitArr[0]] = splitArr[1];
                }
            }
        }
    }
}
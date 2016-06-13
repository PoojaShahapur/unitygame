using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class FileVerBase
    {
        protected void loadFormText(string text, Dictionary<string, FileVerInfo> dic)
        {
            string[] lineSplitStr = { UtilApi.CR_LF };
            string[] equalSplitStr = { UtilApi.SEPARATOR };
            string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            string[] equalList = null;
            FileVerInfo fileInfo;
            while (lineIdx < lineList.Length)
            {
                equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                fileInfo = new FileVerInfo();

                fileInfo.mOrigPath = equalList[0];
                fileInfo.mResUniqueId = equalList[1];
                fileInfo.mLoadPath = equalList[2];
                fileInfo.m_fileMd5 = equalList[3];
                fileInfo.m_fileSize = Int32.Parse(equalList[4]);

                //dic[fileInfo.mResUniqueId] = fileInfo;
                dic[fileInfo.mOrigPath] = fileInfo;
                ++lineIdx;
            }
        }
    }
}
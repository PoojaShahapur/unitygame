using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class FileVerBase
    {
        public string mCurVer;
        public FileVerInfo mFileVerInfo;     // 这个主要是记录文件版本的版本

        public bool mIsMiniLoadSuccess;
        public bool mIsVerLoadSuccess;

        public AddOnceAndCallOnceEventDispatch mMiniLoadedDisp;
        public AddOnceAndCallOnceEventDispatch mLoadedDisp;

        public FileVerBase()
        {
            mIsMiniLoadSuccess = false;
            mIsVerLoadSuccess = false;

            mCurVer = "";           // 当前版本，当前日期，例如 201606091136
            mFileVerInfo = new FileVerInfo();

            mMiniLoadedDisp = new AddOnceAndCallOnceEventDispatch();
            mLoadedDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public void parseMiniFile(string text)
        {
            string[] lineSplitStr = { UtilApi.CR_LF };
            string[] equalSplitStr = { UtilApi.SEPARATOR };
            string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            string[] equalList = null;

            // 第一行是版本号
            lineIdx = 0;

            UtilStr.removeLastCR(ref lineList[lineIdx]);
            equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
            mCurVer = equalList[1];

            // 第二行是版本的版本
            lineIdx = 1;
            UtilStr.removeLastCR(ref lineList[lineIdx]);

            equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);

            mFileVerInfo.mOrigPath = equalList[0];
            mFileVerInfo.mResUniqueId = equalList[1];
            mFileVerInfo.mLoadPath = equalList[2];
            mFileVerInfo.mFileMd5 = equalList[3];
            mFileVerInfo.mFileSize = Int32.Parse(equalList[4]);
        }

        // 这个主要是解析版本文件的
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
                UtilStr.removeLastCR(ref lineList[lineIdx]);
                equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                fileInfo = new FileVerInfo();

                fileInfo.mOrigPath = equalList[0];
                fileInfo.mResUniqueId = equalList[1];
                fileInfo.mLoadPath = equalList[2];
                fileInfo.mFileMd5 = equalList[3];
                fileInfo.mFileSize = Int32.Parse(equalList[4]);

                //dic[fileInfo.mResUniqueId] = fileInfo;
                dic[fileInfo.mOrigPath] = fileInfo;
                ++lineIdx;
            }
        }
    }
}
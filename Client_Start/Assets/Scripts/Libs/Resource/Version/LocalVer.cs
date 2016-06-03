﻿using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 本地版本系统
     */
    public class LocalVer
    {
        public Dictionary<string, FileVerInfo> m_path2Ver_R_Dic;
        public Dictionary<string, FileVerInfo> m_path2Ver_S_Dic;
        public Dictionary<string, FileVerInfo> m_path2Ver_P_Dic;

        public LocalVer()
        {
            m_path2Ver_R_Dic = new Dictionary<string, FileVerInfo>();
            m_path2Ver_S_Dic = new Dictionary<string, FileVerInfo>();
            m_path2Ver_P_Dic = new Dictionary<string, FileVerInfo>();
        }

        public void load()
        {
            loadLocalRVer();
            loadLocalSVer();
            loadLocalPVer();
        }

        public void loadLocalRVer()
        {
            AuxTextLoader textLoader = new AuxTextLoader();
            textLoader.syncLoad("Version_R.txt");

            loadFormText(textLoader.getText(), m_path2Ver_R_Dic);

            textLoader.dispose();
            textLoader = null;
        }

        public void loadLocalSVer()
        {

        }

        public void loadLocalPVer()
        {

        }

        protected void loadFormText(string text, Dictionary<string, FileVerInfo> dic)
        {
            string[] lineSplitStr = { "\r\n" };
            string[] equalSplitStr = { "=" };
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

                dic[fileInfo.mResUniqueId] = fileInfo;
                ++lineIdx;
            }
        }
    }
}
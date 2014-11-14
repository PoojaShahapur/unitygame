using System.Collections.Generic;
using UnityEngine;

namespace EditorTool
{
    class ResCfgData
    {
        public List<PackType> m_packList = new List<PackType>();

        public void parseXml()
        {
            ResCfgParse resCfgParse = new ResCfgParse();
            resCfgParse.parseXml(Application.dataPath + "/Config/ResPackCfg.xml", m_packList);
        }
    }
}
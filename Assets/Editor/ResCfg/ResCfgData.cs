using System.Collections.Generic;
using UnityEngine;

namespace EditorTool
{
    class ResCfgData
    {
        public List<Pack> m_packList = new List<Pack>();

        public void parseXml()
        {
            ResCfgParse resCfgParse = new ResCfgParse();
            resCfgParse.parseXml(Application.dataPath + "/Config/ResPackCfg.xml", m_packList);
        }
    }
}
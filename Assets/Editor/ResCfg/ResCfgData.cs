using System.Collections.Generic;
using UnityEngine;

namespace Editor.ResCfg
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

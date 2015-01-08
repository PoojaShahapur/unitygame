using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MaterialMgr
    {
        public Dictionary<MaterialID, Material> m_ID2MatDic = new Dictionary<MaterialID, Material>();
        public Dictionary<string, Material> m_path2MatDic = new Dictionary<string, Material>();

        public MaterialMgr()
        {

        }

        public Material get(string path)
        {
            if(m_path2MatDic.ContainsKey(path))
            {
                return m_path2MatDic[path];
            }

            return null;
        }

        // ╪сть Material 
        public void Load(LoadParam param)
        {

        }
    }
}
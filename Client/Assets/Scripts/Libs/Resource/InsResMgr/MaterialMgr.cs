using Game.UI;
using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MaterialMgr : ResMgrBase
    {
        //public Dictionary<MaterialID, Material> m_ID2MatDic = new Dictionary<MaterialID, Material>();

        public MaterialMgr()
        {

        }

        // ͨ��ְҵ��ȡ������Ĳ���
        public MatRes getCardGroupMatByOccup(EnPlayerCareer Occup)
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_path;

            return getAndSyncLoad<MatRes>(path);
        }

        // ͨ��ְҵ��ȡ������LOGO�Ĳ���
        public MatRes getCardGroupLOGOMatByOccup(EnPlayerCareer Occup)
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_logoPath;

            return getAndSyncLoad<MatRes>(path);
        }
    }
}
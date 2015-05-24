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

        // 通过职业获取卡牌组的材质
        public MatRes getCardGroupMatByOccup(EnPlayerCareer Occup)
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_path;

            return syncGet<MatRes>(path) as MatRes;
        }

        // 通过职业获取卡牌组LOGO的材质
        public MatRes getCardGroupLOGOMatByOccup(EnPlayerCareer Occup)
        {
            string path = Ctx.m_instance.m_dataPlayer.m_dataCard.m_id2CardGroupMatAttrDic[(int)Occup].m_logoPath;

            return syncGet<MatRes>(path) as MatRes;
        }
    }
}
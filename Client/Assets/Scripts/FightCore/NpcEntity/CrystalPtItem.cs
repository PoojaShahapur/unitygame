using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class CrystalPtItem : NpcEntityBase
    {
        protected AuxDynModel m_modelItem;
        
        public AuxDynModel modelItem
        {
            get
            {
                return m_modelItem;
            }
            set
            {
                m_modelItem = value;
            }
        }

        public void load(GameObject pntGo_)
        {
            m_modelItem = new AuxDynModel();
            m_modelItem.pntGo = pntGo_;
            m_modelItem.modelResPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Scene/yun_zhanchang.prefab");
            m_modelItem.syncUpdateModel();
        }
    }
}
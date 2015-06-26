using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class CrystalPtItem : NpcEntityBase
    {
        protected AuxDynModelDynTex m_modelItem;

        public AuxDynModelDynTex modelItem
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

        override public void dispose()
        {
            if(m_modelItem != null)
            {
                m_modelItem.dispose();
                m_modelItem = null;
            }
        }

        public void load(GameObject pntGo_)
        {
            m_modelItem = new AuxDynModelDynTex();
            m_modelItem.pntGo = pntGo_;
            m_modelItem.modelResPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Scene/yun_zhanchang.prefab");
            m_modelItem.syncUpdateModel();
        }

        public void updateTexture(bool bEnable)
        {
            if(bEnable)
            {
                m_modelItem.texPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBuildImage], "yun_kapai.tga");
            }
            else
            {
                m_modelItem.texPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBuildImage], "yun2_kapai.tga");
            }

            m_modelItem.syncUpdateTex();
        }
    }
}
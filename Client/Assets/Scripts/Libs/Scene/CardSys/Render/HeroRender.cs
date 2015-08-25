using SDK.Lib;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 英雄卡渲染器
     */
    public class HeroRender : ExceptBlackCardRender
    {
        public HeroRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_subTex = new CardSubPart[1];
            m_subTex[0] = new CardSubPart();

            m_uiPrefabPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/HeroCardUI.prefab");
            m_boxModelPath = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel], "Character/HeroCardBox.prefab");
        }

        override public void dispose()
        {
            m_subTex[0].dispose();

            base.dispose();
        }

        override protected void updateLeftAttr(TableCardItemBody tableBody)
        {

        }

        // 修改卡牌纹理
        override protected void modifyTex(GameObject go_, TableCardItemBody tableBody)
        {
            // 头像是每一个卡牌一个配置
            string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathCardImage], tableBody.m_dzCardHeader);
            GameObject _go = UtilApi.TransFindChildByPObjAndPath(go_, m_cardModelItem.m_headerSubModel);
            m_subTex[0].tex.selfGo = _go;          // 场上 Hero 的纹理贴图
            m_subTex[0].tex.texPath = path;
            m_subTex[0].tex.syncUpdateTex();
        }
    }
}
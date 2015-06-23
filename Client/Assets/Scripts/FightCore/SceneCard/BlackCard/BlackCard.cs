using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 敌人的背面牌
     */
    public class BlackCard : SceneCardBase
    {
        public BlackCard(SceneDZData data) : 
            base(data)
        {
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_trackAniControl = new BlackAniControl(this);
            m_render = new BlackCardRender(this);
        }

        override public void dispose()
        {
            Ctx.m_instance.m_logSys.log("客户端彻底删除 Enemy Hand 卡牌");
            base.dispose();
        }

        override protected void removeRef()
        {
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].removeOneCard(this);
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as BlackCardRender).setIdAndPnt(objId, pntGo_);
        }

        // 更新初始卡牌场景位置信息
        override public void updateInitCardSceneInfo(Transform trans)
        {
            UtilApi.setPos(this.transform(), trans.localPosition);
            UtilApi.setScale(this.transform(), trans.localScale);
            UtilApi.setRot(this.transform(), trans.localRotation);
        }

        public override string getDesc()
        {
            return string.Format("BlackCard {0}", m_ClientId);
        }
    }
}
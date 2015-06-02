using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 这个场景特效管理器
     */
    public class SceneEffectMgr : EntityMgrBase
    {
        override protected void onTickExec(float delta)
        {
            foreach (SceneEntityBase entity in m_sceneEntityList)
            {
                entity.onTick(delta);
            }
        }

        public EffectBase createAndAdd(EffectType type)
        {
            EffectBase effect = null;

            if(EffectType.eLinkEffect == type)
            {
                effect = new LinkEffect();
            }
            else if (EffectType.eMoveEffect == type)
            {
                effect = new MoveEffect();
            }
            Ctx.m_instance.m_sceneEffectMgr.add2List(effect);

            return effect;
        }

        public void removeAndDestroy(EffectBase effect)
        {
            this.removeFromeList(effect);
            effect.dispose();
        }
    }
}
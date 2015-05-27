using SDK.Common;
namespace SDK.Lib
{
    /**
     * @brief 帧动画管理器
     */
    public class SpriteAniMgr : EntityMgrBase
    {
        override public void OnTick(float delta)
        {
            foreach(ISceneEntity entity in m_sceneEntityList)
            {
                (entity as SpriteAni).onTick(delta);
            }
        }

        public SpriteAni createAndAdd(SpriteComType type)
        {
            SpriteAni ani = null;
            if (SpriteComType.eSpriteRenderer == type)
            {
                ani = new SpriteRenderSpriteAni();
            }
            if (SpriteComType.eImage == type)
            {
                ani = new ImageSpriteAni();
            }

            Ctx.m_instance.m_spriteAniMgr.add2List(ani);
            return ani;
        }

        public void removeAndDestroy(SpriteAni ani)
        {
            this.removeFromeList(ani);
            ani.dispose();
        }
    }
}
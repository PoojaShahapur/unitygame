using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 这个是完整的显示流程，场景中不能直接使用这个，需要使用 Effect 对象
     */
    public class SpriteRenderSpriteAni : SpriteAni
    {
        protected SceneEntityBase m_entity;
        protected SpriteRenderer m_spriteRender;    // 精灵渲染器

        public SpriteRenderSpriteAni(SceneEntityBase entity_)
        {
            m_entity = entity_;
        }

        public override void dispose()
        {
            if(m_selfGo != null)        // 场景中的特效需要直接释放这个 GameObject
            {
                UtilApi.Destroy(m_selfGo);
            }
            base.dispose();
        }

        override public void stop()
        {
            base.stop();
            m_spriteRender.sprite = null;
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (m_spriteRender == null)
            {
                if (string.IsNullOrEmpty(m_goName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
                {
                    m_spriteRender = UtilApi.getComByP<SpriteRenderer>(m_selfGo);
                }
                else
                {
                    m_spriteRender = UtilApi.getComByP<SpriteRenderer>(m_pntGo, m_goName);
                }
            }
        }
        
        override protected void updateImage()
        {
            m_spriteRender.sprite = m_atlasScriptRes.getImage(m_curFrame).image;
        }

        override protected void dispEndEvent()
        {
            m_playEndEventDispatch.dispatchEvent(m_entity);
        }
    }
}
using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class SpriteRenderSpriteAni : SpriteAni
    {
        protected SpriteRenderer m_spriteRender;    // 精灵渲染器

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
    }
}
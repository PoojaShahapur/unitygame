using System;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief UI 直接使用这个精灵动画，不用使用具体 Effect 对象
     */
    public class ImageSpriteAni : SpriteAni, IDelayHandleItem
    {
        protected Image m_image;    // 精灵渲染器

        public override void dispose()
        {
            Ctx.m_instance.m_spriteAniMgr.removeFromList(this);
            base.dispose();
        }

        override public void stop()
        {
            base.stop();
            if (!m_bKeepLastFrame)
            {
                m_image.sprite = null;
            }
            m_image.rectTransform.sizeDelta = new UnityEngine.Vector2(0, 0);
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (m_image == null)
            {
                if (string.IsNullOrEmpty(m_goName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
                {
                    m_image = UtilApi.getComByP<Image>(m_selfGo);
                }
                else
                {
                    m_image = UtilApi.getComByP<Image>(m_pntGo, m_goName);
                }
            }
        }

        override public void updateImage()
        {
            //try
            //{
                m_image.sprite = m_atlasScriptRes.getImage(m_curFrame).image;
            //}
            //catch(Exception ex)
            //{
            //    Ctx.m_instance.m_logSys.catchLog(ex.ToString());
            //}

            UtilApi.SetNativeSize(m_image);
        }

        override public bool checkRender()
        {
            return m_image != null;
        }
    }
}
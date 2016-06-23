using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class TextureRes : InsResBase
    {
        public Texture m_texture;
        protected Sprite m_sprite;

        public TextureRes()
        {

        }

        public Texture getTexture()
        {
            return m_texture;
        }

        override protected void initImpl(ResItem res)
        {
            m_texture = res.getObject(res.getPrefabName()) as Texture;
            base.initImpl(res);
        }

        public override void unload()
        {
            if (m_texture != null)
            {
                // 这个接口不知道行不行
                UtilApi.UnloadAsset(m_texture);
                m_texture = null;

                // 这个接口肯定可以
                //UtilApi.UnloadUnusedAssets();
            }
            base.unload();
        }

        // 设置图像的纹理资源
        public void setImageTex(Image image)
        {
            if(image != null)
            {
                if (m_sprite == null)
                {
                    if (m_texture != null)
                    {
                        m_sprite = UtilApi.Create(m_texture as Texture2D, new Rect(0, 0, m_texture.width, m_texture.height), new Vector2(0.5f, 0.5f));
                    }
                }
                // 创建一个 Sprite 
                image.sprite = m_sprite;
            }
        }
    }
}
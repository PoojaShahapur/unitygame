using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class TextureRes : InsResBase
    {
        public Texture mTexture;
        protected Sprite mSprite;

        public TextureRes()
        {

        }

        public Texture getTexture()
        {
            return mTexture;
        }

        override protected void initImpl(ResItem res)
        {
            mTexture = res.getObject(res.getPrefabName()) as Texture;
            base.initImpl(res);
        }

        public override void unload()
        {
            if (mTexture != null)
            {
                // 这个接口不知道行不行
                UtilApi.UnloadAsset(mTexture);
                mTexture = null;

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
                if (mSprite == null)
                {
                    if (mTexture != null)
                    {
                        mSprite = UtilApi.Create(mTexture as Texture2D, new Rect(0, 0, mTexture.width, mTexture.height), new Vector2(0.5f, 0.5f));
                    }
                }
                // 创建一个 Sprite 
                image.sprite = mSprite;
            }
        }
    }
}
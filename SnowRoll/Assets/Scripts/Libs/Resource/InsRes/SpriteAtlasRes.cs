using UnityEngine;

namespace SDK.Lib
{
    public class SpriteAtlasRes : InsResBase
    {
        protected Sprite[] mSpriteAtlas;

        public SpriteAtlasRes()
        {

        }

        override protected void initImpl(ResItem res)
        {
            this.mSpriteAtlas = res.loadAllAssets<UnityEngine.Sprite>();
            base.initImpl(res);
        }

        public Sprite[] getSpriteAtlas()
        {
            return this.mSpriteAtlas;
        }

        public UnityEngine.Sprite getSprite(string spriteName)
        {
            spriteName = UtilApi.convFullPath2SpriteName(spriteName);

            UnityEngine.Sprite sprite = null;
            int idx = 0;
            int len = this.mSpriteAtlas.Length;

            while(idx < len)
            {
                if(this.mSpriteAtlas[idx].name == spriteName)
                {
                    sprite = this.mSpriteAtlas[idx];
                    break;
                }

                ++idx;
            }

            return sprite;
        }

        public override void unload()
        {
            if (this.mSpriteAtlas != null)
            {
                int idx = 0;
                int len = this.mSpriteAtlas.Length;
                while (idx < len)
                {
                    if (this.mSpriteAtlas[idx] != null)
                    {
                        UtilApi.UnloadAsset(this.mSpriteAtlas[idx]);
                        this.mSpriteAtlas[idx] = null;
                    }
                }
                this.mSpriteAtlas = null;

                // 这个接口肯定可以
                //UtilApi.UnloadUnusedAssets();
            }

            base.unload();
        }
    }
}
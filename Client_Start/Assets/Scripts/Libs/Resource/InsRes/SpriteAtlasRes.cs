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
            mSpriteAtlas = res.loadAllAssets<UnityEngine.Sprite>();
            base.initImpl(res);
        }

        public Sprite[] getSpriteAtlas()
        {
            return mSpriteAtlas;
        }

        public UnityEngine.Sprite getSprite(string spriteName)
        {
            int idx = 0;
            int len = mSpriteAtlas.Length;
            while(idx < len)
            {
                if(mSpriteAtlas[idx].name == spriteName)
                {
                    return mSpriteAtlas[idx];
                }

                ++idx;
            }

            return null;
        }

        public override void unload()
        {
            //UtilApi.UnloadAsset(mSpriteAtlas);
            mSpriteAtlas = null;
            base.unload();
        }
    }
}
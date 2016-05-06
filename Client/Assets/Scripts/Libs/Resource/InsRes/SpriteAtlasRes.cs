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
            UnityEngine.Object[] ret = res.getAllObject();
            mSpriteAtlas = ret as Sprite[];
            base.initImpl(res);
        }

        public Sprite[] getSpriteAtlas()
        {
            return mSpriteAtlas;
        }

        public override void unload()
        {
            //UtilApi.UnloadAsset(mSpriteAtlas);
            mSpriteAtlas = null;
            base.unload();
        }
    }
}
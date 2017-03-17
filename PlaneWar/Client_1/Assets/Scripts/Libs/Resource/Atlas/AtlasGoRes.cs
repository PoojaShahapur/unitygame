using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 一项地图集加载资源
     */
    public class AtlasGoRes : InsResBase
    {
        public GameObject mGo;
        protected GameObject mSubGo;

        protected MDictionary<string, ImageItem> mPath2Image;

        public AtlasGoRes()
        {
            mPath2Image = new MDictionary<string, ImageItem>();
        }

        public override void unload()
        {
            mGo = null;
        }

        public ImageItem getImage(string spriteName)
        {
            if(!mPath2Image.ContainsKey(spriteName))
            {
                mSubGo = UtilApi.TransFindChildByPObjAndPath(mGo, spriteName);
                Image image = UtilApi.getComByP<Image>(mSubGo);
                ImageItem item = new ImageItem();
                item.image = image.sprite;
                mPath2Image[spriteName] = item;
            }
            else
            {
                mPath2Image[spriteName].refCountResLoadResultNotify.refCount.incRef();
            }

            return mPath2Image[spriteName];
        }
    }
}
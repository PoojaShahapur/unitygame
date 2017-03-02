using UnityEngine;

namespace SDK.Lib
{
    public class MatRes : InsResBase
    {
        protected Material mMat;

        public MatRes()
        {

        }

        public Material getMat()
        {
            return mMat;
        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            mMat = res.getObject(res.getPrefabName()) as Material;

            base.initImpl(res);
        }

        public override void unload()
        {
            if (mMat != null)
            {
                // 这个接口不知道行不行
                UtilApi.UnloadAsset(mMat);
                mMat = null;

                // 这个接口肯定可以
                //UtilApi.UnloadUnusedAssets();
            }

            base.unload();
        }
    }
}
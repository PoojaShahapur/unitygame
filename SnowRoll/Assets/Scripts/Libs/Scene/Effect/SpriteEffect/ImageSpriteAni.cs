using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief UI 直接使用这个精灵动画，不用使用具体 Effect 对象
     */
    public class ImageSpriteAni : SpriteAni, IDelayHandleItem
    {
        protected Image mImage;    // 精灵渲染器

        public override void dispose()
        {
            Ctx.mInstance.mSpriteAniMgr.removeFromList(this);
            base.dispose();
        }

        override public void stop()
        {
            base.stop();
            if (!this.mIsKeepLastFrame)
            {
                this.mImage.sprite = null;
            }
            this.mImage.rectTransform.sizeDelta = new UnityEngine.Vector2(0, 0);
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (this.mImage == null)
            {
                if (string.IsNullOrEmpty(this.mGoName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
                {
                    this.mImage = UtilApi.getComByP<Image>(this.mSelfGo);
                }
                else
                {
                    this.mImage = UtilApi.getComByP<Image>(this.mPntGo, this.mGoName);
                }
            }
        }

        override public void updateImage()
        {
            //try
            //{
                this.mImage.sprite = this.mAtlasScriptRes.getImage(this.mCurFrame).image;
            //}
            //catch(Exception ex)
            //{
            //    Ctx.mInstance.mLogSys.catchLog(ex.ToString());
            //}

            UtilApi.SetNativeSize(this.mImage);
        }

        override public bool checkRender()
        {
            return this.mImage != null;
        }
    }
}
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 动态加载的 Image，但是 GameObject 是静态的
     */
    public class AuxDynImageStaticGOImage : AuxDynAtlasImage
    {
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
                    this.mImage = UtilApi.getComByP<Image>(mPntGo, this.mGoName);
                }
            }
        }

        // 同步更新显示
        override public void syncUpdateCom()
        {
            findWidget();
            base.syncUpdateCom();
        }
    }
}
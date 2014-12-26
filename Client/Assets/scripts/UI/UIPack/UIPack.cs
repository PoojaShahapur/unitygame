using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 包裹
     */
    public class UIPack : Form, IUIPack
    {
        public UIPack()
        {

        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        // 关联窗口
        protected void getWidget()
        {

        }

        protected void addEventHandle()
        {
            
        }
    }
}
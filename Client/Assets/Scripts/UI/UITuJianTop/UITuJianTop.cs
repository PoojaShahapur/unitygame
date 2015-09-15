using SDK.Lib;

namespace Game.UI
{
    public class UITuJianTop : Form, IUITuJianTop
    {
        public TuJianTopData m_tuJianTopData;

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            m_tuJianTopData = new TuJianTopData(this);

            findWidget();
            addEventHandle();

            m_tuJianTopData.init();
        }

        override public void onShow()
        {
            base.onShow();
        }

        public override void onExit()
        {
            base.onExit();
            m_tuJianTopData.dispose();
        }

        public void findWidget()
        {
            m_tuJianTopData.findWidget();
        }

        public void addEventHandle()
        {
            m_tuJianTopData.addEventHandle();
        }
    }
}
using SDK.Common;

namespace Game.UI
{
    public class ClassFilterPnl : TuJianPnlBase
    {
        public int m_tabBtnIdx = -1;         // 当前点击的 Tab Btn 索引

        public ClassFilterPnl(TuJianData data):
            base(data)
        {
            
        }

        // 点击分类按钮
        public void updateByCareer(EnPlayerCareer myclass, bool bUpdate)
        {
            if ((int)myclass != m_tabBtnIdx)
            {
                m_tabBtnIdx = (int)myclass;
            }

            m_tuJianData.m_wdscCardPnl.destroyAndUpdateCardList();
        }

        // 返回是否是当前的 Up 按钮索引
        public bool bCurUpBtn(int btnIdx)
        {
            return m_tabBtnIdx == btnIdx;
        }
    }
}
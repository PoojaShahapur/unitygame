using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief ����
     */
    public class UIPack : Form, IUIPack
    {
        public UIPack()
        {

        }

        // ��ʼ���ؼ�
        override public void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        // ��������
        protected void getWidget()
        {

        }

        protected void addEventHandle()
        {
            
        }
    }
}
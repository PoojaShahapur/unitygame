using SDK.Lib;

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

            findWidget();
            addEventHandle();
        }

        // ��������
        protected void findWidget()
        {

        }

        protected void addEventHandle()
        {
            
        }
    }
}
using SDK.Lib;

namespace SDK.Lib
{
    public class HurtSeqData
    {
        protected MList<OneHurtFlowSeq> m_hurtSeqList;

        public HurtSeqData()
        {
            m_hurtSeqList = new MList<OneHurtFlowSeq>();
        }

        public void onTime(float delta)
        {

        }
    }
}
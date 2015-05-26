using SDK.Common;
using UnityEngine;

namespace Game.UI
{
    public class TuJianTopPnlBase
    {
        public TuJianTopData m_tuJianTopData;
        public GameObject m_go;
        public bool m_bInit = false;

        public TuJianTopPnlBase(TuJianTopData data)
        {
            m_tuJianTopData = data;
        }

        virtual public void findWidget()
        {

        }

        virtual public void addEventHandle()
        {

        }

        virtual public void init()
        {
            UtilApi.SetActive(m_go, true);
        }

        virtual public void dispose()
        {

        }
    }
}
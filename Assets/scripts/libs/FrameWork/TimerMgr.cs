using SDK.Common;
using System.Collections.Generic;

/**
 * @brief 定时器管理器
 */
namespace SDK.Lib
{
    public class TimerMgr : ITimerMgr
    {
        protected Dictionary<TimerType, TimerList> m_id2TimerLstDic;

        public TimerMgr()
        {
            m_id2TimerLstDic = new Dictionary<TimerType, TimerList>();
            RegisterTimer();
        }

        // 注册定时器
        protected void RegisterTimer()
        {
            m_id2TimerLstDic[TimerType.eTickTimer] = new TimerList();
            m_id2TimerLstDic[TimerType.eTickTimer].m_Delta = 0;
            m_id2TimerLstDic[TimerType.eTickTimer].m_CVDelta = 0;

            m_id2TimerLstDic[TimerType.eOneSecTimer] = new TimerList();
            m_id2TimerLstDic[TimerType.eOneSecTimer].m_Delta = 1;
            m_id2TimerLstDic[TimerType.eOneSecTimer].m_CVDelta = 1;

            m_id2TimerLstDic[TimerType.eFiveSecTimer] = new TimerList();
            m_id2TimerLstDic[TimerType.eFiveSecTimer].m_Delta = 5;
            m_id2TimerLstDic[TimerType.eFiveSecTimer].m_CVDelta = 5;
        }

        public void Advance(float delta)
        {
            foreach(TimerList tmLst in m_id2TimerLstDic.Values)
            {
                tmLst.OnTimer(delta);
            }
        }
    }
}
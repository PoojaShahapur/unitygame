using System;
namespace SDK.Common
{
    public enum EGameStage
    {
        eStage_None,            // 未知状态
        eStage_Login,           // 在登陆模块
        eStage_Game,            // 在游戏模块
        eStage_DZ,              // 在战斗模块
    }

    /**
     * @brief 游戏所处的 stage
     */
    public class GameRunStage
    {
        protected EGameStage m_ePreGameStage = EGameStage.eStage_None;          // 之前游戏状体
        protected EGameStage m_eCurGameStage = EGameStage.eStage_None;             // 当前游戏状态
        protected Action<EGameStage> m_quitStageDisp;           // 退出状态分发
        protected Action<EGameStage> m_enteringStageDisp;       // 正在进入状态分发
        protected Action<EGameStage> m_enteredStageDisp;        // 进入状态分发

        public void toggleGameStage(EGameStage newStage)
        {
            if (newStage != m_eCurGameStage)
            {
                quitCurStage();
                m_ePreGameStage = m_eCurGameStage;
                m_eCurGameStage = newStage;
                enteringCurStage();
            }
        }

        // 退出当前 stage
        protected void quitCurStage()
        {
            if(m_quitStageDisp != null)
            {
                m_quitStageDisp(m_eCurGameStage);
            }
        }

        protected void enteringCurStage()
        {
            if (m_enteringStageDisp != null)
            {
                m_enteringStageDisp(m_eCurGameStage);
            }
        }

        public void enteredCurStage()
        {
            if (m_enteredStageDisp != null)
            {
                m_enteredStageDisp(m_eCurGameStage);
            }
        }

        public bool isPreInStage(EGameStage eGameStage)
        {
            return m_eCurGameStage == eGameStage;
        }

        public bool isCurInStage(EGameStage eGameStage)
        {
            return m_eCurGameStage == eGameStage;
        }

        public void addQuitDisp(Action<EGameStage> handle)
        {
            m_quitStageDisp += handle;
        }

        public void removeQuitDisp(Action<EGameStage> handle)
        {
            m_quitStageDisp -= handle;
        }

        public void addEnteringDisp(Action<EGameStage> handle)
        {
            m_enteringStageDisp += handle;
        }

        public void removeEnteringDisp(Action<EGameStage> handle)
        {
            m_enteringStageDisp -= handle;
        }

        public void addEnteredDisp(Action<EGameStage> handle)
        {
            m_enteredStageDisp += handle;
        }

        public void removeEnteredDisp(Action<EGameStage> handle)
        {
            m_enteredStageDisp -= handle;
        }
    }
}
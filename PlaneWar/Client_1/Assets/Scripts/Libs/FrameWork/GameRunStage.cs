using System;

namespace SDK.Lib
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
        protected Action<EGameStage, EGameStage> m_quitingAndEnteringStageDisp;       // 退出并且正在进入状态分发
        protected Action<EGameStage, EGameStage> m_quitedAndEnteredStageDisp;               // 退出后进入状态分发

        public EGameStage ePreGameStage
        {
            get
            {
                return this.m_ePreGameStage;
            }
        }

        public EGameStage eCurGameStage
        {
            get
            {
                return this.m_eCurGameStage ;
            }
        }

        public void toggleGameStage(EGameStage newStage)
        {
            if (newStage != this.m_eCurGameStage)
            {
                this.m_ePreGameStage = this.m_eCurGameStage;
                this.m_eCurGameStage = newStage;
                this.quitingAndEnteringCurStage();
            }
        }

        protected void quitingAndEnteringCurStage()
        {
            if (this.m_quitingAndEnteringStageDisp != null)
            {
                this.m_quitingAndEnteringStageDisp(m_ePreGameStage, m_eCurGameStage);
            }
        }

        public void quitedAndEnteredCurStage()
        {
            if (this.m_quitedAndEnteredStageDisp != null)
            {
                this.m_quitedAndEnteredStageDisp(this.m_ePreGameStage, this.m_eCurGameStage);
            }
        }

        public bool isPreInStage(EGameStage eGameStage)
        {
            return this.m_eCurGameStage == eGameStage;
        }

        public bool isCurInStage(EGameStage eGameStage)
        {
            return this.m_eCurGameStage == eGameStage;
        }

        public void addQuitingAndEnteringDisp(Action<EGameStage, EGameStage> handle)
        {
            this.m_quitingAndEnteringStageDisp += handle;
        }

        public void removeQuitingAndEnteringDisp(Action<EGameStage, EGameStage> handle)
        {
            this.m_quitingAndEnteringStageDisp -= handle;
        }

        public void addQuitedAndEnteredDisp(Action<EGameStage, EGameStage> handle)
        {
            this.m_quitedAndEnteredStageDisp += handle;
        }

        public void removeQuitedAndEnteredDisp(Action<EGameStage, EGameStage> handle)
        {
            this.m_quitedAndEnteredStageDisp -= handle;
        }
    }
}
using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    public enum FightExecParallelMask
    {
        eAttackHurt,        // 攻击被击，包括普通和技能
        eDie,               // 死亡
    }

    /**
     * @brief 基本战斗项
     */
    public class FightRoundItemBase : IDispatchObject
    {
        protected SceneDZData m_sceneDZData;
        protected byte m_parallelMask;          // 可以和自己连续的战斗 Item 同时执行的 Mask
        protected FightExecParallelMask m_parallelFlag;     // 自己的标志
        protected EventDispatch m_OneAttackAndHurtEndDisp;

        public FightRoundItemBase(SceneDZData data)
        {
            m_sceneDZData = data;
            m_OneAttackAndHurtEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public byte parallelMask
        {
            get
            {
                return m_parallelMask;
            }
            set
            {
                m_parallelMask = value;
            }
        }

        public FightExecParallelMask parallelFlag
        {
            get
            {
                return m_parallelFlag;
            }
            set
            {
                m_parallelFlag = value;
            }
        }

        public void addOneAttackAndHurtEndHandle(System.Action<IDispatchObject> dispObj)
        {
            m_OneAttackAndHurtEndDisp.addEventHandle(dispObj);
        }

        virtual public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {

        }

        virtual public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {

        }

        virtual public void processOneAttack()
        {

        }
    }
}
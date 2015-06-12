using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 基本战斗项
     */
    public class FightRoundItemBase : IDispatchObject
    {
        protected SceneDZData m_sceneDZData;
        protected EventDispatch m_OneAttackAndHurtEndDisp;

        public FightRoundItemBase(SceneDZData data)
        {
            m_sceneDZData = data;
            m_OneAttackAndHurtEndDisp = new AddOnceAndCallOnceEventDispatch();
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
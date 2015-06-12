using Game.Msg;
using SDK.Common;
using SDK.Lib;
namespace FightCore
{
    /**
     * @brief 战斗回合
     */
    public class FightRound : IDispatchObject
    {
        protected SceneDZData m_sceneDZData;
        protected FightRoundItemBase m_curFightData;     // 当前战斗数据
        protected MList<FightRoundItemBase> m_cacheList; // 缓存的战斗数据列表
        protected EventDispatch m_roundEndDisp;

        public FightRound(SceneDZData data)
        {
            m_sceneDZData = data;
            m_curFightData = null;
            m_cacheList = new MList<FightRoundItemBase>();
            m_roundEndDisp = new AddOnceAndCallOnceEventDispatch();
        }

        public SceneDZData sceneDZData
        {
            get
            {
                return m_sceneDZData;
            }
            set
            {
                m_sceneDZData = value;
            }
        }

        public void addRoundEndHandle(System.Action<IDispatchObject> dispObj)
        {
            m_roundEndDisp.addEventHandle(dispObj);
        }

        public void onOneAttackAndHurtEndHandle(IDispatchObject dispObj)
        {
            m_curFightData = null;
            nextOneAttact();
        }

        public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            FightRoundItemBase attItem = new FightRoundAttItem(m_sceneDZData);
            attItem.addOneAttackAndHurtEndHandle(onOneAttackAndHurtEndHandle);
            attItem.psstNotifyBattleCardPropertyUserCmd(msg);
            m_cacheList.Add(attItem);
            nextOneAttact();
        }

        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            FightRoundItemBase attItem = new FightRoundDelItem(m_sceneDZData);
            attItem.addOneAttackAndHurtEndHandle(onOneAttackAndHurtEndHandle);
            attItem.psstRetRemoveBattleCardUserCmd(msg, side, sceneItem);
            m_cacheList.Add(attItem);
        }

        public void nextOneAttact()
        {
            if (m_curFightData == null)     // 如果当前没有攻击进行
            {
                if (m_cacheList.Count() > 0)    // 如果有攻击数据
                {
                    m_curFightData = m_cacheList[0];
                    m_cacheList.Remove(m_curFightData);
                    m_curFightData.processOneAttack();
                }
                else
                {
                    m_roundEndDisp.dispatchEvent(this);
                }
            }
        }
    }
}
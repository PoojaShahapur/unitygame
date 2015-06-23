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
        protected static int ID_ALLOC_IDX = 0;      // 分配 ID 索引的

        protected SceneDZData m_sceneDZData;
        protected FightRoundItemBase m_curFightData;        // 当前战斗数据
        protected FightRoundItemBase m_nextFightData;       // 下一个战斗数据
        protected MList<FightRoundItemBase> m_cacheList;    // 缓存的战斗数据列表
        protected EventDispatch m_roundEndDisp;
        protected bool m_bSvrRoundEnd;      // 服务器的战斗回合数据是否结束
        protected bool m_bHasFightData;     // 是否有战斗数据
        protected int m_uniqueId;           // 战斗回合唯一 Id

        public FightRound(SceneDZData data)
        {
            m_sceneDZData = data;
            m_curFightData = null;
            m_cacheList = new MList<FightRoundItemBase>();
            m_roundEndDisp = new AddOnceAndCallOnceEventDispatch();
            m_bSvrRoundEnd = false;

            m_uniqueId = ID_ALLOC_IDX;
            ++ID_ALLOC_IDX;
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

        public bool bSvrRoundEnd
        {
            get
            {
                return m_bSvrRoundEnd;
            }
            set
            {
                m_bSvrRoundEnd = value;
            }
        }

        public bool bHasFightData
        {
            get
            {
                return m_bHasFightData;
            }
            set
            {
                m_bHasFightData = value;
            }
        }

        public int uniqueId
        {
            get
            {
                return m_uniqueId;
            }
            set
            {
                m_uniqueId = value;
            }
        }

        public void addRoundEndHandle(System.Action<IDispatchObject> dispObj)
        {
            m_roundEndDisp.addEventHandle(dispObj);
        }

        public void onOneAttackAndHurtEndHandle(IDispatchObject dispObj)
        {
            if (UtilApi.isAddressEqual(dispObj, m_curFightData))        // 死亡是可以并行执行的，但是只保留并行执行的最后一个 Item ，只有最后一个回调才起作用
            {
                m_curFightData = null;
                nextOneAttact();
            }
        }

        public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            m_bHasFightData = true;
            FightRoundItemBase attItem = null;
            if (0 == msg.type)      // 攻击
            {
                attItem = new FightRoundAttItem(m_sceneDZData);
            }
            else if (1 == msg.type)     // 召唤
            {
                attItem = new FightRoundSummonItem(m_sceneDZData);
            }
            else if (2 == msg.type)     // 抽牌
            {
                attItem = new FightRoundGetItem(m_sceneDZData);
            }

            attItem.addOneAttackAndHurtEndHandle(onOneAttackAndHurtEndHandle);
            attItem.psstNotifyBattleCardPropertyUserCmd(msg);
            m_cacheList.Add(attItem);
            //nextOneAttact();
        }

        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            m_bHasFightData = true;
            FightRoundItemBase attItem = new FightRoundDelItem(m_sceneDZData);
            attItem.addOneAttackAndHurtEndHandle(onOneAttackAndHurtEndHandle);
            attItem.psstRetRemoveBattleCardUserCmd(msg, side, sceneItem);
            m_cacheList.Add(attItem);
            //nextOneAttact();
        }

        public void nextOneAttact()
        {
            if (m_curFightData == null)     // 如果当前没有攻击进行
            {
                if (m_cacheList.Count() > 0)    // 如果有攻击数据
                {
                    while (m_cacheList.Count() > 0)         // 死亡是可以并行执行的
                    {
                        m_curFightData = m_cacheList[0];

                        Ctx.m_instance.m_logSys.fightLog(string.Format("\n[Fight] 开始进行第 {0} 回合中的第 {1} 次战斗", m_uniqueId, m_curFightData.uniqueId));

                        m_cacheList.Remove(m_curFightData);
                        m_curFightData.processOneAttack();

                        if(m_cacheList.Count() > 0)
                        {
                            m_nextFightData = m_cacheList[0];
                            if(!canParallelExec(m_curFightData, m_nextFightData))
                            {
                                break;
                            }
                        }
                    }
                }
                else if (m_bSvrRoundEnd)        // 服务器通知才算是结算
                {
                    m_roundEndDisp.dispatchEvent(this);
                }
            }
        }

        // 检查两个攻击、死亡是否可以同时进行，死亡可以，攻击被击不行
        protected bool canParallelExec(FightRoundItemBase lhv, FightRoundItemBase rhv)
        {
            return UtilMath.checkState((int)lhv.parallelFlag, rhv.parallelMask);
        }
    }
}
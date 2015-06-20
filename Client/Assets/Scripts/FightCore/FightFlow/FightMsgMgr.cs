using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 战斗消息都放在这里缓存，需要的时候再拿出来执行
     */
    public class FightMsgMgr
    {
        protected SceneDZData m_sceneDZData;
        protected FightRound m_curFightData;        // 当前战斗数据
        protected FightRound m_curParseRound;       // 当前正在解析的客户端认为的一个战斗回合 
        protected MList<FightRound> m_cacheList; // 缓存的战斗数据列表

        public FightMsgMgr(SceneDZData data)
        {
            m_sceneDZData = data;
            m_curFightData = null;
            m_curParseRound = null;
            m_cacheList = new MList<FightRound>();
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

        // 接收到消息
        public void psstNotifyBattleCardPropertyUserCmd(stNotifyBattleCardPropertyUserCmd msg)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 接收到攻击数据");

            m_curParseRound.psstNotifyBattleCardPropertyUserCmd(msg);
            //nextOneAttactRound();
        }

        // 删除一个消息
        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 接收到删除数据");

            m_curParseRound.psstRetRemoveBattleCardUserCmd(msg, side, sceneItem);
            //nextOneAttactRound();
        }

        // 一个操作开始
        public void psstNotifyBattleFlowStartUserCmd(ByteBuffer ba)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 接收到战斗回合开始指令");

            // 如果是空值才申请，否则就直接使用
            if (m_curParseRound == null)
            {
                m_curParseRound = new FightRound(m_sceneDZData);
                m_curParseRound.addRoundEndHandle(onOneRoundEnd);
                //m_cacheList.Add(m_curParseRound);
            }
        }

        // 一个操作结束
        public void psstNotifyBattleFlowEndUserCmd(ByteBuffer ba)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 接收到战斗回合结束指令");

            if (m_curParseRound.bHasFightData)    // 说明有真正的攻击数据
            {
                m_cacheList.Add(m_curParseRound);   // 结束的时候才添加，因为现在有很多只有开始和结束的消息，没有战斗的消息
                m_curParseRound.bSvrRoundEnd = true;
                m_curParseRound = null;             // 设置成空值
                nextOneAttactRound();               // 真正有消息数据的时候，才开始下一场战斗
            }
        }

        // 一个战斗回合结束
        public void onOneRoundEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.fightLog("[Fight] 结束一场战斗回合，将要开始下一场战斗回合攻击");

            m_curFightData = null;
            nextOneAttactRound();
        }

        protected void nextOneAttactRound()
        {
            if (m_curFightData == null)     // 如果当前没有攻击进行
            {
                if (m_cacheList.Count() > 0)    // 如果有攻击数据
                {
                    m_curFightData = m_cacheList[0];
                    m_cacheList.Remove(m_curFightData);
                    m_curFightData.nextOneAttact();
                }
            }
        }
    }
}
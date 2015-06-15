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

        //public int m_attCount;              // 当前一局攻击数量
        //public int m_delCount;              // 当前一局删除数量

        public FightMsgMgr(SceneDZData data)
        {
            //m_attCount = 0;
            //m_delCount = 1;     // 这个方便流程统一处理

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
            Ctx.m_instance.m_logSys.log("[Fight] 接收到攻击数据");

            //if(m_delCount != 0)         // 战斗回合开始，新的战斗回合
            //{
            //    m_delCount = 0;         // 新的回合要清除删除数量
            //    m_attCount = 1;         // 新的战斗回合开始

                //m_curParseRound = new FightRound(m_sceneDZData);
                //m_curParseRound.addRoundEndHandle(onOneRoundEnd);
                //m_cacheList.Add(m_curParseRound);
            //}
            //else
            //{
            //    ++m_attCount;
            //}

            m_curParseRound.psstNotifyBattleCardPropertyUserCmd(msg);
            nextOneAttactRound();
        }

        // 删除一个消息
        public void psstRetRemoveBattleCardUserCmd(stRetRemoveBattleCardUserCmd msg, int side, SceneCardItem sceneItem)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 接收到删除数据");

            //f(m_attCount != 0)         // 战斗回合结束，接收第一个删除消息
            //{
            //    m_attCount = 0;
            //    m_delCount = 1;
            //}
            //else
            //{
            //    ++m_delCount;
            //}

            m_curParseRound.psstRetRemoveBattleCardUserCmd(msg, side, sceneItem);
            nextOneAttactRound();
        }

        public void psstNotifyBattleFlowStartUserCmd(ByteBuffer ba)
        {
            m_curParseRound = new FightRound(m_sceneDZData);
            m_curParseRound.addRoundEndHandle(onOneRoundEnd);
            m_cacheList.Add(m_curParseRound);
        }

        public void psstNotifyBattleFlowEndUserCmd(ByteBuffer ba)
        {
            m_curParseRound.bSvrRoundEnd = true;
        }

        // 一个战斗回合结束
        public void onOneRoundEnd(IDispatchObject dispObj)
        {
            Ctx.m_instance.m_logSys.log("[Fight] 结束一场战斗回合，将要开始下一场战斗回合攻击攻击");

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
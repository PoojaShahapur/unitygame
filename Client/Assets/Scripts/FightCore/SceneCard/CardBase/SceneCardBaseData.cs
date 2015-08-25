using BehaviorLibrary;
using FSM;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class SceneCardBaseData
    {
        public ushort m_curIndex = 0;// 当前索引，因为可能初始卡牌的时候 m_sceneCardItem 
        public ushort m_preIndex = 0;// 在牌中的索引，主要是手里的牌和打出去的牌，这个是客户端设置的索引，服务器的索引在 t_Card 类型里面

        public AuxDynModel m_chaHaoModel = null;
        public uint m_startCardID = 0;

        public FightData m_fightData = null;            // 战斗数据
        public AnimFSM m_animFSM = null;                // 动画状态机

        public AIController m_aiController = null;
        public BehaviorControl m_behaviorControl = null;
        public TrackAniControl m_trackAniControl = null;
        public IOControlBase m_ioControl = null;
        public EffectControl m_effectControl = null;
        public CardMoveControl m_moveControl = null;
    }
}
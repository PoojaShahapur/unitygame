using BehaviorLibrary;
using FSM;
using UnityEngine;

namespace Game.UI
{
    public class SceneCardBaseData
    {
        public ushort m_curIndex = 0;// 当前索引，因为可能初始卡牌的时候 m_sceneCardItem 
        public ushort m_preIndex = 0;// 在牌中的索引，主要是手里的牌和打出去的牌，这个是客户端设置的索引，服务器的索引在 t_Card 类型里面

        public GameObject m_chaHaoGo;
        public uint m_startCardID;

        public FightData m_fightData;            // 战斗数据
        public AnimFSM m_animFSM;                // 动画状态机

        public AIController m_aiController;
        public BehaviorControl m_behaviorControl;
        public ClickControl m_clickControl;
        public AniControl m_aniControl;
        public DragControl m_dragControl;
        public EffectControl m_effectControl;
    }
}
using Game.Msg;
using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 场景中可拖放的卡牌，仅限场景中可拖放的卡牌
     */
    public class SceneDragCard : SceneCardEntityBase
    {
        public override void Start()
        {
            base.Start();

            UIDragObject drag = gameObject.AddComponent<UIDragObject>();
            drag.target = gameObject.transform;
            drag.m_dropEndDisp = onDrogEnd;

            WindowDragTilt title = gameObject.AddComponent<WindowDragTilt>();
        }

        // 拖放结束处理
        protected void onDrogEnd()
        {
            stMoveGameCardUserCmd cmd = new stMoveGameCardUserCmd();
            cmd.dst = new stObjectLocation();
            cmd.dst.dwLocation = (int)CardArea.CARDCELLTYPE_COMMON;
            cmd.dst.dwTableID = 0;
            cmd.dst.x = 0;
            cmd.dst.y = 1;
            cmd.qwThisID = m_sceneCardItem.m_svrCard.qwThisID;
            UtilMsg.sendMsg(cmd);
        }
    }
}
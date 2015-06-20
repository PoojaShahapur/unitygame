using Game.Msg;

namespace SDK.Common
{
    public interface IUIJobSelect : IUIBase
    {
        void psstRetHeroFightMatchUserCmd(stRetHeroFightMatchUserCmd cmd);
        // 更新 hero 显示
        void updateHeroList();
        // 返回进入战斗场景消息
        void psstRetHeroIntoBattleSceneUserCmd(ByteBuffer msg);
    }
}
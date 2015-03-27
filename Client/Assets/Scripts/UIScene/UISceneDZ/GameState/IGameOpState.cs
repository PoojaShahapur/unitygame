using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 游戏当前操作状态，仅处理场景中交互的内容，如果触摸到不交互的场景的内容，不打断场景逻辑
     */
    public interface IGameOpState
    {
        void enterAttackOp(EnGameOp op, SceneCardEntityBase card);
        void quitAttackOp();
        bool bInOp(EnGameOp op);
        bool canAttackOp(SceneCardEntityBase card, EnGameOp gameOp);
        uint getOpCardID();
        int getOpCardFaShu();
    }
}
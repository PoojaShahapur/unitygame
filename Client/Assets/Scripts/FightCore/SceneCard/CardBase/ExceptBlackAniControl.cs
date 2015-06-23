using SDK.Common;
using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 除了 Enemy 手牌的所有牌的动画
     */
    public class ExceptBlackAniControl : TrackAniControl
    {
        public ExceptBlackAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        override public void dispose()
        {
            base.dispose();
        }
    }
}
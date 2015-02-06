using Game.UI;
using SDK.Common;

namespace Game.Game
{
    /**
     * @brief UI 创建工厂
     */
    public class GameUIFactory : IUIFactory
    {
        public IForm CreateForm(UIFormID id)
        {
            switch(id)
            {
                case UIFormID.UIPack:
                {
                    return new UIPack();
                }
                case UIFormID.UIBlurBg:
                {
                    return new UIBlurBg();
                }
                case UIFormID.UITest:
                {
                    return new UITest();
                }
                case UIFormID.UIDZ:
                {
                    return new UIDZ();
                }
                case UIFormID.UIExtraOp:
                {
                    return new UIExtraOp();
                }
                case UIFormID.UIChat:
                {
                    return new UIChat();
                }
                default:
                {
                    break;
                }
            }

            return null;
        }
    }
}
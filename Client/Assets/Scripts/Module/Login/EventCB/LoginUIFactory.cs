using Game.UI;
using SDK.Common;

namespace Game.Login
{
    /**
     * @brief UI 创建工厂
     */
    public class LoginUIFactory : IUIFactory
    {
        public IForm CreateForm(UIFormID id)
        {
            switch (id)
            {
                case UIFormID.UILogin:
                {
                    return new UILogin();
                }
                case UIFormID.UIHeroSelect:
                {
                    return new UIHeroSelect();
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
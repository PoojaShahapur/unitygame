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
            if (UIFormID.UILogin == id)
            {
                return new UILogin();
            }

            return null;
        }
    }
}
using Game.UI;
using SDK.Common;

namespace Game.Game
{
    /**
     * @brief UI 创建工厂
     */
    public class GameUISceneFactory : IUISceneFactory
    {
        public ISceneForm CreateSceneForm(UISceneFormID id)
        {
            switch(id)
            {
                case UISceneFormID.eUISceneMain:
                {
                    return new UISceneMain();
                }
                case UISceneFormID.eUISceneExtPack:
                {
                    return new UISceneExtPack();
                }
                case UISceneFormID.eUISceneShop:
                {
                    return new UISceneShop();
                }
                case UISceneFormID.eUISceneWDSC:
                {
                    return new UISceneWDSC();
                }
                case UISceneFormID.eUISceneMoShi:
                {
                    return new UISceneMoShi();
                }
                case UISceneFormID.eUISceneHero:
                {
                    return new UISceneHero();
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
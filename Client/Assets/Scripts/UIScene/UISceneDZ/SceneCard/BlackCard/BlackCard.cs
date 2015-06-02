using SDK.Lib;

namespace Game.UI
{
    public class BlackCard : SceneCard
    {
        public BlackCard(SceneDZData data) : 
            base(data)
        {
            m_render = new BlackCardRender();
        }
    }
}
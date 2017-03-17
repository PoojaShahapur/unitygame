using SDK.Lib;

namespace Game.Game
{
    public class GameRouteCB : MsgRouteDispHandle
    {
        public PlayerRouteHandle mPlayerRouteHandle;

        public GameRouteCB()
        {

        }

        override public void init()
        {
            GameRouteHandle gameRouteHandle = new GameRouteHandle();
            gameRouteHandle.init();

            this.addRouteHandle((int)MsgRouteType.eMRT_BASIC, gameRouteHandle, gameRouteHandle.handleMsg);
            
            this.mPlayerRouteHandle = new PlayerRouteHandle();
            this.mPlayerRouteHandle.init();

            this.addRouteHandle((int)MsgRouteType.eMRT_SCENE_COMMAND, this.mPlayerRouteHandle, this.mPlayerRouteHandle.handleMsg);

            SceneRouteHandle sceneRouteHandle = new SceneRouteHandle();
            sceneRouteHandle.init();

            this.addRouteHandle((int)MsgRouteType.eMRT_SCENE_COMMAND, sceneRouteHandle, sceneRouteHandle.handleMsg);
        }

        override public void dispose()
        {

        }
    }
}
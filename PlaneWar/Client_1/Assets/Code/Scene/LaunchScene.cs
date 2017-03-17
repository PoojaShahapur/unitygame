using UnityEngine;
using rpc;
using plane;
using GameBox.Framework;
using GameBox.Service.GiantLightServer;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class LaunchScene : Scene
    {
        private IGiantLightServer mLightServer = null;
        private UILogin uiLogin;
        public override void OnEnter()
        {
            base.OnEnter();

            using (var asset = Game.instance.assetManager.Load("UI/UILogin.prefab", AssetType.PREFAB))
            {
                var obj = GameObject.Instantiate(asset.Cast<GameObject>());
                uiLogin = obj.GetComponent<UILogin>();
                mLightServer = ServiceCenter.GetService<IGiantLightServer>();

                var proxy = mLightServer.CreateProxy("rpc.Login", ServiceType.PULL);
                proxy.Register<LoginResponse>("Login", uiLogin.OnLogin);

                uiLogin.server = mLightServer;
                uiLogin.proxy = proxy;

                var handler = new GiantLightServerHandler(mLightServer);
                handler.OnDisconnectHandler = Game.instance.OnDisconnect;
                Game.instance.handler = handler;
                handler.BeginService("plane.Plane");
                handler.Register<EmptyMsg, EnterRoomResponse>("EnterRoom", uiLogin.OnEnterRoom);
            }

        }

        public override void OnLeave()
        {
            var handler = Game.instance.handler;
            if (handler != null)
                handler.EndService("rpc.Login");
            if (uiLogin != null)
                uiLogin.Close();
            base.OnLeave();
        }

        public void Clear()
        {
        }
    }
}


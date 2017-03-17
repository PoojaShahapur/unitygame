using GameBox.Service.GiantLightServer;
using GameBox.Framework;
using UnityEngine;
using rpc;
using plane;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class Game : MonoBehaviour 
    {
        // Use this for initialization
        static public Game instance { get; private set; }

        public GiantLightServerHandler handler { set; get; }

        public IAssetManager assetManager { get; set; }

        //当前战报[回复或录制]
        private Replay replay;
        public void Start()
        {
            ServiceCenter.RemoteDebug(55055);

            DontDestroyOnLoad(gameObject);
            instance = this;
            GotoScene(new SplashScene());
        }

        public void Disconnect()
        {
            var server = ServiceCenter.GetService<IGiantLightServer>();
            server.Disconnect();
            OnDisconnect();
        }

        public void OnDisconnect()
        {
            GotoScene(new SplashScene());
        }


        // Update is called once per frame
        public void FixedUpdate()
        {
            if (null != mCurrentScene) {
                mCurrentScene.OnUpdate(Time.deltaTime);
            }
        }

        public void OnDestroy()
        {
            if (null != mCurrentScene) {
                mCurrentScene.OnLeave();
            }
        }

        public void GotoScene(Scene scene)
        {
            if (null != mCurrentScene) {
                mCurrentScene.OnLeave();
            }

            mCurrentScene = scene;

            if (null != mCurrentScene) {
                mCurrentScene.OnEnter();
            }
        }

        private Scene mCurrentScene = null;
    }
}

using UnityEngine;
using GameBox.Framework;
using GameBox.Service.GiantLightServer;
using GameBox.Service.AssetManager;
using GameBox.Service.ObjectPool;

namespace Giant
{
    public class SplashScene : Scene
    {
        public GameObject uiConnect;
        public override void OnEnter()
        {
            new ServicesTask(new string[] {
                "com.giant.service.giantlightserver",
                "com.giant.service.assetmanager",
                "com.giant.service.objectpool",
            }).Start().Continue(task =>
            {
                //Singleton.GetInstance<PrefabFactory>().Initialize();
                //Singleton.GetInstance<PrefabFactory>().Preload("Prefabs/star", 100);
                //Singleton.GetInstance<PrefabFactory>().Preload("Prefabs/trangle", 100);
                //Singleton.GetInstance<PrefabFactory>().Preload("Prefabs/bullet", 100);

                Singleton.GetInstance<PoolManager>().CreatePool("Prefabs/star", 100);
                Singleton.GetInstance<PoolManager>().CreatePool("Prefabs/trangle", 100);
                Singleton.GetInstance<PoolManager>().CreatePool("Prefabs/bullet", 100);

                Game.instance.assetManager = ServiceCenter.GetService<IAssetManager>();
                Game.instance.GotoScene(new LaunchScene());

                return null;
            });
        }

        public override void OnLeave()
        {
            ServiceCenter.Launch();
            base.OnLeave();
        }
    }
}


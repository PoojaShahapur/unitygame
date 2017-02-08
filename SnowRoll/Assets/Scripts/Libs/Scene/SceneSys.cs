namespace SDK.Lib
{
    /**
     * @brief 同一时刻只能有一个场景存在
     */
    public class SceneSys
    {
        protected AddOnceAndCallOnceEventDispatch mOnSceneLoadedEventDispatch;

        protected SceneParse mSceneParse;
        protected Scene mScene;
        protected AuxLevelLoader mAuxLevelLoader;

        public SceneSys()
        {
            this.mSceneParse = new SceneParse();
            this.mOnSceneLoadedEventDispatch = new AddOnceAndCallOnceEventDispatch();
            this.mAuxLevelLoader = new AuxLevelLoader();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public Scene getCurScene()
        {
            return this.mScene;
        }

        public bool isSceneLoaded()
        {
            if (null != this.mScene)
            {
                return this.mScene.isSceneLoaded();
            }

            return false;
        }

        public void loadScene(string filename, MAction<IDispatchObject> sceneLoadHandle)
        {
            // 卸载之前的场景
            this.unloadScene();

            // 加载新的场景
            this.mScene = new Scene();
            this.mScene.getSceneCfg().initSize();
            this.mScene.file = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene] + filename;

            if(null != sceneLoadHandle)
            {
                this.mOnSceneLoadedEventDispatch.addEventHandle(null, sceneLoadHandle);
            }
            //loadSceneCfg(filename);
            this.loadSceneRes(filename);
        }

        protected void unloadScene()
        {
            if(null != this.mScene)
            {
                this.mScene.dispose();
                this.mScene = null;
            }
            if(null != this.mAuxLevelLoader)
            {
                this.mAuxLevelLoader.unload();
                this.mAuxLevelLoader = null;
            }
        }

        public void loadSceneCfg(string filename)
        {
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathSceneXml] + filename);
            param.mLoadEventHandle = onSceneCfgLoadded;
            Ctx.mInstance.mResLoadMgr.loadBundle(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        protected void onSceneCfgLoadded(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            this.mSceneParse.sceneCfg = mScene.getSceneCfg();
            string text = res.getText(mScene.file);
            this.mSceneParse.parse(text);
        }

        public void loadSceneRes(string filename)
        {
            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = true;        // 加载场景需要停止处理消息，因为很多资源都要等到场景加载完成才初始化

            if (null == this.mAuxLevelLoader)
            {
                this.mAuxLevelLoader = new AuxLevelLoader();
            }

            this.mAuxLevelLoader.asyncLoad(filename, this.onSceneResLoaded, this.onSceneLoadProgress);
        }

        public void onSceneResLoaded(IDispatchObject dispObj)
        {
            //ResItem res = dispObj as ResItem;
            this.mOnSceneLoadedEventDispatch.dispatchEvent(this.mScene);

            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = false;        // 加载场景完成需要处理处理消息

            this.mAuxLevelLoader.unload();

            this.mScene.init();
        }

        public void onSceneLoadProgress(IDispatchObject dispObj)
        {
            LoadItem item = dispObj as LoadItem;
            float progress = item.getProgress();
            //UnityEngine.Debug.Log(string.Format("aaaaa = {0}", progress));
            Ctx.mInstance.mLuaSystem.onSceneLoadProgress(progress);
        }

        // 卸载多有的场景
        public void unloadAll()
        {
            this.unloadScene();
        }

        // 创建当前场景对应的地图
        public void createTerrain()
        {
            if(this.mScene != null)
            {
                this.mScene.createTerrain();
            }
        }

        public void updateClip()
        {
            this.mScene.updateClip();
        }

        public float getHeightAt(float x, float z)
        {
            if (this.mScene != null)
            {
                return this.mScene.getHeightAt(x, z);
            }

            return 0;
        }

        public UnityEngine.Vector3 adjustPosInRange(UnityEngine.Vector3 pos)
        {
            if(pos.x < 0)
            {
                pos.x = 0;
            }
            else if (pos.x > Ctx.mInstance.mSceneSys.getCurScene().getSceneCfg().getWidth())
            {
                pos.x = Ctx.mInstance.mSceneSys.getCurScene().getSceneCfg().getWidth();
            }

            if (pos.z < 0)
            {
                pos.z = 0;
            }
            else if (pos.z > Ctx.mInstance.mSceneSys.getCurScene().getSceneCfg().getDepth())
            {
                pos.z = Ctx.mInstance.mSceneSys.getCurScene().getSceneCfg().getDepth();
            }

            return pos;
        }
    }
}
namespace SDK.Lib
{
    /**
     * @brief 同一时刻只能有一个场景存在
     */
    public class SceneSys : ITickedObject, IDelayHandleItem
    {
        protected AddOnceAndCallOnceEventDispatch mOnSceneLoadedDisp;

        protected SceneParse mSceneParse;
        protected Scene mScene;
        protected AuxLevelLoader mAuxLevelLoader;

        public SceneSys()
        {
            mSceneParse = new SceneParse();
            mOnSceneLoadedDisp = new AddOnceAndCallOnceEventDispatch();
            mAuxLevelLoader = new AuxLevelLoader();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void onTick(float delta)
        {
            if(null != this.mScene)
            {
                this.mScene.onTick(delta);
            }
        }

        public void setClientDispose()
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public Scene scene
        {
            get
            {
                return mScene;
            }
            set
            {
                mScene = value;
            }
        }

        public void loadScene(string filename, MAction<IDispatchObject> func)
        {
            // 卸载之前的场景
            unloadScene();

            // 加载新的场景
            mScene = new Scene();
            mScene.file = Ctx.mInstance.mCfg.mPathLst[(int)ResPathType.ePathScene] + filename;
            if(func != null)
            {
                mOnSceneLoadedDisp.addEventHandle(null, func);
            }
            //loadSceneCfg(filename);
            loadSceneRes(filename);
        }

        protected void unloadScene()
        {
            if(null != mScene)
            {
                mScene.dispose();
                mScene = null;
            }
            if(null != mAuxLevelLoader)
            {
                mAuxLevelLoader.unload();
                mAuxLevelLoader = null;
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
            mSceneParse.sceneCfg = mScene.sceneCfg;
            string text = res.getText(mScene.file);
            mSceneParse.parse(text);
        }

        public void loadSceneRes(string filename)
        {
            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = true;        // 加载场景需要停止处理消息，因为很多资源都要等到场景加载完成才初始化

            mAuxLevelLoader.asyncLoad(filename, onSceneResLoadded);
        }

        public void onSceneResLoadded(IDispatchObject dispObj)
        {
            //ResItem res = dispObj as ResItem;
            mOnSceneLoadedDisp.dispatchEvent(mScene);

            Ctx.mInstance.mNetCmdNotify.isStopNetHandle = false;        // 加载场景完成需要处理处理消息

            mAuxLevelLoader.unload();

            mScene.init();
        }

        // 卸载多有的场景
        public void unloadAll()
        {
            unloadScene();
        }

        // 创建当前场景对应的地图
        public void createTerrain()
        {
            if(mScene != null)
            {
                mScene.createTerrain();
            }
        }

        public void updateClip()
        {
            mScene.updateClip();
        }

        public float getHeightAt(float x, float z)
        {
            if (mScene != null)
            {
                return mScene.getHeightAt(x, z);
            }

            return 0;
        }
    }
}
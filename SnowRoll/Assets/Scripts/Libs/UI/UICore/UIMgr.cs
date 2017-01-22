using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 所有 UI 管理
     * 1. 对于新创建的Form对象，其所属的层是由其 FormId 决定的
     * 2. UI 设计原则，主要界面是资源创建完成才运行逻辑，小的共享界面是逻辑和资源同时运行，因为 MVC 结构实在是要写很多代码，因此主要界面不适用 MVC 结构
     * 
     * UIMgr 中并没在某个地方缓存 AssetBundle.LoadAsset 加载的 Object ，如果要重复使用 AssetBundle.LoadAsset 加载的 Object ，不要重复使用 ResLoadMgr.load ，要自己找到对应的 Form ，然后获取 Object ，然后再次实例化，因为这种情况使用很少，目前暂时这样使用，如果需要大量重复实例化共享对象，再修改
     */
    public class UIMgr : IResizeObject
	{
		private MDictionary<UIFormId, Form> mId2FormDic; //[id,form]
        private MList<UICanvas> mCanvasList;
        public UIAttrSystem mUIAttrs;

        private MDictionary<UIFormId, UILoadingItem> mId2CodeLoadingItemDic;         // 记录当前代码正在加载的项
        private MDictionary<UIFormId, UILoadingItem> mId2WidgetLoadingItemDic;         // 记录当前窗口控件正在加载的项

        private MList<UIFormId> mTmpList;
        private UniqueNumIdGen mUniqueNumIdGen;

        public UnityEngine.Canvas mHudCanvas;
        public UnityEngine.GameObject mHudParent;


        public UIMgr()
		{
            this.mId2FormDic = new MDictionary<UIFormId, Form>();
            this.mUIAttrs = new UIAttrSystem();
            this.mId2CodeLoadingItemDic = new MDictionary<UIFormId, UILoadingItem>();
            this.mId2WidgetLoadingItemDic = new MDictionary<UIFormId, UILoadingItem>();
            this.mTmpList = new MList<UIFormId>();

            createCanvas();
		}

        public void init()
        {
            mUIAttrs.init();
            this.findCanvasGO();

            this.mHudParent = Ctx.mInstance.mUiMgr.getLayer(UICanvasID.eHudCanvas, UILayerId.eBtmLayer).getLayerGO();
            this.mHudCanvas = Ctx.mInstance.mUiMgr.getCanvas(UICanvasID.eHudCanvas);
        }

        public void dispose()
        {
            
        }

        protected void createCanvas()
        {
            mCanvasList = new MList<UICanvas>();
            int idx = 0;
            for (idx = 0; idx < (int)UICanvasID.eCanvas_Total; ++idx)
            {
                mCanvasList.Add(new UICanvas((UICanvasID)idx));
            }

            mCanvasList[(int)UICanvasID.eFirstCanvas].goName = NotDestroyPath.ND_CV_UIFirstCanvas;
            mCanvasList[(int)UICanvasID.eSecondCanvas].goName = NotDestroyPath.ND_CV_UISecondCanvas;
            mCanvasList[(int)UICanvasID.eHudCanvas].goName = NotDestroyPath.ND_CV_HudCanvas;
        }

        // 关联每一层的对象
        public void findCanvasGO()
        {
            int idx = 0;
            for (idx = 0; idx < (int)UICanvasID.eCanvas_Total; ++idx)
            {
                mCanvasList[idx].findCanvasGO();
            }
        }

        //public void loadAndShow<T>(UIFormId formId) where T : Form, new()
        public void loadAndShow(UIFormId formId)
        {
            if (hasForm(formId))
            {
                showFormInternal(formId);
            }
            else
            {
                loadForm(formId);
            }
        }

        // 显示一个 UI
        public void showForm(UIFormId formId)
		{
            if (hasForm(formId))
            {
                showFormInternal(formId);
            }
		}

        public void showFormInternal(UIFormId formId)
        {
            Form win = getForm(formId);
            if (win != null)
            {
                if (!win.bReady)
                {
                    win.onReady();
                }
                if (!win.IsVisible())
                {
                    UtilApi.SetActive(win.mGuiWin.mUiRoot, true);
                    win.onShow();
                }
            }
        }
		
        // 隐藏一个 UI
        private void hideFormInternal(UIFormId formId)
		{
			Form win = getForm(formId);
			if (win != null)
			{
				if (win.IsVisible())
				{
                    UtilApi.SetActive(win.mGuiWin.mUiRoot, false);
					win.onHide();
				}
			}
		}

        // 退出一个 UI
        public void exitForm(UIFormId formId, bool bForce = false)
		{
			Form win = getForm(formId);

            if (win != null)
			{
                if (win.exitMode || bForce)
                {
                    exitFormInternal(formId);
                }
                else
                {
                    hideFormInternal(formId);
                }
			}
		}

        protected void exitFormInternal(UIFormId formId)
        {
            Form win = getForm(formId);

            if (win != null)
            {
                // 清理列表
                UILayer layer = win.uiLayer;
                layer.winDic.Remove(formId);
                // 释放界面资源
                win.onExit();
                UtilApi.Destroy(win.mGuiWin.mUiRoot);
                win.mGuiWin.mUiRoot = null;
                // 释放加载的资源
                //string path = mUIAttrs.getPath(formId);
                //if (path != null)
                //{
                //    Ctx.mInstance.mResLoadMgr.unload(path);
                //}
                // 很卡，暂时屏蔽掉
                //UtilApi.UnloadUnusedAssets();       // 异步卸载共用资源
                mId2FormDic.Remove(formId);
                win = null;
            }
        }

        public void addForm(Form form)
        {
            addFormNoReady(form);
            form.onInit();
        }

        public UnityEngine.Canvas getCanvas(UICanvasID canvasID)
        {
            UnityEngine.Canvas canvas = null;

            if (canvasID < UICanvasID.eCanvas_Total)
            {
                canvas = mCanvasList[(int)canvasID].getCanvas();
            }

            return canvas;
        }

        public UILayer getLayer(UICanvasID canvasID, UILayerId layerID)
        {
            UILayer layer = null;

            if (canvasID < UICanvasID.eCanvas_Total)
            {
                if (layerID < UILayerId.eMaxLayer)
                {
                    layer = mCanvasList[(int)canvasID].layerList[(int)layerID];
                }
            }

            return layer;
        }

        // 内部接口
        private void addFormNoReady(Form form)
        {
            UILayer layer = getLayer(mUIAttrs.mId2AttrDic[form.id].mCanvasID, mUIAttrs.mId2AttrDic[form.id].mLayerID);
            form.uiLayer = layer;
            layer.addForm(form);

            mId2FormDic[form.id] = form;
            form.init();        // 初始化
        }

        //public T getForm<T>(UIFormId formId) where T : Form
        public Form getForm(UIFormId formId)
        {
            if (mId2FormDic.ContainsKey(formId))
            {
                return mId2FormDic[formId];
            }
            else
            {
                return null;
            }
        }

        public bool hasForm(UIFormId formId)
        {
            return (mId2FormDic.ContainsKey(formId));
        }

        // 这个事加载界面需要的代码
        //public void loadForm<T>(UIFormId formId) where T : Form, new()
        public void loadForm(UIFormId formId)
        {
            UIAttrItem attrItem = mUIAttrs.mId2AttrDic[formId];
            Form window = getForm(formId);

            if (window != null)     // 本地已经创建了这个窗口，
            {
                if (window.IsResReady)      // 如果资源也已经加载进来了
                {
                    if(null != Ctx.mInstance.mCbUIEvent)
                    {
                        Ctx.mInstance.mCbUIEvent.onCodeFormLoaded(window);  // 资源加载完成
                    }
                }
            }
            else if (!mId2CodeLoadingItemDic.ContainsKey(formId))                       // 如果什么都没有创建，第一次加载
            {
                // 创建窗口
                Form form = null;
                if (attrItem.mIsNeedLua)
                {
                    form = new Form();
                }
                else
                {
                    form = Ctx.mInstance.mScriptDynLoad.getScriptObject(attrItem.mScriptTypeName) as Form;
                }
                
                if (form != null)                   // 如果代码已经在本地
                {
                    (form as Form).id = formId;
                    if (attrItem.mIsNeedLua)
                    {
                        form.luaCSBridgeForm = new LuaCSBridgeForm(attrItem, form);
                        form.luaCSBridgeForm.init();
                    }

                    addFormNoReady(form);           // 仅仅是创建数据，资源还没有加载完成
                    // 继续加载资源
                    //if (form.isLoadWidgetRes)
                    //{
                        // 默认会继续加载资源
                        Ctx.mInstance.mUiMgr.loadWidgetRes(formId);
                    //}
                    onCodeLoadedByForm(form);
                }

                // 这个地方应该抛出异常， registerScriptType 没有注册对应的类型
                if (null == form)    // 本地没有代码
                {
                    mId2CodeLoadingItemDic[formId] = new UILoadingItem();
                    mId2CodeLoadingItemDic[formId].mId = formId;

                    loadFromFile(attrItem.mCodePath, onCodeLoadEventHandle);
                }
            }
        }

        // 加载窗口控件资源，窗口资源都是从文件加载
        public void loadWidgetRes(UIFormId formId)
        {
            UIAttrItem attrItem = mUIAttrs.mId2AttrDic[formId];
            if (!mId2WidgetLoadingItemDic.ContainsKey(formId))                       // 如果什么都没有创建，第一次加载
            {
                mId2WidgetLoadingItemDic[formId] = new UILoadingItem();
                mId2WidgetLoadingItemDic[formId].mId = formId;

                loadFromFile(attrItem.mWidgetPath, onWidgetLoadEventHandle);
            }
        }

        // 从本地磁盘或者网络加载资源
        protected void loadFromFile(string reaPath, MAction<IDispatchObject> onLoadEventHandle)
        {
            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(reaPath);
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            param.mLoadEventHandle = onLoadEventHandle;
            Ctx.mInstance.mPrefabMgr.load<PrefabRes>(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }
		
		// 代码资源加载处理
        public void onCodeLoadEventHandle(IDispatchObject dispObj)
		{
            PrefabRes res = dispObj as PrefabRes;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                onCodeloadedByRes(res);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                UIFormId formId = mUIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathCodePath);  // 获取 FormID
                mId2CodeLoadingItemDic.Remove(formId);
            }
		}

        // 窗口控件资源加载处理
        public void onWidgetLoadEventHandle(IDispatchObject dispObj)
        {
            PrefabRes res = dispObj as PrefabRes;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                onWidgetloadedByRes(res);
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                UIFormId formId = mUIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathComUI);  // 获取 FormID
                mId2WidgetLoadingItemDic.Remove(formId);
            }
        }

        // 代码资源加载完成处理
        public void onCodeloadedByRes(PrefabRes res)
        {
            UIFormId ID = mUIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathCodePath);  // 获取 FormID
            mId2CodeLoadingItemDic.Remove(ID);
            addFormNoReady(mId2FormDic[ID]);
            onCodeLoadedByForm(mId2FormDic[ID]);
        }

        protected void onCodeLoadedByForm(Form form)
        {
            if (null != Ctx.mInstance.mCbUIEvent)
            {
                Ctx.mInstance.mCbUIEvent.onCodeFormLoaded(form);  // 资源加载完成
            }
        }

        // 窗口控件资源加载完成处理
        public void onWidgetloadedByRes(PrefabRes res)
        {
            string resUniqueId = res.getResUniqueId();
            string path = res.getLogicPath();
            UIFormId formId = mUIAttrs.GetFormIDByPath(path, ResPathType.ePathComUI);  // 获取 FormID
            mId2WidgetLoadingItemDic.Remove(formId);

            UIAttrItem attrItem = mUIAttrs.mId2AttrDic[formId];
            mId2FormDic[formId].isLoadWidgetRes = true;
            mId2FormDic[formId].mGuiWin.mUiRoot = res.InstantiateObject(attrItem.mWidgetPath, false,  UtilMath.ZeroVec3, UtilMath.UnitQuat);
            if (attrItem.mIsNeedLua)
            {
                mId2FormDic[formId].luaCSBridgeForm.gameObject = mId2FormDic[formId].mGuiWin.mUiRoot;
                mId2FormDic[formId].luaCSBridgeForm.postInit();
            }

            // 设置位置
            UtilApi.SetParent(mId2FormDic[formId].mGuiWin.mUiRoot.transform, mCanvasList[(int)attrItem.mCanvasID].layerList[(int)attrItem.mLayerID].layerTrans, false);

            // 先设置再设置缩放，否则无效
            mId2FormDic[formId].mGuiWin.mUiRoot.transform.SetAsLastSibling();               // 放在最后
            UtilApi.SetActive(mId2FormDic[formId].mGuiWin.mUiRoot, false);      // 出发 onShow 事件
            //if (m_dicForm[ID].hideOnCreate)
            //{
            //    UtilApi.SetActive(m_dicForm[ID].mGuiWin.mUiRoot, false);
            //}
            if (!mId2FormDic[formId].hideOnCreate)
            {
                showFormInternal(formId);   // 如果 onShow 中调用 exit 函数，就会清掉 m_dicForm 中的内容。如果设置了 exitMode = false，就不会清掉 m_dicForm ，就不会有问题
            }

            if (null != Ctx.mInstance.mCbUIEvent)
            {
                if (mId2FormDic.ContainsKey(formId))      // 如果 onShow 中调用 exit 函数，并且没有设置 exitMode = false ，就会清除 m_dicForm， 这个时候再调用这个函数，就会有问题，是不是添加延迟卸载
                {
                    Ctx.mInstance.mCbUIEvent.onWidgetLoaded(mId2FormDic[formId]);  // 资源加载完成
                }
            }

            // 卸载资源
            Ctx.mInstance.mPrefabMgr.unload(resUniqueId, onWidgetLoadEventHandle);
        }

        // 大小发生变化后，调用此函数
        public void onResize(int viewWidth, int viewHeight)
        {
            int canvasIdx = 0;
            int layerIdx = 0;
            for(canvasIdx = 0; canvasIdx < (int)UICanvasID.eCanvas_Total; ++canvasIdx)
            {
                for (layerIdx = 0; layerIdx < (int)UILayerId.eMaxLayer; ++layerIdx)
                {
                    mCanvasList[canvasIdx].layerList[layerIdx].onStageReSize();
                }
            }
        }

        // 关闭所有显示的窗口
        public void exitAllWin()
        {
            foreach(UIFormId id in mId2FormDic.Keys)
            {
                mTmpList.Add(id);
            }

            foreach (UIFormId id in mTmpList.list())
            {
                exitForm(id);
            }
            mTmpList.Clear();
        }

        // 根据场景类型卸载 UI，强制卸载
        public void unloadUIBySceneType(UISceneType unloadSceneType, UISceneType loadSceneTpe)
        {
            foreach (UIFormId id in mId2FormDic.Keys)
            {
                if (mUIAttrs.mId2AttrDic[id].canUnloadUIBySceneType(unloadSceneType, loadSceneTpe))
                {
                    mTmpList.Add(id);
                }
            }

            foreach (UIFormId id in mTmpList.list())
            {
                exitForm(id, true);
            }
            mTmpList.Clear();
        }
	}
}
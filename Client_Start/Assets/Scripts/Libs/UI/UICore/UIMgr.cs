using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 所有 UI 管理
     * 1. 对于新创建的Form对象，其所属的层是由其ID决定的
     * 2. UI 设计原则，主要界面是资源创建完成才运行逻辑，小的共享界面是逻辑和资源同时运行，因为 MVC 结构实在是要写很多代码，因此主要界面不适用 MVC 结构
     * 
     * UIMgr 中并没在某个地方缓存 AssetBundle.LoadAsset 加载的 Object ，如果要重复使用 AssetBundle.LoadAsset 加载的 Object ，不要重复使用 ResLoadMgr.load ，要自己找到对应的 Form ，然后获取 Object ，然后再次实例化，因为这种情况使用很少，目前暂时这样使用，如果需要大量重复实例化共享对象，再修改
     */
    public class UIMgr : IResizeObject
	{
		private Dictionary<UIFormID, Form> m_id2FormDic; //[id,form]
        private List<UICanvas> m_canvasList;
        public UIAttrSystem m_UIAttrs;

        private Dictionary<UIFormID, UILoadingItem> m_ID2CodeLoadingItemDic;         // 记录当前代码正在加载的项
        private Dictionary<UIFormID, UILoadingItem> m_ID2WidgetLoadingItemDic;         // 记录当前窗口控件正在加载的项

        private List<UIFormID> m_tmpList;

		public UIMgr()
		{
            m_id2FormDic = new Dictionary<UIFormID, Form>();
            m_UIAttrs = new UIAttrSystem();
            m_ID2CodeLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
            m_ID2WidgetLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
            m_tmpList = new List<UIFormID>();

            createCanvas();
		}

        public void init()
        {
            m_UIAttrs.init();
        }

        protected void createCanvas()
        {
            m_canvasList = new List<UICanvas>();
            int idx = 0;
            for (idx = 0; idx < (int)UICanvasID.eCanvas_Total; ++idx)
            {
                m_canvasList.Add(new UICanvas((UICanvasID)idx));
            }

            m_canvasList[(int)UICanvasID.eCanvas_50].goName = NotDestroyPath.ND_CV_UICanvas_50;
            m_canvasList[(int)UICanvasID.eCanvas_100].goName = NotDestroyPath.ND_CV_UICanvas_100;
        }

        // 关联每一层的对象
        public void findCanvasGO()
        {
            int idx = 0;
            for (idx = 0; idx < (int)UICanvasID.eCanvas_Total; ++idx)
            {
                m_canvasList[idx].findCanvasGO();
            }
        }

        //public void loadAndShow<T>(UIFormID ID) where T : Form, new()
        public void loadAndShow(UIFormID ID)
        {
            if (hasForm(ID))
            {
                showFormInternal(ID);
            }
            else
            {
                loadForm(ID);
            }
        }

        // 显示一个 UI
        public void showForm(UIFormID ID)
		{
            if (hasForm(ID))
            {
                showFormInternal(ID);
            }
		}

        public void showFormInternal(UIFormID ID)
        {
            Form win = getForm(ID);
            if (win != null)
            {
                if (!win.bReady)
                {
                    win.onReady();
                }
                if (!win.IsVisible())
                {
                    UtilApi.SetActive(win.m_guiWin.m_uiRoot, true);
                    win.onShow();
                }
            }
        }
		
        // 隐藏一个 UI
        private void hideFormInternal(UIFormID ID)
		{
			Form win = getForm(ID);
			if (win != null)
			{
				if (win.IsVisible())
				{
                    UtilApi.SetActive(win.m_guiWin.m_uiRoot, false);
					win.onHide();
				}
			}
		}

        // 退出一个 UI
        public void exitForm(UIFormID ID, bool bForce = false)
		{
			Form win = getForm(ID);

            if (win != null)
			{
                if (win.exitMode || bForce)
                {
                    exitFormInternal(ID);
                }
                else
                {
                    hideFormInternal(ID);
                }
			}
		}

        protected void exitFormInternal(UIFormID ID)
        {
            Form win = getForm(ID);

            if (win != null)
            {
                // 清理列表
                UILayer layer = win.uiLayer;
                layer.winDic.Remove(ID);
                // 释放界面资源
                win.onExit();
                UtilApi.Destroy(win.m_guiWin.m_uiRoot);
                win.m_guiWin.m_uiRoot = null;
                // 释放加载的资源
                //string path = m_UIAttrs.getPath(ID);
                //if (path != null)
                //{
                //    Ctx.m_instance.m_resLoadMgr.unload(path);
                //}
                UtilApi.UnloadUnusedAssets();       // 异步卸载共用资源
                m_id2FormDic.Remove(ID);
                win = null;
            }
        }

        public void addForm(Form form)
        {
            addFormNoReady(form);
            form.onInit();
        }

        public UILayer getLayer(UICanvasID canvasID, UILayerID layerID)
        {
            UILayer layer = null;

            if (UICanvasID.eCanvas_50 <= canvasID && canvasID <= UICanvasID.eCanvas_100)
            {
                if (UILayerID.eBtmLayer <= layerID && layerID <= UILayerID.eTopLayer)
                {
                    layer = m_canvasList[(int)canvasID].layerList[(int)layerID];
                }
            }

            return layer;
        }

        // 内部接口
        private void addFormNoReady(Form form)
        {
            UILayer layer = getLayer(m_UIAttrs.m_id2AttrDic[form.id].m_canvasID, m_UIAttrs.m_id2AttrDic[form.id].m_LayerID);
            form.uiLayer = layer;
            layer.addForm(form);

            m_id2FormDic[form.id] = form;
            form.init();        // 初始化
        }

        //public T getForm<T>(UIFormID ID) where T : Form
        public Form getForm(UIFormID ID)
        {
            if (m_id2FormDic.ContainsKey(ID))
            {
                return m_id2FormDic[ID];
            }
            else
            {
                return null;
            }
        }

        public bool hasForm(UIFormID ID)
        {
            return (m_id2FormDic.ContainsKey(ID));
        }

        // 这个事加载界面需要的代码
        //public void loadForm<T>(UIFormID ID) where T : Form, new()
        public void loadForm(UIFormID ID)
        {
            UIAttrItem attrItem = m_UIAttrs.m_id2AttrDic[ID];
            Form window = getForm(ID);

            if (window != null)     // 本地已经创建了这个窗口，
            {
                if (window.IsResReady)      // 如果资源也已经加载进来了
                {
                    if(null != Ctx.m_instance.m_cbUIEvent)
                    {
                        Ctx.m_instance.m_cbUIEvent.onCodeFormLoaded(window);  // 资源加载完成
                    }
                }
            }
            else if (!m_ID2CodeLoadingItemDic.ContainsKey(ID))                       // 如果什么都没有创建，第一次加载
            {
                // 创建窗口
                Form form = null;
                if (attrItem.m_bNeedLua)
                {
                    form = new Form();
                }
                else
                {
                    form = Ctx.m_instance.m_scriptDynLoad.getScriptObject(attrItem.m_scriptTypeName) as Form;
                }
                
                if (form != null)                   // 如果代码已经在本地
                {
                    (form as Form).id = ID;
                    if (attrItem.m_bNeedLua)
                    {
                        form.luaCSBridgeForm = new LuaCSBridgeForm(attrItem, form);
                        form.luaCSBridgeForm.init();
                    }

                    addFormNoReady(form);           // 仅仅是创建数据，资源还没有加载完成
                    onCodeLoadedByForm(form);
                }

                // 这个地方应该抛出异常
                if(null == form)    // 本地没有代码
                {
                    m_ID2CodeLoadingItemDic[ID] = new UILoadingItem();
                    m_ID2CodeLoadingItemDic[ID].m_ID = ID;

                    loadFromFile(attrItem.m_codePath, onCodeLoadEventHandle);
                }
            }
        }

        // 加载窗口控件资源，窗口资源都是从文件加载
        public void loadWidgetRes(UIFormID ID)
        {
            UIAttrItem attrItem = m_UIAttrs.m_id2AttrDic[ID];
            if (!m_ID2WidgetLoadingItemDic.ContainsKey(ID))                       // 如果什么都没有创建，第一次加载
            {
                m_ID2WidgetLoadingItemDic[ID] = new UILoadingItem();
                m_ID2WidgetLoadingItemDic[ID].m_ID = ID;

                loadFromFile(attrItem.m_widgetPath, onWidgetLoadEventHandle);
            }
        }

        // 从本地磁盘或者网络加载资源
        protected void loadFromFile(string reaPath, MAction<IDispatchObject> onLoadEventHandle)
        {
            LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
            param.setPath(reaPath);
            param.m_loadNeedCoroutine = false;
            param.m_resNeedCoroutine = false;
            param.m_loadEventHandle = onLoadEventHandle;
            Ctx.m_instance.m_prefabMgr.load<PrefabRes>(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
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
                UIFormID ID = m_UIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathCodePath);  // 获取 FormID
                m_ID2CodeLoadingItemDic.Remove(ID);
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
                UIFormID ID = m_UIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathComUI);  // 获取 FormID
                m_ID2WidgetLoadingItemDic.Remove(ID);
                Ctx.m_instance.m_logSys.log("UIFormID =  ， Failed Prefab");
            }
        }

        // 代码资源加载完成处理
        public void onCodeloadedByRes(PrefabRes res)
        {
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.getLogicPath(), ResPathType.ePathCodePath);  // 获取 FormID
            m_ID2CodeLoadingItemDic.Remove(ID);
            addFormNoReady(m_id2FormDic[ID]);
            onCodeLoadedByForm(m_id2FormDic[ID]);
        }

        protected void onCodeLoadedByForm(Form form)
        {
            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                Ctx.m_instance.m_cbUIEvent.onCodeFormLoaded(form);  // 资源加载完成
            }
        }

        // 窗口控件资源加载完成处理
        public void onWidgetloadedByRes(PrefabRes res)
        {
            string resUniqueId = res.getResUniqueId();
            string path = res.getLogicPath();
            UIFormID ID = m_UIAttrs.GetFormIDByPath(path, ResPathType.ePathComUI);  // 获取 FormID
            m_ID2WidgetLoadingItemDic.Remove(ID);

            UIAttrItem attrItem = m_UIAttrs.m_id2AttrDic[ID];
            m_id2FormDic[ID].bLoadWidgetRes = true;
            m_id2FormDic[ID].m_guiWin.m_uiRoot = res.InstantiateObject(attrItem.m_widgetPath);
            if (attrItem.m_bNeedLua)
            {
                m_id2FormDic[ID].luaCSBridgeForm.gameObject = m_id2FormDic[ID].m_guiWin.m_uiRoot;
                m_id2FormDic[ID].luaCSBridgeForm.postInit();
            }

            // 设置位置
            UtilApi.SetParent(m_id2FormDic[ID].m_guiWin.m_uiRoot.transform, m_canvasList[(int)attrItem.m_canvasID].layerList[(int)attrItem.m_LayerID].layerTrans, false);

            // 先设置再设置缩放，否则无效
            m_id2FormDic[ID].m_guiWin.m_uiRoot.transform.SetAsLastSibling();               // 放在最后
            UtilApi.SetActive(m_id2FormDic[ID].m_guiWin.m_uiRoot, false);      // 出发 onShow 事件
            //if (m_dicForm[ID].hideOnCreate)
            //{
            //    UtilApi.SetActive(m_dicForm[ID].m_guiWin.m_uiRoot, false);
            //}
            if (!m_id2FormDic[ID].hideOnCreate)
            {
                showFormInternal(ID);   // 如果 onShow 中调用 exit 函数，就会清掉 m_dicForm 中的内容。如果设置了 exitMode = false，就不会清掉 m_dicForm ，就不会有问题
            }

            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                if (m_id2FormDic.ContainsKey(ID))      // 如果 onShow 中调用 exit 函数，并且没有设置 exitMode = false ，就会清除 m_dicForm， 这个时候再调用这个函数，就会有问题，是不是添加延迟卸载
                {
                    Ctx.m_instance.m_cbUIEvent.onWidgetLoaded(m_id2FormDic[ID]);  // 资源加载完成
                }
            }

            // 卸载资源
            Ctx.m_instance.m_prefabMgr.unload(resUniqueId, onWidgetLoadEventHandle);
        }

        // 大小发生变化后，调用此函数
        public void onResize(int viewWidth, int viewHeight)
        {
            int canvasIdx = 0;
            int layerIdx = 0;
            for(canvasIdx = 0; canvasIdx < (int)UICanvasID.eCanvas_Total; ++canvasIdx)
            {
                for (layerIdx = 0; layerIdx <= (int)UILayerID.eMaxLayer; ++layerIdx)
                {
                    m_canvasList[canvasIdx].layerList[layerIdx].onStageReSize();
                }
            }
        }

        // 关闭所有显示的窗口
        public void exitAllWin()
        {
            foreach(UIFormID id in m_id2FormDic.Keys)
            {
                m_tmpList.Add(id);
            }

            foreach (UIFormID id in m_tmpList)
            {
                exitForm(id);
            }
            m_tmpList.Clear();
        }

        // 根据场景类型卸载 UI，强制卸载
        public void unloadUIBySceneType(UISceneType unloadSceneType, UISceneType loadSceneTpe)
        {
            foreach (UIFormID id in m_id2FormDic.Keys)
            {
                if (m_UIAttrs.m_id2AttrDic[id].canUnloadUIBySceneType(unloadSceneType, loadSceneTpe))
                {
                    m_tmpList.Add(id);
                }
            }

            foreach (UIFormID id in m_tmpList)
            {
                exitForm(id, true);
            }
            m_tmpList.Clear();
        }
	}
}
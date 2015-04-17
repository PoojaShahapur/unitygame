using SDK.Lib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
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
		private Dictionary<UIFormID, Form> m_dicForm = new Dictionary<UIFormID,Form>(); //[id,form]
		private List<UILayer> m_vecLayer;
        public UIAttrs m_UIAttrs = new UIAttrs();

        private Dictionary<UIFormID, UILoadingItem> m_ID2CodeLoadingItemDic;         // 记录当前代码正在加载的项
        private Dictionary<UIFormID, UILoadingItem> m_ID2WidgetLoadingItemDic;         // 记录当前窗口控件正在加载的项

        private List<UIFormID> m_tmpList = new List<UIFormID>();

		public UIMgr()
		{
            m_vecLayer = new List<UILayer>();
            m_vecLayer.Add(new UILayer(UILayerID.BtmLayer));
            m_vecLayer.Add(new UILayer(UILayerID.FirstLayer));
            m_vecLayer.Add(new UILayer(UILayerID.SecondLayer));
            m_vecLayer.Add(new UILayer(UILayerID.ThirdLayer));
            m_vecLayer.Add(new UILayer(UILayerID.ForthLayer));
            m_vecLayer.Add(new UILayer(UILayerID.TopLayer));

            m_ID2CodeLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
            m_ID2WidgetLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
		}

        // 关联每一层的对象
        public void getLayerGameObject()
        {
            m_vecLayer[(int)UILayerID.BtmLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIBtmLayer].transform;
            m_vecLayer[(int)UILayerID.FirstLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer].transform;
            m_vecLayer[(int)UILayerID.SecondLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UISecondLayer].transform;
            m_vecLayer[(int)UILayerID.ThirdLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIThirdLayer].transform;
            m_vecLayer[(int)UILayerID.ForthLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIForthLayer].transform;
            m_vecLayer[(int)UILayerID.TopLayer].layerTrans = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UITopLayer].transform;
        }

        public void loadAndShow<T>(UIFormID ID) where T : Form, new()
        {
            if (hasForm(ID))
            {
                showFormInternal(ID);
            }
            else
            {
                loadForm<T>(ID);
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
            Form win = getForm<Form>(ID);
            if (win != null)
            {
                //UILayer layer = win.uiLayer;
                if (!win.bReady)
                {
                    win.onReady();
                }
                if (!win.IsVisible())
                {
                    UtilApi.SetActive(win.m_GUIWin.m_uiRoot, true);
                    win.onShow();
                }
            }
        }
		
        // 隐藏一个 UI
        private void hideFormInternal(UIFormID ID)
		{
			Form win = getForm<Form>(ID);
			if (win != null)
			{
				//UILayer layer = win.uiLayer;
				if (win.IsVisible())
				{
                    UtilApi.SetActive(win.m_GUIWin.m_uiRoot, false);
					win.onHide();
				}
			}
		}

        // 退出一个 UI
        public void exitForm(UIFormID ID, bool bForce = false)
		{
			Form win = getForm<Form>(ID);

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
            Form win = getForm<Form>(ID);

            if (win != null)
            {
                // 清理列表
                UILayer layer = win.uiLayer;
                layer.winDic.Remove(ID);
                // 释放界面资源
                win.onExit();
                UtilApi.Destroy(win.m_GUIWin.m_uiRoot);
                // 释放加载的资源
                //string path = m_UIAttrs.getPath(ID);
                //if (path != null)
                //{
                //    Ctx.m_instance.m_resLoadMgr.unload(path);
                //}
                UtilApi.UnloadUnusedAssets();       // 异步卸载共用资源
                m_dicForm.Remove(ID);
                win = null;
            }
        }

        public void addForm(Form form)
        {
            addFormNoReady(form);
            form.onInit();
        }

        public UILayer getLayer(UILayerID layerID)
        {
            UILayer layer = null;

            if (layerID >= UILayerID.BtmLayer && layerID <= UILayerID.MaxLayer)
            {
                layer = m_vecLayer[(int)layerID];
            }
            return layer;
        }

        // 内部接口
        private void addFormNoReady(Form form)
        {
            UILayer layer = getLayer(m_UIAttrs.m_dicAttr[form.id].m_LayerID);
            form.uiLayer = layer;
            layer.addForm(form);
            m_dicForm[form.id] = form;
            form.init();        // 初始化
        }

        public T getForm<T>(UIFormID ID) where T : Form
        {
            if (m_dicForm.ContainsKey(ID))
            {
                return m_dicForm[ID] as T;
            }
            else
            {
                return null;
            }
        }

        public bool hasForm(UIFormID ID)
        {
            return (m_dicForm.ContainsKey(ID));
        }

        // 这个事加载界面需要的代码
        public void loadForm<T>(UIFormID ID) where T : Form, new()
        {
            // exitAllWin();                       // 关闭所有的界面

            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
            Form window = getForm<Form>(ID);

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
                form = new T();
                if (form != null)                   // 如果代码已经在本地
                {
                    (form as Form).id = ID;
                    addFormNoReady(form);           // 仅仅是创建数据，资源还没有加载完成
                    onCodeLoadedByForm(form);
                }

                // 这个地方应该抛出异常
                if(null == form)    // 本地没有代码
                {
                    m_ID2CodeLoadingItemDic[ID] = new UILoadingItem();
                    m_ID2CodeLoadingItemDic[ID].m_ID = ID;

                    loadFromFile(attrItem.m_codePath, onCodeloaded, onCodeFailed, onCodeloadedByRes);
                }
            }
        }

        // 加载窗口控件资源，窗口资源都是从文件加载
        public void loadWidgetRes(UIFormID ID)
        {
            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
            if (!m_ID2WidgetLoadingItemDic.ContainsKey(ID))                       // 如果什么都没有创建，第一次加载
            {
                m_ID2WidgetLoadingItemDic[ID] = new UILoadingItem();
                m_ID2WidgetLoadingItemDic[ID].m_ID = ID;

                loadFromFile(attrItem.m_widgetPath, onWidgetloaded, onWidgetFailed, onWidgetloadedByRes);
            }
        }

        // 从本地磁盘或者网络加载资源
        protected void loadFromFile(string reaPath, Action<IDispatchObject> onLoaded, Action<IDispatchObject> onFailed, Action<IResItem> onloadedAndInit)
        {
            // 创建窗口资源
            IResItem res = Ctx.m_instance.m_resLoadMgr.getResource(reaPath);
            if (res != null)
            {
                if (!res.HasLoaded())
                {
                    // 添加事件监听,不用增加引用计数
                    res.addEventListener(EventID.LOADED_EVENT, onLoaded);
                    res.addEventListener(EventID.FAILED_EVENT, onFailed);
                }
                else // 已经加载完成
                {
                    onloadedAndInit(res);
                }
            }
            else // 资源从来没有加载过
            {
                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = Ctx.m_instance.m_pPakSys.getCurResPakPathByResPath(reaPath);
                param.m_loaded = onLoaded;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }
		
		// 代码资源加载成功
        public void onCodeloaded(IDispatchObject resEvt)
		{
            IResItem res = resEvt as IResItem;                         // 类型转换
            onCodeloadedByRes(res);
		}

        // 代码资源加载失败
        private void onCodeFailed(IDispatchObject resEvt)
		{
            IResItem res = resEvt as IResItem;                         // 类型转换
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), ResPathType.ePathCodePath);  // 获取 FormID
            m_ID2CodeLoadingItemDic.Remove(ID);
		}

        // 窗口控件资源加载成功
        public void onWidgetloaded(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            onWidgetloadedByRes(res);
        }

        // 窗口控件资源加载失败
        private void onWidgetFailed(IDispatchObject resEvt)
        {
            IResItem res = resEvt as IResItem;                         // 类型转换
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), ResPathType.ePathComUI);  // 获取 FormID
            m_ID2WidgetLoadingItemDic.Remove(ID);
        }

        // 代码资源加载完成处理
        public void onCodeloadedByRes(IResItem res)
        {
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), ResPathType.ePathCodePath);  // 获取 FormID
            m_ID2CodeLoadingItemDic.Remove(ID);
            addFormNoReady(m_dicForm[ID]);
            onCodeLoadedByForm(m_dicForm[ID]);
        }

        protected void onCodeLoadedByForm(Form form)
        {
            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                Ctx.m_instance.m_cbUIEvent.onCodeFormLoaded(form);  // 资源加载完成
            }
        }

        // 窗口控件资源加载完成处理
        public void onWidgetloadedByRes(IResItem res)
        {
            string path = res.GetPath();
            UIFormID ID = m_UIAttrs.GetFormIDByPath(path, ResPathType.ePathComUI);  // 获取 FormID
            m_ID2WidgetLoadingItemDic.Remove(ID);

            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
            m_dicForm[ID].bLoadWidgetRes = true;
            m_dicForm[ID].m_GUIWin.m_uiRoot = res.InstantiateObject(attrItem.m_widgetPath);
            // 设置位置
            //m_dicForm[ID].m_GUIWin.m_uiRoot.transform.parent = m_vecLayer[(int)attrItem.m_LayerID].layerTrans;
            UtilApi.SetParent(m_dicForm[ID].m_GUIWin.m_uiRoot.transform, m_vecLayer[(int)attrItem.m_LayerID].layerTrans, false);
            // 先设置再设置缩放，否则无效
            //m_dicForm[ID].m_GUIWin.m_uiRoot.transform.localPosition = Vector3.zero;
            //m_dicForm[ID].m_GUIWin.m_uiRoot.transform.localScale = Vector3.one;
            m_dicForm[ID].m_GUIWin.m_uiRoot.transform.SetAsLastSibling();               // 放在最后
            UtilApi.SetActive(m_dicForm[ID].m_GUIWin.m_uiRoot, false);      // 出发 onShow 事件
            //if (m_dicForm[ID].hideOnCreate)
            //{
            //    UtilApi.SetActive(m_dicForm[ID].m_GUIWin.m_uiRoot, false);
            //}
            if (!m_dicForm[ID].hideOnCreate)
            {
                showFormInternal(ID);
            }

            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                Ctx.m_instance.m_cbUIEvent.onWidgetLoaded(m_dicForm[ID]);  // 资源加载完成
            }

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(path);
        }

        // 大小发生变化后，调用此函数
        public void onResize(int viewWidth, int viewHeight)
        {
            int index;
            for (index = 0; index <= (int)UILayerID.MaxLayer; index++)
            {
                m_vecLayer[index].onStageReSize();
            }
        }

        // 关闭所有显示的窗口
        public void exitAllWin()
        {
            foreach(UIFormID id in m_dicForm.Keys)
            {
                m_tmpList.Add(id);
            }

            foreach (UIFormID id in m_tmpList)
            {
                exitForm(id);
            }
            m_tmpList.Clear();
        }
	}
}
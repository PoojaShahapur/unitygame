using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
	/**
	 * @brief 所有 UI 管理
	 * 1. 对于新创建的Form对象，其所属的层是由其ID决定的
     * 2. UI 设计原则，主要界面是资源创建完成才运行逻辑，小的共享界面是逻辑和资源同时运行，因为 MVC 结构实在是要写很多代码，因此主要界面不适用 MVC 结构
	 */
	public class UIMgr : IUIMgr, IResizeObject
	{
		private Dictionary<UIFormID, Form> m_dicForm = new Dictionary<UIFormID,Form>(); //[id,form]
		private List<UILayer> m_vecLayer;
        public UIAttrs m_UIAttrs = new UIAttrs();
        public IUIFactory m_IUIFactory;

        private Dictionary<UIFormID, UILoadingItem> m_ID2CodeLoadingItemDic;         // 记录当前代码正在加载的项
        private Dictionary<UIFormID, UILoadingItem> m_ID2WidgetLoadingItemDic;         // 记录当前窗口控件正在加载的项

		public UIMgr()
		{
            m_vecLayer = new List<UILayer>();
            m_vecLayer.Add(new UILayer(UILayerID.FirstLayer));
            m_ID2CodeLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
            m_ID2WidgetLoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
		}

		public void showForm(UIFormID ID)
		{
			Form win = getForm(ID);
			if (win != null)
			{
				UILayer layer = win.uiLayer;
				if (!win.bReady)
				{
                    win.onReady();
				}
                if (!win.IsVisible())
                {
                    win.m_GUIWin.m_uiRoot.SetActive(true);
                    win.onShow();
                }
			}
		}
		
		public void hideForm(UIFormID ID)
		{
			Form win = getForm(ID);
			if (win != null)
			{
				UILayer layer = win.uiLayer;
				if (win.IsVisible())
				{
                    win.m_GUIWin.m_uiRoot.SetActive(false);
					win.onHide();
				}
			}
		}

        public void exitForm(UIFormID ID)
		{
			Form win = getForm(ID);
			
			if (win != null)
			{
                // 清理列表
				UILayer layer = win.uiLayer;
				layer.winDic.Remove(ID);
                // 释放界面资源
                win.onExit();
                UtilApi.DestroyImmediate(win.m_GUIWin.m_uiRoot);
                // 释放加载的资源
				string path = m_UIAttrs.getPath(ID);
				if (path != null)
				{
					Ctx.m_instance.m_resMgr.unload(path);
				}
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

            if (layerID >= UILayerID.FirstLayer && layerID <= UILayerID.MaxLayer)
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
        }

        public Form getForm(UIFormID ID)
        {
            if (m_dicForm.ContainsKey(ID))
            {
                return m_dicForm[ID];
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

        // 加载并且显示
        public void LoadAndShowForm(UIFormID ID)
        {
            if (hasForm(ID))
            {
                showForm(ID);
            }
            else
            {
                loadForm(ID);
            }
        }

        // 这个事加载界面需要的代码
        public void loadForm(UIFormID ID)
        {
            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
            Form window = getForm(ID);

            if (window != null)     // 本地已经创建了这个窗口，
            {
                if (window.IsResReady)      // 如果资源也已经加载进来了
                {
                    if(null != Ctx.m_instance.m_cbUIEvent)
                    {
                        Ctx.m_instance.m_cbUIEvent.onWidgetLoaded(window);  // 资源加载完成
                    }
                }
            }
            else if (!m_ID2CodeLoadingItemDic.ContainsKey(ID))                       // 如果什么都没有创建，第一次加载
            {
                // 创建窗口
                IForm form = null;
                if (m_IUIFactory != null)                   // 如果本地有代码，就直接创建
                {
                    form = m_IUIFactory.CreateForm(ID);
                    if (form != null)                   // 如果代码已经在本地
                    {
                        (form as Form).id = ID;
                        addFormNoReady(form as Form);           // 仅仅是创建数据，资源还没有加载完成
                        onCodeLoadedByForm(form as Form);
                    }
                }

                if(null == form)    // 本地没有代码
                {
                    m_ID2CodeLoadingItemDic[ID] = new UILoadingItem();
                    m_ID2CodeLoadingItemDic[ID].m_ID = ID;

                    loadFromFile(attrItem.m_codePath, attrItem.m_codePrefabName, onCodeloaded, onCodeFailed, onCodeloadedByRes);
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

                loadFromFile(attrItem.m_widgetPath, attrItem.m_widgetPrefabName, onWidgetloaded, onWidgetFailed, onWidgetloadedByRes);
            }
        }

        // 从本地磁盘或者网络加载资源
        protected void loadFromFile(string reaPath, string prefabName, Action<EventDisp> onloaded, Action<EventDisp> onFailed, Action<IRes> onloadedAndInit)
        {
            // 创建窗口资源
            IRes res = Ctx.m_instance.m_resMgr.getResource(reaPath);
            if (res != null)
            {
                if (!res.HasLoaded())
                {
                    // 添加事件监听,不用增加引用计数
                    res.addEventListener(EventID.LOADED_EVENT, onloaded);
                    res.addEventListener(EventID.FAILED_EVENT, onFailed);
                }
                else // 已经加载完成
                {
                    onloadedAndInit(res);
                }
            }
            else // 资源从来没有加载过
            {
                LoadParam param = Ctx.m_instance.m_resMgr.getLoadParam();
                param.m_path = reaPath;
                //param.m_resPackType = ResPackType.eBundleType;
                //param.m_resLoadType = ResLoadType.eLoadDicWeb;
                //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
                param.m_prefabName = prefabName;
                param.m_loadedcb = onloaded;
                //param.m_resNeedCoroutine = false;
                //param.m_loadNeedCoroutine = true;
                //Ctx.m_instance.m_resMgr.load(param);
                //Ctx.m_instance.m_resMgr.loadBundle(param);
                Ctx.m_instance.m_resMgr.loadResources(param);
            }
        }
		
		// 代码资源加载成功
        public void onCodeloaded(EventDisp resEvt)
		{
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            onCodeloadedByRes(res);
		}

        // 代码资源加载失败
        private void onCodeFailed(EventDisp resEvt)
		{

		}

        // 窗口控件资源加载成功
        public void onWidgetloaded(EventDisp resEvt)
        {
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            onWidgetloadedByRes(res);
        }

        // 窗口控件资源加载失败
        private void onWidgetFailed(EventDisp resEvt)
        {

        }

        // 代码资源加载完成处理
        public void onCodeloadedByRes(IRes res)
        {
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), ResPathType.ePathCodePath);  // 获取 FormID
            onCodeLoadedByForm(m_dicForm[ID]);
        }

        protected void onCodeLoadedByForm(Form form)
        {
            form.onInit();
            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                Ctx.m_instance.m_cbUIEvent.onCodeFormLoaded(form);  // 资源加载完成
            }
        }

        // 窗口控件资源加载完成处理
        public void onWidgetloadedByRes(IRes res)
        {
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), ResPathType.ePathComUI);  // 获取 FormID

            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
            m_dicForm[ID].m_GUIWin.m_uiRoot = res.InstantiateObject(attrItem.m_codePrefabName);
            // 设置位置
            m_dicForm[ID].m_GUIWin.m_uiRoot.transform.parent = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UIFirstLayer].transform;
            // 先设置再设置缩放，否则无效
            m_dicForm[ID].m_GUIWin.m_uiRoot.transform.localPosition = Vector3.zero;
            m_dicForm[ID].m_GUIWin.m_uiRoot.transform.localScale = Vector3.one;

            if (null != Ctx.m_instance.m_cbUIEvent)
            {
                Ctx.m_instance.m_cbUIEvent.onWidgetLoaded(m_dicForm[ID]);  // 资源加载完成
            }
        }

        public void SetIUIFactory(IUIFactory value)
        {
            m_IUIFactory = value;
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
	}
}
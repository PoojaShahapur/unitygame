using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
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
        private Dictionary<UIFormID, UILoadingItem> m_ID2LoadingItemDic;         // 记录当前正在加载的项

		public UIMgr()
		{
            m_vecLayer = new List<UILayer>();
            m_vecLayer.Add(new UILayer(UILayerID.FirstLayer));
            m_ID2LoadingItemDic = new Dictionary<UIFormID, UILoadingItem>();
		}

        public void SetIUIFactory(IUIFactory value)
        {
            m_IUIFactory = value;
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
		
		public void addForm(Form form)
		{
            addFormNoReady(form);
			form.onReady();
		}

        // 内部接口
        private void addFormNoReady(Form form)
        {
            UILayer layer = getLayer(m_UIAttrs.m_dicAttr[form.id].m_LayerID);
            form.uiLayer = layer;
            m_dicForm[form.id] = form;
        }
		
		public void loadForm(UIFormID ID)
		{
			//string path = m_UIAttrs.getPath(ID);
            UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
			Form window = getForm(ID);
			
			if (window != null)     // 本地已经创建了这个窗口，
			{
                if (window.IsResReady)      // 如果资源也已经加载进来了
                {
                    Action<IForm> loadedFun = Ctx.m_instance.m_cbUIEvent.getLoadedFunc(ID);
                    if (loadedFun != null)
                    {
                        loadedFun(window);
                    }
                }
			}
            else if(!m_ID2LoadingItemDic.ContainsKey(ID))                       // 如果什么都没有创建，第一次加载
			{
                m_ID2LoadingItemDic[ID] = new UILoadingItem();
                m_ID2LoadingItemDic[ID].m_ID = ID;
                // 创建窗口
                IForm form = null;
                if (m_IUIFactory != null)
                {
                    form = m_IUIFactory.CreateForm(ID);
                    if (form != null)                   // 如果代码已经在本地
                    {
                        addFormNoReady(form as Form);           // 仅仅是创建数据，资源还没有加载完成
                        m_ID2LoadingItemDic[ID].m_logicLoaded = true;
                    }
                }

                // 创建窗口资源
                IRes res = Ctx.m_instance.m_resMgr.getResource(attrItem.m_resPath);
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
						onloadedByRes(res);
					}
				}
				else // 资源从来没有加载过
				{
                    LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
                    param.m_path = attrItem.m_resPath;
                    //param.m_resPackType = ResPackType.eBundleType;
                    //param.m_resLoadType = ResLoadType.eLoadDicWeb;
                    //param.m_resLoadType = Ctx.m_instance.m_cfg.m_resLoadType;
                    param.m_prefabName = attrItem.m_prefabName;
                    param.m_loadedcb = onloaded;
                    //param.m_resNeedCoroutine = false;
                    //param.m_loadNeedCoroutine = true;
                    //Ctx.m_instance.m_resMgr.load(param);
                    Ctx.m_instance.m_resMgr.loadBundle(param);
				}
			}
		}
		
		public Form getForm(UIFormID ID)
		{
            //return m_dicForm[ID];
            if(m_dicForm.ContainsKey(ID))
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

		public void showFormWidthProgress(UIFormID ID)
		{
			Form form = this.getForm(ID);
			if (form != null)
			{
				form.show();
				return;
			}
		}

		public void showForm(UIFormID ID)
		{
			Form win = getForm(ID);
			if (win != null)
			{
				UILayer layer = win.uiLayer;
				if (win.isInitiated)
				{
					if (win.IsVisible())
					{
						win.onShow();
					}
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
					win.onHide();
				}
			}
		}
		
		//关闭界面
		public void exitForm(UIFormID ID)
		{
			Form win = getForm(ID);
			if (win != null)
			{
				UILayer layer = win.uiLayer;
				win.exit();
			}
		}
		
		public void destroyForm(UIFormID ID)
		{
			Form win = getForm(ID);
			
			if (win != null)
			{
				UILayer layer = win.uiLayer;

				if (win.IsVisible())
				{
					win.onHide();
				}
				
				win.onDestroy();
				win.dispose();				
				layer.winDic.Remove(ID);
				string path = m_UIAttrs.getPath(ID);
				if (path != null)
				{
					Ctx.m_instance.m_resMgr.unload(path);
				}
				m_dicForm.Remove(ID);
				win = null;
			}
		}
		
		// 资源加载成功，通过事件回调
        public void onloaded(Event resEvt)
		{
            IRes res = resEvt.m_param as IRes;                         // 类型转换
            onloadedByRes(res);
		}
		
		// 资源加载失败，通过事件回调
        private void onFailed(Event resEvt)
		{

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

        public void onloadedByRes(IRes res)
        {
            ResPathType pathType = UtilRes.ConvPath3Type(res.GetPath());
            UIFormID ID = m_UIAttrs.GetFormIDByPath(res.GetPath(), pathType);  // 获取 FormID

            if (ResPathType.ePathComUI == pathType)
            {
                m_ID2LoadingItemDic[ID].m_resLoaded = true;
            }
            else if (ResPathType.ePathCodePath == pathType)
            {
                m_ID2LoadingItemDic[ID].m_logicLoaded = true;
            }

            if (m_ID2LoadingItemDic[ID].IsLoaded())
            {
                UIAttrItem attrItem = m_UIAttrs.m_dicAttr[ID];
                Action<IForm> loadedFun = Ctx.m_instance.m_cbUIEvent.getLoadedFunc(ID);
                m_dicForm[ID].m_GUIWin.m_uiRoot = res.InstantiateObject(attrItem.m_prefabName);
                if (loadedFun != null)
                {
                    loadedFun(m_dicForm[ID]);
                }
            }
        }
	}
}
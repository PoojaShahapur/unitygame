using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
	/**
	 * @brief 所有 UI 管理
	 * 1. 对于新创建的Form对象，其所属的层是由其ID决定的
	 */
	public class UIMgr : IUIMgr, IResizeObject
	{
		private Dictionary<UIFormID, Form> m_dicForm = new Dictionary<UIFormID,Form>(); //[id,form]
		private List<UILayer> m_vecLayer;
        public UIAttrs m_UIAttrs;
        public IUIFactory m_IUIFactory;
		
		public UIMgr()
		{
            m_vecLayer = new List<UILayer>();
            m_vecLayer.Add(new UILayer(UILayerID.FirstLayer));
			Ctx.m_instance.m_ResizeMgr.addResizeObject(this);
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
			UILayer layer = getLayer(m_UIAttrs.m_dicAttr[form.id].m_LayerID);
			form.uiLayer = layer;
			m_dicForm[form.id] = form;
			form.onReady();
		}
		
		public void loadForm(UIFormID ID)
		{
			string path = m_UIAttrs.getPath(ID);
			Form window = getForm(ID);
			
			if (window != null)
			{
                Action<IForm> loadedFun = Ctx.m_instance.m_cbUIEvent.getLoadedFunc(ID);
				if (loadedFun != null)
				{
					loadedFun(window);
				}
			}
			else
			{
                // 创建窗口
                IForm form = m_IUIFactory.CreateForm(ID);
                addForm(form as Form);

                // 创建窗口资源
				IRes res = Ctx.m_instance.m_resMgr.getResource(path);
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
                    param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathUI] + "UIScrollForm.unity3d";
                    param.m_type = ResPackType.eBundleType;
                    param.m_prefabName = "UIScrollForm";
                    //param.m_cb = onloaded;
                    param.m_resNeedCoroutine = false;
                    param.m_loadNeedCoroutine = true;
                    Ctx.m_instance.m_resMgr.load(param);
				}
			}
		}
		
		public Form getForm(UIFormID ID)
		{
			return m_dicForm[ID];
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

        }
	}
}
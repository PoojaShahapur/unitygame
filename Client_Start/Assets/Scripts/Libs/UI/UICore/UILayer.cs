using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public enum UILayerID
    {
        eBtmLayer,                   // 最低层啊，一般不放东西，以备不时之需，目前放模糊的界面
        eFirstLayer,                 // 第一层，聊天之类的主界面窗口
        eSecondLayer,                // 第二层，主要是功能性的界面，弹出需要关闭的界面
        eThirdLayer,                 // 第三层，新手引导层
        eForthLayer,                 // 第四层，提示窗口层
        eTopLayer,                   // 最顶层，一般不放东西，以备不时之需

        eMaxLayer
    }

    /**
     * @brief 描述一个 UI 层
     */
	public class UILayer 
	{
        private UILayerID m_layer;	                        // UIFormID.FirstLayer定义
        private Transform m_layerTrans;                     // 当前所在层根节点转换
        private Dictionary<UIFormID, Form> m_winDic;        // 当前层所有的界面
        protected string m_goName;

        public UILayer(UILayerID layer) 
		{
			m_layer = layer;
            m_winDic = new Dictionary<UIFormID, Form>();
		}

        public Dictionary<UIFormID, Form> winDic
		{
            get
            {
			    return m_winDic;
            }
		}

        public Transform layerTrans
        {
            get
            {
                return m_layerTrans;
            }
            set
            {
                m_layerTrans = value;
            }
        }

        public string goName
        {
            set
            {
                m_goName = value;
            }
        }

        public void getLayerGO()
        {

        }
		
		public bool hasForm(Form form)
		{
			return m_winDic.ContainsKey(form.id);
		}
		
		public void removeForm(Form form)
		{
            if (m_winDic.ContainsKey(form.id))
			{
                m_winDic.Remove(form.id);
			}
		}

        public UILayerID layerID
		{
            get
            {
			    return m_layer;
            }
		}
		
		public void addForm(Form form)
		{
			m_winDic[form.id] = form;
		}

		public void onStageReSize()
		{
			foreach (Form form in m_winDic.Values)
			{
				form.onStageReSize();
			}
		}

		public void closeAllForm()
		{
            foreach (Form form in m_winDic.Values)
			{
			    form.exit();
			}
		}

        public void findLayerGOAndTrans()
        {
            m_layerTrans = Ctx.mInstance.mLayerMgr.m_path2Go[m_goName].transform;
        }
	}
}
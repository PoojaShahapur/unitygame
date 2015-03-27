using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
	public class UILayer 
	{
        private UILayerID m_layer;	                        // UIFormID.FirstLayer定义
        private Transform m_layerTrans;                     // 当前所在层根节点转换
        private Dictionary<UIFormID, Form> m_winDic;        // 当前层所有的界面

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
	}
}
using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
	public class UILayer 
	{
        private UILayerID m_layer;	//UIFormID.FirstLayer定义
		private DeskTop m_deskTop;
        private Dictionary<UIFormID, Form> m_winDic;

        public UILayer(UILayerID layer) 
		{
			m_layer = layer;
			m_deskTop = new DeskTop();
            m_winDic = new Dictionary<UIFormID, Form>();
		}
		public DeskTop deskTop
		{
            get
            {
			    return m_deskTop;
            }
		}

        public Dictionary<UIFormID, Form> winDic
		{
            get
            {
			    return m_winDic;
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
		
		public void addForm(Form form, bool bShow)
		{
			m_winDic[form.id] = form;
			if (bShow)
			{
                form.m_GUIWin.m_uiRoot.SetActive(bShow);
			}
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
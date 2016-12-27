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
        private UILayerID mLayer;	                        // UIFormID.FirstLayer定义
        private Transform mLayerTrans;                     // 当前所在层根节点转换
        private MDictionary<UIFormID, Form> mWinDic;        // 当前层所有的界面
        protected string mGoName;

        public UILayer(UILayerID layer) 
		{
			mLayer = layer;
            mWinDic = new MDictionary<UIFormID, Form>();
		}

        public MDictionary<UIFormID, Form> winDic
		{
            get
            {
			    return mWinDic;
            }
		}

        public Transform layerTrans
        {
            get
            {
                return mLayerTrans;
            }
            set
            {
                mLayerTrans = value;
            }
        }

        public string goName
        {
            set
            {
                mGoName = value;
            }
        }

        public GameObject getLayerGO()
        {
            return mLayerTrans.gameObject;
        }
		
		public bool hasForm(Form form)
		{
			return mWinDic.ContainsKey(form.id);
		}
		
		public void removeForm(Form form)
		{
            if (mWinDic.ContainsKey(form.id))
			{
                mWinDic.Remove(form.id);
			}
		}

        public UILayerID layerID
		{
            get
            {
			    return mLayer;
            }
		}
		
		public void addForm(Form form)
		{
			mWinDic[form.id] = form;
		}

		public void onStageReSize()
		{
			foreach (Form form in mWinDic.Values)
			{
				form.onStageReSize();
			}
		}

		public void closeAllForm()
		{
            foreach (Form form in mWinDic.Values)
			{
			    form.exit();
			}
		}

        public void findLayerGOAndTrans()
        {
            mLayerTrans = Ctx.mInstance.mLayerMgr.mPath2Go[mGoName].transform;
        }
	}
}
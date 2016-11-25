using System.Collections.Generic;

namespace SDK.Lib
{
    public class XmlCfgMgr
    {
        public Dictionary<XmlCfgID, XmlCfgBase> mId2CfgDic;        // 商城
        private ResItem mRes;

        public XmlCfgMgr()
        {
            mId2CfgDic = new Dictionary<XmlCfgID, XmlCfgBase>();
        }

        protected void loadCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            T item = new T();
            mId2CfgDic[id] = item;

            LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
            param.setPath(item.mPath);
            param.mLoadEventHandle = onLoadEventHandle;
            param.mLoadNeedCoroutine = false;
            param.mResNeedCoroutine = false;
            Ctx.mInstance.mResLoadMgr.loadAsset(param);
            Ctx.mInstance.mPoolSys.deleteObj(param);
        }

        // 加载一个表完成
        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            mRes = dispObj as ResItem;
            if (mRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, mRes.getLoadPath());

                string text = mRes.getText("");
                if (text != null)
                {
                    mId2CfgDic[getXmlCfgIDByPath(mRes.getLogicPath())].parseXml(text);
                }
            }
            else if (mRes.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, mRes.getLoadPath());
            }

            Ctx.mInstance.mResLoadMgr.unload(mRes.getResUniqueId(), onLoadEventHandle);
        }

        protected XmlCfgID getXmlCfgIDByPath(string path)
        {
            foreach (KeyValuePair<XmlCfgID, XmlCfgBase> kv in mId2CfgDic)
            {
                if (kv.Value.mPath == path)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        public T getXmlCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            if (!mId2CfgDic.ContainsKey(id))
            {
                loadCfg<T>(id);
            }

            return mId2CfgDic[id] as T;
        }
    }
}
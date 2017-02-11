using System.Collections.Generic;

namespace SDK.Lib
{
    public class XmlCfgMgr
    {
        public MDictionary<XmlCfgID, XmlCfgBase> mId2CfgDic;
        private ResItem mRes;

        public XmlCfgMgr()
        {
            this.mId2CfgDic = new MDictionary<XmlCfgID, XmlCfgBase>();
        }

        protected void loadCfg<T>(XmlCfgID id) where T : XmlCfgBase, new()
        {
            T item = new T();
            this.mId2CfgDic[id] = item;

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
            this.mRes = dispObj as ResItem;

            if (this.mRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, this.mRes.getLoadPath());

                string text = this.mRes.getText("");

                if (text != null)
                {
                    this.mId2CfgDic[getXmlCfgIDByPath(this.mRes.getLogicPath())].parseXml(text);
                }
            }
            else if (this.mRes.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, this.mRes.getLoadPath());
            }

            Ctx.mInstance.mResLoadMgr.unload(this.mRes.getResUniqueId(), this.onLoadEventHandle);
        }

        protected XmlCfgID getXmlCfgIDByPath(string path)
        {
            foreach (KeyValuePair<XmlCfgID, XmlCfgBase> kv in this.mId2CfgDic)
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
            if (!this.mId2CfgDic.ContainsKey(id))
            {
                this.loadCfg<T>(id);
            }

            return this.mId2CfgDic[id] as T;
        }
    }
}
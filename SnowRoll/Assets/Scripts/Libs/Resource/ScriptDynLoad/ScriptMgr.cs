namespace SDK.Lib
{
    public class ScriptDynLoad : InsResMgrBase
    {
        public void registerScriptType(string path, System.Type scriptType)
        {
            mPath2ResDic[path] = new ScriptRes();
            (mPath2ResDic[path] as ScriptRes).scriptType = scriptType;
        }

        public System.Object getScriptObject(string path)
        {
            System.Object ret = null;
            if(mPath2ResDic.ContainsKey(path))
            {
                ret = (mPath2ResDic[path] as ScriptRes).createInstance();
            }
            return ret;
        }
    }
}
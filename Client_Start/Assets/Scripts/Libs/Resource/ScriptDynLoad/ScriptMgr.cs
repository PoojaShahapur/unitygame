namespace SDK.Lib
{
    public class ScriptDynLoad : InsResMgrBase
    {
        public void registerScriptType(string path, System.Type scriptType)
        {
            m_path2ResDic[path] = new ScriptRes();
            (m_path2ResDic[path] as ScriptRes).scriptType = scriptType;
        }

        public System.Object getScriptObject(string path)
        {
            System.Object ret = null;
            if(m_path2ResDic.ContainsKey(path))
            {
                ret = (m_path2ResDic[path] as ScriptRes).createInstance();
            }
            return ret;
        }
    }
}
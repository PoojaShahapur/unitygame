namespace SDK.Lib
{
    public class ScriptDynLoad : ResMgrBase
    {
        public void registerScriptType(string path, System.Type scriptType)
        {
            m_path2ResDic[path] = new ScriptRes();
            (m_path2ResDic[path] as ScriptRes).scriptType = scriptType;
        }

        public System.Object getScriptObject(string path)
        {
            return (m_path2ResDic[path] as ScriptRes).createInstance();
        }
    }
}
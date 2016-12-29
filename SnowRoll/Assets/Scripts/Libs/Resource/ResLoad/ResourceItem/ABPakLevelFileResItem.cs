namespace SDK.Lib
{
    /**
     * @brief 打包的 Level stream 系统
     */
    public class ABPakLevelFileResItem : ABPakFileResItemBase
    {
        protected string mLevelName = "";

        public string levelName
        {
            set
            {
                mLevelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            base.init(item);
            // 继续加载当前的关卡
            //loadLevel(mOrigPath);
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        //public void loadLevel(string resName)
        //{
        //    // 获取打包后的 unity3d 资源名字
        //    string unity3dName = Ctx.mInstance.mPakSys.path2PakDic[resName].m_unity3dName;
        //    if (!m_path2UnPakRes.ContainsKey(unity3dName))
        //    {
        //        m_path2UnPakRes[unity3dName] = new ABUnPakLevelFileResItem();
        //        m_path2UnPakRes[unity3dName].copyFrom(this);
        //        // 资源转换成，每一个单独的资源目录信息
        //        m_path2UnPakRes[unity3dName].path = resName;
        //        m_path2UnPakRes[unity3dName].extName = resName.Substring(resName.IndexOf('.') + 1);
        //        (m_path2UnPakRes[unity3dName] as ABUnPakLevelFileResItem).levelName = UtilApi.convScenePath2LevelName(resName);
        //        // 暂时不支持异步实例化
        //        m_path2UnPakRes[unity3dName].clearListener();
        //        m_path2UnPakRes[unity3dName].resNeedCoroutine = false;
        //        m_path2UnPakRes[unity3dName].initByBytes(getBytes(unity3dName), ABUnPakLevelFileResItem.SCENE_PRE_PATH);
        //    }
        //}
    }
}
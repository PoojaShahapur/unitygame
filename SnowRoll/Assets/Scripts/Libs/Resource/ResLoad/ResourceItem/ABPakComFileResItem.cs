using System.IO;

namespace SDK.Lib
{
    /**
     * @brief Asset Bundles 打包的普通文件资源
     */
    public class ABPakComFileResItem : ABPakFileResItemBase
    {
        public FileStream m_fs = null;      // 文件句柄

        override public void init(LoadItem item)
        {
            base.init(item);
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        //override public GameObject InstantiateObject(string resName)
        //{
        //    ABUnPakFileResItemBase unPakRes = loadRes(resName);
        //    return unPakRes.InstantiateObject(resName);
        //}

        //public override ABUnPakFileResItemBase loadRes(string resName)
        //{
        //    // 获取打包后的 unity3d 资源名字
        //    string unity3dName = Ctx.mInstance.mPakSys.path2PakDic[resName].m_unity3dName;
        //    if (!m_path2UnPakRes.ContainsKey(unity3dName))
        //    {
        //        m_path2UnPakRes[unity3dName] = new ABUnPakComFileResItem();
        //        //m_path2UnPakRes[unity3dName].copyFrom(this);
        //        // 资源转换成，每一个单独的资源目录信息
        //        m_path2UnPakRes[unity3dName].path = resName;
        //        m_path2UnPakRes[unity3dName].extName = resName.Substring(resName.IndexOf('.') + 1);
        //        // 暂时不支持异步实例化
        //        m_path2UnPakRes[unity3dName].clearListener();
        //        m_path2UnPakRes[unity3dName].resNeedCoroutine = false;
        //        m_path2UnPakRes[unity3dName].initByBytes(getBytes(unity3dName), PRE_PATH);
        //    }

        //    m_path2UnPakRes[unity3dName].increaseRef();

        //    return m_path2UnPakRes[unity3dName];
        //}

        //public void unloadRes(string path)
        //{
        //    string unity3dName = Ctx.mInstance.mPakSys.path2PakDic[path].m_unity3dName;
        //    if (m_path2UnPakRes.ContainsKey(unity3dName))
        //    {
        //        m_path2UnPakRes[unity3dName].decreaseRef();
        //        if (m_path2UnPakRes[unity3dName].bNoRef())
        //        {
        //            unloadNoRef(unity3dName);
        //        }
        //    }
        //}

        //// 不考虑引用计数，直接卸载
        //public void unloadNoRef(string path)
        //{
        //    if (m_path2UnPakRes.ContainsKey(path))
        //    {
        //        m_path2UnPakRes[path].unload();
        //        m_path2UnPakRes.Remove(path);
        //    }
        //    else
        //    {
        //        
        //    }
        //}
    }
}
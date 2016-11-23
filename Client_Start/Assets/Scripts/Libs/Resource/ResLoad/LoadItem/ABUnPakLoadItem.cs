using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class ABUnPakLoadItem : LoadItem
    {
        public byte[] m_bytes;

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        override public void load()
        {
            base.load();

            string curExt = "";

            // 如果是打包的资源
            if (Ctx.mInstance.mCfg.mPakExtNameList.IndexOf(mExtName) != -1)
            {
                curExt = FileResItem.UNITY3D_EXT;
            }
            else if(string.IsNullOrEmpty(mExtName))        // 材质 mat 源文件是没有扩展名字的，因为 3dmax 材质扩展名字也是 mat ，可能 unity 怕识别错误了吧
            {
                curExt = FileResItem.UNITY3D_EXT;
            }
            else
            {
                curExt = mExtName;
            }

            string curPath;
            curPath = Path.Combine(Application.streamingAssetsPath, mLoadPath);
            curPath = UtilLogic.getPakPathAndExt(curPath, curExt);

            if (UtilPath.existFile(curPath))
            {
                MDataStream mDataStream = new MDataStream(curPath);
                m_bytes = mDataStream.readByte();
                mDataStream.dispose();
                mDataStream = null;
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("{0} 文件不存在", curPath));
            }

            if (m_bytes != null)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
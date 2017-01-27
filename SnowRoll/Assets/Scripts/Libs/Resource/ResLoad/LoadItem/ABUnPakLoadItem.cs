using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class ABUnPakLoadItem : LoadItem
    {
        public byte[] mBytes;

        override public void reset()
        {
            base.reset();
            mBytes = null;
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
                mBytes = mDataStream.readByte();
                mDataStream.dispose();
                mDataStream = null;
            }
            else
            {
                
            }

            if (mBytes != null)
            {
                mNonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mNonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            mNonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
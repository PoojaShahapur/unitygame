using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class PrefabResBase : InsResBase
    {
        public GameObject mGo;
        public GameObject mRetGO;

        public PrefabResBase()
        {

        }

        override protected void initImpl(ResItem res)
        {
            mGo = res.getObject(res.getPrefabName()) as GameObject;

            base.initImpl(res);
        }

        public GameObject InstantiateObject(string resName, bool isSetInitOrientPos, Vector3 position, Quaternion rotation)
        {
            mRetGO = null;

            if (null == mGo)
            {
                Ctx.mInstance.mLogSys.log("Actor default object is null", LogTypeId.eLogCommon);
            }
            else
            {
                if (isSetInitOrientPos)
                {
                    mRetGO = GameObject.Instantiate(mGo, position, rotation) as GameObject;
                }
                else
                {
                    mRetGO = GameObject.Instantiate(mGo) as GameObject;
                }

                if (null == mRetGO)
                {
                    Ctx.mInstance.mLogSys.log("Actor instance object is null", LogTypeId.eLogCommon);
                }
            }

            return mRetGO;
        }

        public void InstantiateObject(string resName, bool isSetInitOrientPos, Vector3 position, Quaternion rotation,  ResInsEventDispatch evtHandle)
        {
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(asyncInstantiateObject(resName, isSetInitOrientPos, position, rotation, evtHandle));
        }

        public IEnumerator asyncInstantiateObject(string resName, bool isSetInitOrientPos, Vector3 position, Quaternion rotation, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == mGo)
            {
                Ctx.mInstance.mLogSys.log("Actor default object is null", LogTypeId.eLogCommon);
            }
            else
            {
                if (isSetInitOrientPos)
                {
                    retGO = GameObject.Instantiate(mGo, position, rotation) as GameObject;
                }
                else
                {
                    retGO = GameObject.Instantiate(mGo) as GameObject;
                }

                if (null == retGO)
                {
                    Ctx.mInstance.mLogSys.log("Actor instance object is null", LogTypeId.eLogCommon);
                }
            }

            yield return null;

            evtHandle.setInsGO(retGO);
            evtHandle.dispatchEvent(evtHandle);
        }

        public GameObject getObject()
        {
            return mGo;
        }

        public override void unload()
        {
            if (mGo != null)
            {
                // 一定要先设置 null，然后再调用 UnloadUnusedAssets ，否则删除不了 Resources 管理器中的 Asset-Object 资源
                mGo = null;

                UtilApi.UnloadUnusedAssets();

                //UtilApi.UnloadAsset(mGo);      // 强制卸载资源数据
                //UtilApi.DestroyImmediate(mGo, true); // 这个会删除磁盘上的资源
                //UtilApi.UnloadUnusedAssets();   // 卸载的时候， AssetBundles 完全卸载掉 AssetBundles 资源，因为现在使用 AssetBundles::Unload(true); 一次性卸载所有的资源，这次这里就不再调用 UnloadUnusedAssets 这个资源了 
            }

            mRetGO = null;

            base.unload();
        }
    }
}
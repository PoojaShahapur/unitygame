using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        public UICamera mUiCam;            // 这个不是 UI 相机，这个是场景相机

        protected MCamera mLocalCamera;         // 这个是系统摄像机，主要进行裁剪使用的
        protected Camera mMainCamera;          // 主相机
        protected Camera mUguiCam;             // UGUI 相机
        //protected ThirdCameraController mCameraController; // 摄像机控制器
        protected RoateCameraController mCameraController;
        protected CameraMan mCameraMan;        // 摄像机玩家
        protected bool mIsFirst;

        public CamSys()
        {
            mIsFirst = true;
        }

        // 初始化
        public void init()
        {
            
        }

        // 析构
        public void dispose()
        {

        }

        public MCamera getLocalCamera()
        {
            return mLocalCamera;
        }

        public void setLocalCamera(Camera cam)
        {
            //mLocalCamera = new MCamera(cam.gameObject.transform);
            mLocalCamera = new MOctreeCamera("OctreeCamera", Ctx.mInstance.mSceneManager, cam.gameObject.transform);
            if (cam.orthographic)
            {
                mLocalCamera.setProjectionType(ProjectionType.PT_ORTHOGRAPHIC);
                mLocalCamera.setOrthoWindow(cam.orthographicSize * 2, cam.orthographicSize * 2);
            }
            else
            {
                mLocalCamera.setProjectionType(ProjectionType.PT_PERSPECTIVE);
                mLocalCamera.setFOVy(new MRadian(UtilMath.DegreesToRadians(cam.fieldOfView)));
                mLocalCamera.setFarClipDistance(cam.farClipPlane);
                mLocalCamera.setNearClipDistance(cam.nearClipPlane);
                mLocalCamera.setAspectRatio(cam.aspect);

                mLocalCamera.testClipPlane();
                testAABB();
            }

            //testCameraCull();
            //testFrustumDir();
        }

        public void setSceneCamera2UICamera()
        {
            mUiCam.mCam = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_UICamera].GetComponent<Camera>();
        }

        public void setSceneCamera2MainCamera()
        {
            mUiCam.mCam = null;
        }

        public Camera getMainCamera()
        {
            return mMainCamera;
        }

        public void setMainCamera(Camera camera)
        {
            mMainCamera = camera;
        }

        public Camera getUGuiCamera()
        {
            return mUguiCam;
        }

        public void setUGuiCamera(Camera camera)
        {
            mUguiCam = camera;
        }

        // 设置摄像机 Man Actor
        public void setCameraActor(SceneEntityBase actor)
        {
            if (mCameraController == null)
            {
                //mCameraController = new ThirdCameraController(mMainCamera, go);
                mCameraController = new RoateCameraController(mMainCamera, actor.gameObject(), actor);
                mCameraController.init();
            }
            else
            {
                mCameraController.setTarget(actor.gameObject());
            }

            if (mCameraMan == null)
            {
                //mCameraMan = new CameraMan(go);
                mCameraMan = new TerrainCameraMan(actor.gameObject());
                mCameraMan.setCameraController(mCameraController);
            }
            else
            {
                mCameraMan.setActor(actor.gameObject());
            }
        }

        public void setCameraActor(GameObject go)
        {
            if (mCameraController == null)
            {
                //mCameraController = new ThirdCameraController(mMainCamera, go);
                mCameraController = new RoateCameraController(mMainCamera, go, null);
                mCameraController.init();
            }
            else
            {
                mCameraController.setTarget(go);
            }

            if (mCameraMan == null)
            {
                //mCameraMan = new CameraMan(go);
                mCameraMan = new TerrainCameraMan(go);
                mCameraMan.setCameraController(mCameraController);
            }
            else
            {
                mCameraMan.setActor(go);
            }
        }

        public MPlane[] getFrustumPlanes()
        {
            return mLocalCamera.getFrustumPlanes();
        }

        public void invalidCamera()
        {
            if (mLocalCamera != null)
            {
                mLocalCamera.invalid();
                //mLocalCamera.updateTmpPosOrient();
                if (Ctx.mInstance.mTerrainGlobalOption.mNeedCull)
                {
                    //Ctx.mInstance.mSceneManager.cullScene();
                    Ctx.mInstance.mSceneManager.runUpdateTask();
                    //testFrustumDir();

                    // 如果是第一次， Tree 刚把 TreeNode 添加到场景管理器中，需要再次更新才能裁剪，才能显示，不是第一次就不用更新了，因为移动会很小，不会有太大问题
                    if (mIsFirst)
                    {
                        mIsFirst = false;
                        //Ctx.mInstance.mSceneManager.cullScene();
                        Ctx.mInstance.mSceneManager.runUpdateTask();
                    }
                }
            }
        }

        protected void testAABB()
        {
            MAxisAlignedBox aabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            aabb.setMinimum(new MVector3(187.5f, -10, 0));
            aabb.setMaximum(new MVector3(375, 10, 187.5f));

            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            if (mLocalCamera.isVisible(ref aabb, ref plane))
            {
                mLocalCamera.isVisible(ref aabb, ref plane);
            }
        }

        protected void testCameraCull()
        {
            UtilApi.setPos(mLocalCamera.getTrans(), new Vector3(780, 41, 620));

            MAxisAlignedBox aabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            aabb.setMaximum(937.5f, -206.5529f, 1312.5f);
            aabb.setMinimum(750, -270.7412f, 1125);
            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            mLocalCamera.isVisible(ref aabb, ref plane);
            Debug.Log("aaa");
        }

        public void testFrustumDir()
        {
            Transform trans = mLocalCamera.getTrans();
            GameObject cube = UtilApi.TransFindChildByPObjAndPath(trans.gameObject, "Cube");
            MVector3 pos = MVector3.fromNative(cube.transform.position);
            FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
            if(!mLocalCamera.isVisible(ref pos, ref plane))
            {
                Debug.Log("Error");
            }
        }
    }
}
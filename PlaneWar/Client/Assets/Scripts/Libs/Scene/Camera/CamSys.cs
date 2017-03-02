using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        protected UICamera mUiCam;            // 这个不是 UI 相机，这个是场景相机

        protected MCamera mLocalCamera;         // 这个是系统摄像机，主要进行裁剪使用的
        public Camera mMainCamera;          // 主相机
        public Camera mUguiCam;             // UGUI 相机
        //protected ThirdCameraController mCameraController; // 摄像机控制器
        protected RoateCameraController mCameraController;
        protected CameraMan mCameraMan;        // 摄像机玩家
        protected bool mIsFirst;
        protected bool mIsHudCanvasNeedWorldCam;    // HUD 是否需要世界相机

        public CamSys()
        {
            this.mIsFirst = true;
            this.mIsHudCanvasNeedWorldCam = false;
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
            return this.mLocalCamera;
        }

        public void setLocalCamera(Camera cam)
        {
            //mLocalCamera = new MCamera(cam.gameObject.transform);
            this.mLocalCamera = new MOctreeCamera("OctreeCamera", Ctx.mInstance.mSceneManager, cam.gameObject.transform);
            if (cam.orthographic)
            {
                this.mLocalCamera.setProjectionType(ProjectionType.PT_ORTHOGRAPHIC);
                this.mLocalCamera.setOrthoWindow(cam.orthographicSize * 2, cam.orthographicSize * 2);
            }
            else
            {
                this.mLocalCamera.setProjectionType(ProjectionType.PT_PERSPECTIVE);
                this.mLocalCamera.setFOVy(new MRadian(UtilMath.DegreesToRadians(cam.fieldOfView)));
                this.mLocalCamera.setFarClipDistance(cam.farClipPlane);
                this.mLocalCamera.setNearClipDistance(cam.nearClipPlane);
                this.mLocalCamera.setAspectRatio(cam.aspect);

                this.mLocalCamera.testClipPlane();
                this.testAABB();
            }

            //testCameraCull();
            //testFrustumDir();
        }

        public void setUiCamera(UICamera camera)
        {
            this.mUiCam = camera;
        }

        public void setSceneCamera2UICamera()
        {
            this.mUiCam.mCam = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_UICamera].GetComponent<Camera>();

            this.setUGuiCamera(mUiCam.mCam);
        }

        public void setSceneCamera2MainCamera()
        {
            this.mUiCam.mCam = null;
        }

        public Camera getMainCamera()
        {
            return this.mMainCamera;
        }

        public void setMainCamera(Camera camera)
        {
            this.mMainCamera = camera;

            if(this.mIsHudCanvasNeedWorldCam)
            {
                UtilApi.setCanvasCam(Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_HudCanvas], this.mMainCamera);
            }
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

        public void ShareTo3Party()
        {
            UtilCaptureScreen.CaptureCamera(Ctx.mInstance.mCamSys.getUGuiCamera(), new Rect(0f, 0f, Screen.width, Screen.height));
        }
    }
}
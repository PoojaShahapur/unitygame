using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 控制摄像机运动， CameraMan 只能 Y 轴旋转，如果要旋转摄像机绕其它轴，需要设置摄像机的坐标系
     */
    public class CameraMan
    {
        protected GameObject mTargetGo;
        protected Transform mTargetTrans;
        protected CameraController mCameraController;
        protected Vector3 mLocalPos;
        protected Vector3 mLocalRot;

        public CameraMan(GameObject targetGo)
        {
            mLocalPos = Vector3.zero;
            mTargetGo = targetGo;
            if (mTargetGo == null)
            {
                mTargetGo = UtilApi.createGameObject("CameraGo");
            }

            mTargetTrans = mTargetGo.transform;
            Ctx.mInstance.mInputMgr.addKeyListener(EventID.KEYPRESS_EVENT, onKeyPress);
        }

        public void setActor(GameObject targetGo)
        {
            mTargetGo = targetGo;
            if (mTargetGo == null)
            {
                mTargetGo = UtilApi.createGameObject("CameraGo");
            }
        }

        public void setCameraController(CameraController controller)
        {
            mCameraController = controller;
        }

        virtual public void onKeyPress(KeyCode key)
        {
            if (KeyCode.W == key)
            {
                //mLocalRot = mTargetTrans.localEulerAngles;
                //mLocalRot.x = UtilApi.incEulerAngles(mLocalRot.x, 1);
                //mTargetTrans.localEulerAngles = mLocalRot;
                //mCameraController.updateControl();
                mCameraController.incTheta(1);
            }
            else if (KeyCode.S == key)
            {
                //mLocalRot = mTargetTrans.localEulerAngles;
                //mLocalRot.x = UtilApi.decEulerAngles(mLocalRot.x, 1);
                //mTargetTrans.localEulerAngles = mLocalRot;
                //mCameraController.updateControl();
                mCameraController.decTheta(1);
            }
            else if (KeyCode.A == key)
            {
                mLocalRot = mTargetTrans.localEulerAngles;
                mLocalRot.y = UtilApi.incEulerAngles(mLocalRot.y, 1);
                mTargetTrans.localEulerAngles = mLocalRot;
                mCameraController.updateControl();
            }
            else if (KeyCode.D == key)
            {
                mLocalRot = mTargetTrans.localEulerAngles;
                mLocalRot.y = UtilApi.decEulerAngles(mLocalRot.y, 1);
                mTargetTrans.localEulerAngles = mLocalRot;
                mCameraController.updateControl();
            }
            else if (KeyCode.UpArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.z = mLocalPos.z + 0.1f;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.DownArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.z = mLocalPos.z - 0.1f;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.RightArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.x = mLocalPos.x + 0.1f;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.LeftArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.x = mLocalPos.x - 0.1f;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
        }
    }
}
using UnityEngine;

namespace SDK.Lib
{
    public class TerrainCameraMan : CameraMan
    {
        public TerrainCameraMan(GameObject targetGo)
            : base(targetGo)
        {

        }

        override public void onKeyPress(KeyCode key)
        {
            if (KeyCode.UpArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.z = mLocalPos.z + 1f;
                float height = Ctx.mInstance.mSceneSys.getHeightAt(mLocalPos.x, mLocalPos.z);
                mLocalPos.y = height + 80;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.DownArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.z = mLocalPos.z - 1f;
                float height = Ctx.mInstance.mSceneSys.getHeightAt(mLocalPos.x, mLocalPos.z);
                mLocalPos.y = height + 80;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.RightArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.x = mLocalPos.x + 1f;
                float height = Ctx.mInstance.mSceneSys.getHeightAt(mLocalPos.x, mLocalPos.z);
                mLocalPos.y = height + 80;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else if (KeyCode.LeftArrow == key)
            {
                mLocalPos = mTargetTrans.localPosition;
                mLocalPos.x = mLocalPos.x - 1f;
                float height = Ctx.mInstance.mSceneSys.getHeightAt(mLocalPos.x, mLocalPos.z);
                mLocalPos.y = height + 80;
                mTargetTrans.localPosition = mLocalPos;
                mCameraController.updateControl();
            }
            else
            {
                base.onKeyPress(key);
            }
        }
    }
}
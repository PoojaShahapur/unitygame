namespace SDK.Lib
{
    public class BeingEntityMovement : SceneEntityMovement
    {
        protected UnityEngine.Vector3 mLastPos;         // 之前位置信息
        protected UnityEngine.Quaternion mLastRotate;   // 之前方向信息

        protected UnityEngine.Vector3 mDestPos;         // 目的位置信息
        protected UnityEngine.Quaternion mDestRotate;   // 目的方向信息

        protected float mMoveSpeed;     // 移动速度
        protected float mRotateSpeed;   // 旋转速度

        public BeingEntityMovement(SceneEntityBase entity)
            : base(entity)
        {
            mMoveSpeed = 1;
            mRotateSpeed = 10;
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }

        // 局部空间移动位置
        public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = mEntity.getRotate() * DeltaLocation;
            mEntity.setOriginal(mEntity.getPos() + localOffset);
        }

        // 局部空间旋转
        public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            mEntity.setRotation(mEntity.getRotate() * UtilApi.convQuatFromEuler(DeltaRotation));
        }

        // 向前移动
        public void moveForward()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, mMoveSpeed * delta);
            this.addActorLocalOffset(localMove);
        }

        // 向后移动
        public void moveBack()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, -mMoveSpeed * delta);
            this.addActorLocalOffset(localMove);
        }

        // 向左旋转
        public void rotateLeft()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 向右旋转
        public void rotateRight()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, -mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 移动到最终地点
        public void moveToPos()
        {

        }

        // 直接到
        public void gotoPos()
        {

        }
    }
}
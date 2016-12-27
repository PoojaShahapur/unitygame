using UnityEngine;

namespace SDK.Lib
{
    /**
	 * @brief 主角
	 */
    public class PlayerMain : Player
	{
        // Child 的大小\数量\位置 发生改变触发事件
        protected AddOnceEventDispatch mChildChangedDispatch;
        // 位置改变量，主要是暂时移动 child，以后改通知为服务器 child 位置，就不用这样修改了
        protected Vector3 mDeltaPos;

        public PlayerMain()
		{
            this.mTypeId = "PlayerMain";
            this.mEntityType = EntityType.ePlayerMain;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genNewStrId();
            this.mMovement = new PlayerMainMovement(this);
            this.mAttack = new PlayerMainAttack(this);
            this.mPlayerSplitMerge = new PlayerMainSplitMerge(this);

            this.mChildChangedDispatch = new AddOnceEventDispatch();
        }

        public override void onSkeletonLoaded()
        {
            base.onSkeletonLoaded();

            //Transform tran = mSkinAniModel.transform.FindChild("Reference/Hips");
            //if(tran)
            //{
                //Ctx.mInstance.mCamSys.m_sceneCam.setTarget(tran);
            //}
        }

        public void evtMove()
        {
            //if (mSkinAniModel.animSys.animator && Camera.main)
            //{
            //    Do(mSkinAniModel.transform, Camera.main.transform, ref speed, ref direction);
            //    mSkinAniModel.animSys.Do(speed * 6, direction * 180);
            //}
        }

        public void Do(Transform root, Transform camera, ref float speed, ref float direction)
        {
            Vector3 rootDirection = root.forward;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

            // Get camera rotation.    

            Vector3 CameraDirection = camera.forward;
            CameraDirection.y = 0.0f; // kill Y
            Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

            // Convert joystick input in Worldspace coordinates
            Vector3 moveDirection = referentialShift * stickDirection;

            Vector2 speedVec = new Vector2(horizontal, vertical);
            speed = Mathf.Clamp(speedVec.magnitude, 0, 1);

            if (speed > 0.01f) // dead zone
            {
                Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
                direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
            }
            else
            {
                direction = 0.0f;
            }
        }

        // 主角随机移动
        override protected void initSteerings()
        {
            // 初始化 vehicle
            //aiController.vehicle.MaxSpeed = 10;
            //aiController.vehicle.setSpeed(5);

            //// 初始化 Steerings
            //aiController.vehicle.Steerings = new Steering[1];
            //aiController.vehicle.Steerings[0] = new SteerForWander();
            //aiController.vehicle.Steerings[0].Vehicle = aiController.vehicle as Vehicle;
            //(aiController.vehicle.Steerings[0] as SteerForWander).MaxLatitudeSide = 100;
            //(aiController.vehicle.Steerings[0] as SteerForWander).MaxLatitudeUp = 100;
        }

        override public void initRender()
        {
            mRender = new PlayerMainRender(this);
            mRender.init();
        }

        public override void preInit()
        {
            base.preInit();

            this.hide();    // PlayerMain 不显示，仅仅记录数据

            this.mMovement.init();
            this.mAttack.init();
            this.mPlayerSplitMerge.init();

            //Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch.addEventHandle(null, this.onChildMassChanged);
        }

        public override void postInit()
        {
            base.postInit();

            this.mPlayerSplitMerge.startSplit();            
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mPlayerMgr.removePlayer(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.addHero(this);
        }

        public override void onPreTick(float delta)
        {
            base.onPreTick(delta);
        }

        override public void setPos(UnityEngine.Vector3 pos)
        {
            Vector3 origPos = this.mPos;
            base.setPos(pos);
            this.mDeltaPos = this.mPos - origPos;
        }

        override public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            base.setDestPos(pos, immePos);

            // 调整 Child 的位置
            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.setDestPos(pos, immePos);
            }
        }

        public void emitSnowBlock()
        {
            this.mPlayerSplitMerge.emitSnowBlock();
        }

        override public void setName(string name)
        {
            base.setName(name);

            this.mPlayerSplitMerge.setName();
        }

        public void addChildChangedHandle(ICalleeObject pThis, MAction<IDispatchObject> handle)
        {
            this.mChildChangedDispatch.addEventHandle(pThis, handle);
        }

        public void onChildChanged()
        {
            this.mPlayerSplitMerge.updateCenterPos();
            this.mChildChangedDispatch.dispatchEvent(this);
        }

        public Vector3 getDeltaPos()
        {
            return this.mDeltaPos;
        }

        // Child 质量发生变化
        public void onChildMassChanged(IDispatchObject disp)
        {
            float totalRadius = this.mPlayerSplitMerge.getAllChildMass();
        }
    }
}
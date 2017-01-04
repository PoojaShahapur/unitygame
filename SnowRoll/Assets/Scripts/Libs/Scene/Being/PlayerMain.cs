namespace SDK.Lib
{
    /**
	 * @brief 主角
	 */
    public class PlayerMain : Player
	{
        public PlayerMain()
		{
            this.mTypeId = "PlayerMain";
            this.mEntityType = EntityType.ePlayerMain;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genNewStrId();
            this.mMovement = new PlayerMainMovement(this);
            this.mAttack = new PlayerMainAttack(this);
            this.mPlayerSplitMerge = new PlayerMainSplitMerge(this);
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

        public void Do(UnityEngine.Transform root, UnityEngine.Transform camera, ref float speed, ref float direction)
        {
            UnityEngine.Vector3 rootDirection = root.forward;
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            UnityEngine.Vector3 stickDirection = new UnityEngine.Vector3(horizontal, 0, vertical);

            // Get camera rotation.    

            UnityEngine.Vector3 CameraDirection = camera.forward;
            CameraDirection.y = 0.0f; // kill Y
            UnityEngine.Quaternion referentialShift = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.forward, CameraDirection);

            // Convert joystick input in Worldspace coordinates
            UnityEngine.Vector3 moveDirection = referentialShift * stickDirection;

            UnityEngine.Vector2 speedVec = new UnityEngine.Vector2(horizontal, vertical);
            speed = UnityEngine.Mathf.Clamp(speedVec.magnitude, 0, 1);

            if (speed > 0.01f) // dead zone
            {
                UnityEngine.Vector3 axis = UnityEngine.Vector3.Cross(rootDirection, moveDirection);
                direction = UnityEngine.Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
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

        public void emitSnowBlock()
        {
            this.mPlayerSplitMerge.emitSnowBlock();
        }

        override public void setName(string name)
        {
            base.setName(name);

            this.mPlayerSplitMerge.setName();
        }

        public void onChildChanged()
        {
            this.mPlayerSplitMerge.updateCenterPos();
            Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.dispatchEvent(this);
        }

        // Child 质量发生变化
        public void onChildMassChanged(IDispatchObject disp)
        {
            float totalRadius = this.mPlayerSplitMerge.getAllChildMass();
        }

        public void moveForwardByOrient(UnityEngine.Vector2 orient)
        {
            UnityEngine.Vector3 dir = new UnityEngine.Vector3(orient.x, 0, orient.y);
            UnityEngine.Quaternion quad = UtilMath.getRotateByOrient(dir) * (this.mMovement as PlayerMainMovement).getForwardRotate();

            this.setDestRotate(quad.eulerAngles, true);
            (this.mMovement as PlayerMainMovement).moveForward();
        }

        public void stopMove()
        {
            (this.mMovement as PlayerMainMovement).stopMove();
        }
    }
}
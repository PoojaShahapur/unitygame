namespace SDK.Lib
{
    public class PlayerMainChildMovement : PlayerChildMovement
    {
        public PlayerMainChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void init()
        {
            base.init();

            //this.transform = this.mEntity.transform();
            this.Start();
        }

        override public void dispose()
        {
            base.dispose();
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            this.Update();
        }

        // 被控制的时候向前移动，需要走这里
        public void moveForwardAndUpdateDir()
        {
            //base.moveForward();

            //if(BeingState.BSIdle == (this.mEntity as BeingEntity).getBeingState())
            //{
                this.updateDir();   // 如果从空闲状态开始走，第一次需要更新一下方向
            //}

            (this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);
            this.setIsMoveToDest(true);

            //if ((this.mEntity as PlayerMainChild).isBehindTargetPoint())
            //{
            //    this.mIsAutoPath = true;
            //}
            //else
            //{
            //    this.mIsAutoPath = false;
            //}

            this.mMoveWay = MoveWay.eIOControlMove;
        }

        // Parent Player 方向改变事件处理器
        public void handleParentOrientChanged(IDispatchObject dispObj)
        {
            this.updateDir();
        }

        // Parent Player 位置改变事件处理器
        public void handleParentPosChanged(IDispatchObject dispObj)
        {
            this.moveForwardAndUpdateDir();
        }

        // 方向停止改变
        public void handleParentOrientStopChanged(IDispatchObject dispObj)
        {
            this.movePause();
        }

        // 位置停止改变
        public void handleParentPosStopChanged(IDispatchObject dispObj)
        {
            this.movePause();
        }

        protected void updateDir()
        {
            UnityEngine.Vector3 targetPoint;

            if ((this.mEntity as PlayerMainChild).isBehindTargetPoint())
            {
                targetPoint = (this.mEntity as PlayerChild).mParentPlayer.mPlayerSplitMerge.getTargetPoint();
                UnityEngine.Quaternion retQuat = UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), targetPoint);
                //(this.mEntity as BeingEntity).setDestPosAndDestRotate(targetPoint, false, true);
                (this.mEntity as BeingEntity).setDestRotate(retQuat.eulerAngles, true);
            }
            else
            {
                //targetPoint = this.mEntity.getPos() + (this.mEntity as PlayerChild).mParentPlayer.getRotate() * new UnityEngine.Vector3(0, 0, (this.mEntity as BeingEntity).mMoveSpeed * Ctx.mInstance.mSystemTimeData.deltaSec);
                (this.mEntity as BeingEntity).setDestRotate((this.mEntity as PlayerChild).mParentPlayer.getRotateEulerAngle(), true);
            }
        }

        //---------------------- SteerForSeparation Start ---------------------------
        private float _comfortDistance = 1;
        private float _multiplierInsideComfortDistance = 1;
        private float _vehicleRadiusImpact = 0;
        private float _comfortDistanceSquared;

        protected MList<SceneEntityBase> _neighbors;
        protected bool _drawNeighbors;

        private float _minDistance = 1;
        private float _maxDistance = 10;

        public float ComfortDistance
        {
            get { return _comfortDistance; }
            set
            {
                _comfortDistance = value;
                _comfortDistanceSquared = _comfortDistance * _comfortDistance;
            }
        }

        public float MaxDistanceSquared { get; private set; }
        public float MinDistanceSquared { get; private set; }

        protected void Start()
        {
            MaxDistanceSquared = _maxDistance * _maxDistance;
            MinDistanceSquared = _minDistance * _minDistance;

            _drawNeighbors = true;
            _comfortDistanceSquared = _comfortDistance * _comfortDistance;
            _neighbors = ((this.mEntity as PlayerChild).mParentPlayer as Player).getChildList();
        }

        public UnityEngine.Vector3 CalculateNeighborContribution(PlayerMainChild other)
        {
            UnityEngine.Vector3 steering = UnityEngine.Vector3.zero;

            // add in steering contribution
            // (opposite of the offset direction, divided once by distance
            // to normalize, divided another time to get 1/d falloff)
            // 如果正好重叠， offset 正好是 0
            UnityEngine.Vector3 offset = other.getPos()- this.mEntity.getPos();
            if(UnityEngine.Vector3.zero == offset)  // 如果两个位置重叠
            {
                offset = UtilMath.UnitCircleRandom();   // 获取一个单位圆随机位置
            }
            float offsetSqrMag = offset.sqrMagnitude;

            steering = (offset / -offsetSqrMag);
            if (!UnityEngine.Mathf.Approximately(_multiplierInsideComfortDistance, 1) && offsetSqrMag < _comfortDistanceSquared)
            {
                steering *= _multiplierInsideComfortDistance;
            }

            if (_vehicleRadiusImpact > 0)
            {
                steering *= (other.getEatSize() + (this.mEntity as PlayerMainChild).getEatSize()) * _vehicleRadiusImpact;
            }

            return steering;
        }

        protected UnityEngine.Vector3 CalculateForces()
        {
            UnityEngine.Vector3 steering = UnityEngine.Vector3.zero;
            for (var i = 0; i < _neighbors.Count(); i++)
            {
                PlayerMainChild other = _neighbors[i] as PlayerMainChild;
                if (other != this.mEntity)
                {
                    UnityEngine.Vector3 direction = other.getPos() - this.mEntity.getPos();
                    if (_drawNeighbors)
                    {
                        UtilApi.DrawLine(this.mEntity.getPos(), other.getPos(), UnityEngine.Color.magenta);
                    }
                    //if (this.IsDirectionInRange(direction))
                    if((this.mEntity as BeingEntity).isNeedSeparate(other as BeingEntity))
                    {
                        steering += this.CalculateNeighborContribution(other);
                    }
                }
            }

            steering.Normalize();

            return steering;
        }

        public bool IsDirectionInRange(UnityEngine.Vector3 difference)
        {
            return
                UtilMath.IntervalComparison(difference.sqrMagnitude, MinDistanceSquared, MaxDistanceSquared) ==
                0;
        }

        private void Update()
        {
            if ((this.mEntity as BeingEntity).canSeparateByState())
            {
                UnityEngine.Vector3 steering = this.CalculateForces();
                if (UnityEngine.Vector3.zero != steering)
                {
                    UnityEngine.Quaternion rotate = UtilMath.getRotateByOrient(steering);
                    (this.mEntity as PlayerMainChild).setDestRotate(rotate.eulerAngles, true);
                    this.moveForwardSeparate();
                }
                else
                {
                    this.stopMove();
                }
            }
        }

        //---------------------- SteerForSeparation Start ---------------------------
    }
}
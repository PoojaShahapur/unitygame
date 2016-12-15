﻿namespace SDK.Lib
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
            {
                this.updateDir();   // 如果从空闲状态开始走，第一次需要更新一下方向
            }

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

            this.mIsAutoPath = false;
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

        //---------------------- Flock Start-----------------------------
        //public float minSpeed = 100.0f;         //movement speed of the flock
        //public float turnSpeed = 20.0f;         //rotation speed of the flock
        //public float randomFreq = 20.0f;

        //public float randomForce = 20.0f;       //Force strength in the unit sphere
        //public float toOriginForce = 20.0f;
        //public float toOriginRange = 100.0f;

        //public float gravity = 2.0f;            //Gravity of the flock

        //public float avoidanceRadius = 400.0f;  //Minimum distance between flocks
        //public float avoidanceForce = 20.0f;

        //public float followVelocity = 4.0f;
        //public float followRadius = 40.0f;      //Minimum Follow distance to the leader

        //private UnityEngine.Transform origin;               //Parent transform
        //private UnityEngine.Vector3 velocity;               //Velocity of the flock
        //private UnityEngine.Vector3 normalizedVelocity;
        //private UnityEngine.Vector3 randomPush;             //Random push value
        //private UnityEngine.Vector3 originPush;
        //private UnityEngine.Transform[] objects;            //Flock objects in the group
        //private PlayerMainChildMovement[] otherFlocks;       //Unity Flocks in the group
        //private UnityEngine.Transform transformComponent;   //My transform

        //private UnityEngine.Transform transform;

        //protected void Start()
        //{
        //    randomFreq = 1.0f / randomFreq;

        //    //Assign the parent as origin
        //    //origin = transform.parent;
        //    origin = (this.mEntity as PlayerChild).mParentPlayer.transform();

        //    //Flock transform           
        //    transformComponent = transform;

        //    //Temporary components
        //    PlayerMovement[] tempFlocks = null;

        //    //Get all the unity flock components from the parent transform in the group
        //    tempFlocks = (this.mEntity as PlayerChild).mParentPlayer.getAllChildMovement();

        //    //Assign and store all the flock objects in this group
        //    objects = new UnityEngine.Transform[tempFlocks.Length];
        //    otherFlocks = new PlayerMainChildMovement[tempFlocks.Length];

        //    for (int i = 0; i < tempFlocks.Length; i++)
        //    {
        //        objects[i] = (tempFlocks[i] as PlayerMainChildMovement).transform;
        //        otherFlocks[i] = (PlayerMainChildMovement)tempFlocks[i];
        //    }

        //    //Null Parent as the flock leader will be UnityFlockController object
        //    //transform.parent = null;

        //    //Calculate random push depends on the random frequency provided
        //    Ctx.mInstance.mCoroutineMgr.StartCoroutine(UpdateRandom());
        //}

        //protected System.Collections.IEnumerator UpdateRandom()
        //{
        //    while (true)
        //    {
        //        randomPush = UnityEngine.Random.insideUnitSphere * randomForce;
        //        yield return new UnityEngine.WaitForSeconds(randomFreq + UnityEngine.Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
        //    }
        //}

        //protected void Update()
        //{
        //    //Internal variables
        //    float speed = velocity.magnitude;
        //    UnityEngine.Vector3 avgVelocity = UnityEngine.Vector3.zero;
        //    UnityEngine.Vector3 avgPosition = UnityEngine.Vector3.zero;
        //    float count = 0;
        //    float f = 0.0f;
        //    float d = 0.0f;
        //    UnityEngine.Vector3 myPosition = transformComponent.position;
        //    UnityEngine.Vector3 forceV;
        //    UnityEngine.Vector3 toAvg;
        //    UnityEngine.Vector3 wantedVel;

        //    for (int i = 0; i < objects.Length; i++)
        //    {
        //        UnityEngine.Transform transform = objects[i];
        //        if (transform != transformComponent)
        //        {
        //            UnityEngine.Vector3 otherPosition = transform.position;

        //            // Average position to calculate cohesion
        //            avgPosition += otherPosition;
        //            count++;

        //            //Directional vector from other flock to this flock
        //            forceV = myPosition - otherPosition;

        //            //Magnitude of that directional vector(Length)
        //            d = forceV.magnitude;

        //            //Add push value if the magnitude is less than follow radius to the leader
        //            if (d < followRadius)
        //            {
        //                //calculate the velocity based on the avoidance distance between flocks 
        //                //if the current magnitude is less than the specified avoidance radius
        //                if (d < avoidanceRadius)
        //                {
        //                    f = 1.0f - (d / avoidanceRadius);

        //                    if (d > 0)
        //                        avgVelocity += (forceV / d) * f * avoidanceForce;
        //                }

        //                //just keep the current distance with the leader
        //                f = d / followRadius;
        //                PlayerMainChildMovement tempOtherFlock = otherFlocks[i];
        //                avgVelocity += tempOtherFlock.normalizedVelocity * f * followVelocity;
        //            }
        //        }
        //    }

        //    if (count > 0)
        //    {
        //        //Calculate the average flock velocity(Alignment)
        //        avgVelocity /= count;

        //        //Calculate Center value of the flock(Cohesion)
        //        toAvg = (avgPosition / count) - myPosition;
        //    }
        //    else
        //    {
        //        toAvg = UnityEngine.Vector3.zero;
        //    }

        //    //Directional Vector to the leader
        //    forceV = origin.position - myPosition;
        //    d = forceV.magnitude;
        //    f = d / toOriginRange;

        //    //Calculate the velocity of the flock to the leader
        //    if (d > 0)
        //        originPush = (forceV / d) * f * toOriginForce;

        //    if (speed < minSpeed && speed > 0)
        //    {
        //        velocity = (velocity / speed) * minSpeed;
        //    }

        //    wantedVel = velocity;

        //    //Calculate final velocity
        //    wantedVel -= wantedVel * Ctx.mInstance.mSystemTimeData.deltaSec;
        //    wantedVel += randomPush * Ctx.mInstance.mSystemTimeData.deltaSec;
        //    wantedVel += originPush * Ctx.mInstance.mSystemTimeData.deltaSec;
        //    wantedVel += avgVelocity * Ctx.mInstance.mSystemTimeData.deltaSec;
        //    wantedVel += toAvg.normalized * gravity * Ctx.mInstance.mSystemTimeData.deltaSec;

        //    //Final Velocity to rotate the flock into
        //    velocity = UnityEngine.Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Ctx.mInstance.mSystemTimeData.deltaSec, 100.00f);
        //    transformComponent.rotation = UnityEngine.Quaternion.LookRotation(velocity);

        //    //Move the flock based on the calculated velocity
        //    transformComponent.Translate(velocity * Ctx.mInstance.mSystemTimeData.deltaSec, UnityEngine.Space.World);

        //    //normalise the velocity
        //    normalizedVelocity = velocity.normalized;
        //}
        //---------------------- Flock End -----------------------------

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
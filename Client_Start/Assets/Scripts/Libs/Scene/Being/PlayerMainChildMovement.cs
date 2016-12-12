namespace SDK.Lib
{
    public class PlayerMainChildMovement : PlayerChildMovement
    {
        public PlayerMainChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            //float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            //if (horizontal > 0.0f)
            //{
            //    this.rotateLeft();
            //}
            //else if (horizontal < 0.0f)
            //{
            //    this.rotateRight();
            //}
            //else
            //{
            //    if(!this.isMoveToDest())
            //    {
            //        this.stopMove();
            //    }
            //}

            //float vertical = UnityEngine.Input.GetAxis("Vertical");
            //if (vertical > 0.0f)
            //{
            //    //this.moveForward();
            //    this.moveAlong();
            //}
            //else if (vertical < 0.0f)
            //{
            //    //this.moveBack();
            //    this.moveAlong();
            //}
            //else
            //{
            //    this.movePause();
            //}
        }

        // Parent Player 方向改变事件处理器
        public void handleParentOrientChanged(IDispatchObject dispObj)
        {
            this.updateDir();
        }

        // Parent Player 位置改变事件处理器
        public void handleParentPosChanged(IDispatchObject dispObj)
        {
            this.moveAlong();
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
            this.lookAt((this.mEntity as PlayerChild).mParentPlayer.mPlayerSplitMerge.getTargetPoint());
        }

        //---------------------- Flock Start-----------------------------
        public float minSpeed = 100.0f;         //movement speed of the flock
        public float turnSpeed = 20.0f;         //rotation speed of the flock
        public float randomFreq = 20.0f;

        public float randomForce = 20.0f;       //Force strength in the unit sphere
        public float toOriginForce = 20.0f;
        public float toOriginRange = 100.0f;

        public float gravity = 2.0f;            //Gravity of the flock

        public float avoidanceRadius = 400.0f;  //Minimum distance between flocks
        public float avoidanceForce = 20.0f;

        public float followVelocity = 4.0f;
        public float followRadius = 40.0f;      //Minimum Follow distance to the leader

        private UnityEngine.Transform origin;               //Parent transform
        private UnityEngine.Vector3 velocity;               //Velocity of the flock
        private UnityEngine.Vector3 normalizedVelocity;
        private UnityEngine.Vector3 randomPush;             //Random push value
        private UnityEngine.Vector3 originPush;
        private UnityEngine.Transform[] objects;            //Flock objects in the group
        private PlayerMainChildMovement[] otherFlocks;       //Unity Flocks in the group
        private UnityEngine.Transform transformComponent;   //My transform

        private UnityEngine.Transform transform;

        void Start()
        {
            randomFreq = 1.0f / randomFreq;

            //Assign the parent as origin
            origin = transform.parent;

            //Flock transform           
            transformComponent = transform;

            //Temporary components
            PlayerMainChildMovement[] tempFlocks = null;

            //Get all the unity flock components from the parent transform in the group
            if (transform.parent)
            {
                tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
            }

            //Assign and store all the flock objects in this group
            objects = new UnityEngine.Transform[tempFlocks.Length];
            otherFlocks = new PlayerMainChildMovement[tempFlocks.Length];

            for (int i = 0; i < tempFlocks.Length; i++)
            {
                objects[i] = tempFlocks[i].transform;
                otherFlocks[i] = (PlayerMainChildMovement)tempFlocks[i];
            }

            //Null Parent as the flock leader will be UnityFlockController object
            transform.parent = null;

            //Calculate random push depends on the random frequency provided
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(UpdateRandom());
        }

        System.Collections.IEnumerator UpdateRandom()
        {
            while (true)
            {
                randomPush = UnityEngine.Random.insideUnitSphere * randomForce;
                yield return new UnityEngine.WaitForSeconds(randomFreq + UnityEngine.Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
            }
        }

        void Update()
        {
            //Internal variables
            float speed = velocity.magnitude;
            UnityEngine.Vector3 avgVelocity = UnityEngine.Vector3.zero;
            UnityEngine.Vector3 avgPosition = UnityEngine.Vector3.zero;
            float count = 0;
            float f = 0.0f;
            float d = 0.0f;
            UnityEngine.Vector3 myPosition = transformComponent.position;
            UnityEngine.Vector3 forceV;
            UnityEngine.Vector3 toAvg;
            UnityEngine.Vector3 wantedVel;

            for (int i = 0; i < objects.Length; i++)
            {
                UnityEngine.Transform transform = objects[i];
                if (transform != transformComponent)
                {
                    UnityEngine.Vector3 otherPosition = transform.position;

                    // Average position to calculate cohesion
                    avgPosition += otherPosition;
                    count++;

                    //Directional vector from other flock to this flock
                    forceV = myPosition - otherPosition;

                    //Magnitude of that directional vector(Length)
                    d = forceV.magnitude;

                    //Add push value if the magnitude is less than follow radius to the leader
                    if (d < followRadius)
                    {
                        //calculate the velocity based on the avoidance distance between flocks 
                        //if the current magnitude is less than the specified avoidance radius
                        if (d < avoidanceRadius)
                        {
                            f = 1.0f - (d / avoidanceRadius);

                            if (d > 0)
                                avgVelocity += (forceV / d) * f * avoidanceForce;
                        }

                        //just keep the current distance with the leader
                        f = d / followRadius;
                        PlayerMainChildMovement tempOtherFlock = otherFlocks[i];
                        avgVelocity += tempOtherFlock.normalizedVelocity * f * followVelocity;
                    }
                }
            }

            if (count > 0)
            {
                //Calculate the average flock velocity(Alignment)
                avgVelocity /= count;

                //Calculate Center value of the flock(Cohesion)
                toAvg = (avgPosition / count) - myPosition;
            }
            else
            {
                toAvg = UnityEngine.Vector3.zero;
            }

            //Directional Vector to the leader
            forceV = origin.position - myPosition;
            d = forceV.magnitude;
            f = d / toOriginRange;

            //Calculate the velocity of the flock to the leader
            if (d > 0)
                originPush = (forceV / d) * f * toOriginForce;

            if (speed < minSpeed && speed > 0)
            {
                velocity = (velocity / speed) * minSpeed;
            }

            wantedVel = velocity;

            //Calculate final velocity
            wantedVel -= wantedVel * Ctx.mInstance.mSystemTimeData.deltaSec;
            wantedVel += randomPush * Ctx.mInstance.mSystemTimeData.deltaSec;
            wantedVel += originPush * Ctx.mInstance.mSystemTimeData.deltaSec;
            wantedVel += avgVelocity * Ctx.mInstance.mSystemTimeData.deltaSec;
            wantedVel += toAvg.normalized * gravity * Ctx.mInstance.mSystemTimeData.deltaSec;

            //Final Velocity to rotate the flock into
            velocity = UnityEngine.Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Ctx.mInstance.mSystemTimeData.deltaSec, 100.00f);
            transformComponent.rotation = UnityEngine.Quaternion.LookRotation(velocity);

            //Move the flock based on the calculated velocity
            transformComponent.Translate(velocity * Ctx.mInstance.mSystemTimeData.deltaSec, UnityEngine.Space.World);

            //normalise the velocity
            normalizedVelocity = velocity.normalized;
        }
        //---------------------- Flock End -----------------------------
    }
}
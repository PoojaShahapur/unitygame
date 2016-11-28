using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 主角
	 */
    public class PlayerMain : Player
	{
		public PlayerMain()
		{
            mTypeId = "PlayerMain";
        }

        public override void onSkeletonLoaded()
        {
            base.onSkeletonLoaded();
            //Transform tran = m_skinAniModel.transform.FindChild("Reference/Hips");
            //if(tran)
            //{
                //Ctx.mInstance.mCamSys.m_sceneCam.setTarget(tran);
            //}
        }

        public void evtMove()
        {
            //if (m_skinAniModel.animSys.animator && Camera.main)
            //{
            //    Do(m_skinAniModel.transform, Camera.main.transform, ref speed, ref direction);
            //    m_skinAniModel.animSys.Do(speed * 6, direction * 180);
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

        // 刷新 Child 的位置
        void RefreshChildrenPosition()
        {
            foreach (var child in this.mChildrenList.list())
            {
                //float player_radius = player.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * player.GetComponent<Transform>().localScale.x;
                //float child_radius = child.childrenObj.GetComponent<MeshFilter>().mesh.bounds.size.x * child.childrenObj.GetComponent<Transform>().localScale.x;
                //float x = player.GetComponent<Transform>().position.x + player_radius + child_radius + child.startX;
                //float z = player.GetComponent<Transform>().position.z + player_radius + child_radius + child.startZ;

                //float y = child.childrenObj.GetComponent<Transform>().position.y;
                //child.childrenObj.GetComponent<Transform>().position = new Vector3(x, y, z);

                float player_radius = this.getBounds().size.x * this.transform().localScale.x;
                float child_radius = child.getBounds().size.x * child.transform().localScale.x;
                float x = this.transform().position.x + player_radius + child_radius + child.startX;
                float z = this.transform().position.z + player_radius + child_radius + child.startZ;

                float y = child.transform().position.y;
                child.transform().position = new Vector3(x, y, z);
            }
        }

        // 刷新 Child 的旋转
        public void RefreshChildrensRotation(Vector3 eulerangles)
        {
            foreach (var child in this.mChildrenList.list())
            {
                child.transform().eulerAngles = eulerangles;
            }
        }

        //--------------------
        public ControlType controlType = ControlType.GravitytouchControl;
        private float horizontal_move = 0.0f;
        private float vertical_move = 0.0f;

        private GameObject player1;//该子物体用于添加表现雪球旋转
        private GameObject cmr;

        //V = k * m + b //速度与质量关系式
        public float MoveSpeed_k = 10.0f;//k = 10 / r
        public float MoveSpeed_b = 2.0f;
        private float MoveSpeed = 10.0f;

        public float forceSensitivity = 1.0f;//加速倾斜效果，使得倾斜分量迅速增加或减少

        // Use this for initialization
        void Start()
        {
            double _speed = MoveSpeed_k / Mathf.Sqrt(this.transform().localScale.x) + MoveSpeed_b;
            MoveSpeed = (float)System.Math.Round(_speed, 3);

            //player1 = transform.FindChild("Player1").gameObject;
            cmr = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

            //this.GetComponent<Food>().entity.m_isOnGround = true;
            //this.GetComponent<Food>().entity.m_isRobot = false;
            //this.GetComponent<Food>().entity.m_canEatRate = this.GetComponent<Food>().canEatRate;

            this.m_isOnGround = true;
            this.m_isRobot = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Edit -> Project Setting -> Input 里面定义以下数值
            if (!this.IsOnGround())
            {
                //log.logHelper.DebugLog("玩家不在地上,不加力");
                return;
            }

            if (CreatePlayer._Instace.GetIsDontMove()) return;

            //电脑方向键控制
            if (controlType == ControlType.KeyBoardControl)
            {
                horizontal_move = Input.GetAxis("Horizontal");
                vertical_move = Input.GetAxis("Vertical");
                //按住前向按钮
                if (CreatePlayer._Instace.GetIsPressForwardForceBtn())
                {
                    vertical_move = -CreatePlayer._Instace.GetForwardForce();
                }

                //if (horizontal_move != 0 || vertical_move != 0)
                //    log.logHelper.DebugLog(this.name + "   Mass: " + this.GetComponent<Rigidbody>().mass + "   半径： " + this.GetComponent<Transform>().localScale.x + "   速度: " + this.GetComponent<Rigidbody>().velocity.magnitude + "   施加力: " + MoveSpeed);

                this.AddRelativeForce(new Vector3(horizontal_move, 0, vertical_move) * MoveSpeed, ForceMode.Impulse);

                if (vertical_move == 0 && horizontal_move == 0)
                {

                }
                else
                {
                    //Vector3 startRotation = player1.transform.rotation.eulerAngles;
                    //Vector3 endRotation = new Vector3(0, (Mathf.Atan2(-vertical_move, horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
                    //player1.transform.rotation = Quaternion.FromToRotation(startRotation, endRotation);
                    player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-vertical_move, horizontal_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
                }
            }

            //手机重力控制
            if (controlType == ControlType.GravitytouchControl)
            {
                horizontal_move = Input.acceleration.y;
                vertical_move = Input.acceleration.x;
                horizontal_move *= forceSensitivity;
                vertical_move *= forceSensitivity;
                if (horizontal_move < -1) horizontal_move = -1;
                if (horizontal_move > 1) horizontal_move = 1;
                if (vertical_move < -1) vertical_move = -1;
                if (vertical_move > 1) vertical_move = 1;

                //按住前向按钮，施加一个向前最大的力
                if (CreatePlayer._Instace.GetIsPressForwardForceBtn())
                {
                    horizontal_move = CreatePlayer._Instace.GetForwardForce();
                }

                log.logHelper.DebugLog(CreatePlayer._Instace.GetForwardForce().ToString());
                Vector3 force = new Vector3(vertical_move, 0, horizontal_move);
                //var transform = this.GetComponent<Transform>();

                //添加力
                this.AddRelativeForce(force * MoveSpeed, ForceMode.Impulse);
                if (vertical_move == 0 && horizontal_move == 0)
                {

                }
                else
                {
                    player1.transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-horizontal_move, vertical_move) * Mathf.Rad2Deg) + cmr.transform.rotation.eulerAngles.y + 90, 0);
                }
            }

            double _speed = MoveSpeed_k / Mathf.Sqrt(this.transform().localScale.x) + MoveSpeed_b;
            MoveSpeed = (float)System.Math.Round(_speed, 3);
        }


        public Vector3 GetCenterPosition()
        {
            Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);
            float x_min = float.MaxValue, x_max = float.MinValue, z_min = float.MaxValue, z_max = float.MinValue;
            foreach (var obj in this.mChildrenList.list())
            {
                if (obj.transform().position.x < x_min) x_min = obj.transform().position.x;
                if (obj.transform().position.x > x_max) x_max = obj.transform().position.x;
                if (obj.transform().position.z < z_min) z_min = obj.transform().position.z;
                if (obj.transform().position.z > z_max) z_max = obj.transform().position.z;
            }
            if (this.transform().position.x < x_min) x_min = this.transform().position.x;
            if (this.transform().position.x > x_max) x_max = this.transform().position.x;
            if (this.transform().position.z < z_min) z_min = this.transform().position.z;
            if (this.transform().position.z > z_max) z_max = this.transform().position.z;

            center.x = (x_max + x_min) / 2;
            center.z = (z_max + z_min) / 2;

            return center;
        }

        public float GetScaleDistance(UnityEngine.Vector3 center)
        {
            float distance = 0.0f;
            float x_2 = UtilLogic.getSquare(this.transform().position.x - center.x);
            float z_2 = UtilLogic.getSquare(this.transform().position.z - center.z);
            float curRadius = this.transform().localScale.x;
            float radius = this.getBounds().size.x * curRadius;
            distance = Mathf.Sqrt(x_2 + z_2) + radius;

            foreach (var obj in this.mChildrenList.list())
            {
                x_2 = UtilLogic.getSquare(obj.transform().position.x - center.x);
                z_2 = UtilLogic.getSquare(obj.transform().position.z - center.z);
                curRadius = obj.transform().localScale.x;
                radius = obj.getBounds().size.x * curRadius;
                float _distance = Mathf.Sqrt(x_2 + z_2) + radius;
                if (_distance > distance)
                    distance = _distance;
            }

            return distance;
        }
    }
}
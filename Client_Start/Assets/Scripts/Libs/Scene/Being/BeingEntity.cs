using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 生物 Entity，有感知，可以交互的
	 */
    public class BeingEntity : SceneEntityBase
    {
        protected SkinModelSkelAnim m_skinAniModel;      // 模型数据

        //protected float speed = 0;
        protected float direction = 0;

        public BeingEntity()
        {
            //m_skinAniModel = new SkinModelSkelAnim();
            //m_skinAniModel.handleCB = onSkeletonLoaded;
        }

        public SkinModelSkelAnim skinAniModel
        {
            get
            {
                return m_skinAniModel;
            }
        }

        public void setLocalPos(Vector3 pos)
        {
            //UtilApi.setPos(m_skinAniModel.transform, pos);
        }

        // 骨骼设置，骨骼不能更换
        public void setSkeleton(string name)
        {
            //if(string.IsNullOrEmpty(m_skinAniModel.m_skeletonName))
            //{
            //    m_skinAniModel.m_skeletonName = name;
            //    m_skinAniModel.loadSkeleton();
            //}
        }

        public void setPartModel(int modelDef, string assetBundleName, string partName)
        {
            //m_skinAniModel.m_modelList[modelDef].m_bundleName = string.Format("{0}{1}", assetBundleName, ".prefab");
            //m_skinAniModel.m_modelList[modelDef].m_partName = partName;
            //m_skinAniModel.loadPartModel(modelDef);
        }

        public virtual void onSkeletonLoaded()
        {
            
        }

        // 目前只有怪有 Steerings ,加载这里是为了测试，全部都有 Steerings
        virtual protected void initSteerings()
        {

        }

        virtual public string getDesc()
        {
            return "";
        }

        public BeingBehaviorControl behaviorControl
        {
            get
            {
                return getBeingBehaviorControl();
            }
        }

        virtual public BeingBehaviorControl getBeingBehaviorControl()
        {
            return null;
        }

        public EffectControl effectControl
        {
            get
            {
                return getEffectControl();
            }
        }

        virtual public EffectControl getEffectControl()
        {
            return null;
        }

        public uint qwThisID
        {
            get
            {
                return 0;
            }
        }

        public void playFlyNum(int num)
        {

        }

        override public void init()
        {
            // 基类初始化
            base.init();
            // 自动处理，例如添加到管理器
            this.autoHandle();
            // 初始化渲染器
            this.initRender();
            // 加载渲染器资源
            this.loadRenderRes();
            // 更新位置
            this.updateTransform();
        }

        override public void loadRenderRes()
        {
            mRender.load();
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
            this.Update();
            this.FixedUpdate();
        }

        //-----------------------------------------------------------
        //-------------- KBEngine -----------------
        public bool isPlayer = false;

        private Vector3 _position = Vector3.zero;
        private Vector3 _eulerAngles = Vector3.zero;
        private Vector3 _scale = Vector3.zero;

        public Vector3 destPosition = Vector3.zero;
        public Vector3 destDirection = Vector3.zero;

        private float _speed = 0f;
        private byte jumpState = 0;
        private float currY = 1.0f;

        private Camera playerCamera = null;

        public string entity_name;

        public string hp = "100/100";

        float npcHeight = 2.0f;

        //public CharacterController characterController;

        public bool isOnGround = true;

        public bool isControlled = false;

        public bool entityEnabled = true;

        //void Start()
        //{
        //    characterController = ((UnityEngine.GameObject)this.gameObject()).GetComponent<CharacterController>();
        //}

        public Vector3 position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;

                if (this.gameObject() != null)
                    this.gameObject().transform.position = _position;
            }
        }

        public Vector3 eulerAngles
        {
            get
            {
                return _eulerAngles;
            }

            set
            {
                _eulerAngles = value;

                if (this.gameObject() != null)
                {
                    this.gameObject().transform.eulerAngles = _eulerAngles;
                }
            }
        }

        public Quaternion rotation
        {
            get
            {
                return Quaternion.Euler(_eulerAngles);
            }

            set
            {
                eulerAngles = value.eulerAngles;
            }
        }

        public Vector3 scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _scale = value;

                if (this.gameObject() != null)
                    this.gameObject().transform.localScale = _scale;
            }
        }

        public float speed
        {
            get
            {
                return _speed;
            }

            set
            {
                _speed = value;
            }
        }

        public void entityEnable()
        {
            entityEnabled = true;
        }

        public void entityDisable()
        {
            entityEnabled = false;
        }

        public void set_state(sbyte v)
        {
            //if (v == 3)
            //{
            //    if (isPlayer)
            //        this.gameObject().transform.FindChild("Graphics").GetComponent<MeshRenderer>().material.color = Color.green;
            //    else
            //        this.gameObject().transform.FindChild("Graphics").GetComponent<MeshRenderer>().material.color = Color.red;
            //}
            //else if (v == 0)
            //{
            //    if (isPlayer)
            //        this.gameObject().transform.FindChild("Graphics").GetComponent<MeshRenderer>().material.color = Color.blue;
            //    else
            //        this.gameObject().transform.FindChild("Graphics").GetComponent<MeshRenderer>().material.color = Color.white;
            //}
            //else if (v == 1)
            //{
            //    gameObject.transform.FindChild("Graphics").GetComponent<MeshRenderer>().material.color = Color.black;
            //}
        }

        void FixedUpdate()
        {
            if (!entityEnabled || KBEngine.KBEngineApp.app == null)
                return;

            if (isPlayer == isControlled)
                return;

            KBEngine.Event.fireIn("updatePlayer", this.gameObject().transform.position.x,
                this.gameObject().transform.position.y, this.gameObject().transform.position.z, this.gameObject().transform.rotation.eulerAngles.y);
        }

        void Update()
        {
            if (!entityEnabled)
            {
                position = destPosition;
                return;
            }

            float deltaSpeed = (speed * Time.deltaTime);

            if (isPlayer == true && isControlled == false)
            {
                (this.mRender as BeingEntityRender).characterController.stepOffset = deltaSpeed;

                if (isOnGround != (this.mRender as BeingEntityRender).characterController.isGrounded)
                {
                    KBEngine.Entity player = KBEngine.KBEngineApp.app.player();
                    player.isOnGround = (this.mRender as BeingEntityRender).characterController.isGrounded;
                    isOnGround = (this.mRender as BeingEntityRender).characterController.isGrounded;
                }

                return;
            }

            if (Vector3.Distance(eulerAngles, destDirection) > 0.0004f)
            {
                rotation = Quaternion.Slerp(rotation, Quaternion.Euler(destDirection), 8f * Time.deltaTime);
            }

            float dist = 0.0f;

            if (isOnGround)
            {
                dist = Vector3.Distance(new Vector3(destPosition.x, 0f, destPosition.z),
                    new Vector3(position.x, 0f, position.z));
            }
            else
            {
                dist = Vector3.Distance(destPosition, position);
            }

            if (jumpState > 0)
            {
                if (jumpState == 1)
                {
                    currY += 0.05f;

                    if (currY > 2.0f)
                        jumpState = 2;
                }
                else
                {
                    currY -= 0.05f;
                    if (currY < 1.0f)
                    {
                        jumpState = 0;
                        currY = 1.0f;
                    }
                }

                Vector3 pos = position;
                pos.y = currY;
                position = pos;
            }

            if (dist > 0.01f)
            {
                Vector3 pos = position;

                Vector3 movement = destPosition - pos;
                movement.y = 0f;
                movement.Normalize();

                movement *= deltaSpeed;

                if (dist > deltaSpeed || movement.magnitude > deltaSpeed)
                    pos += movement;
                else
                    pos = destPosition;

                if (isOnGround)
                    pos.y = currY;

                position = pos;
            }
            else
            {
            }
        }

        public void OnJump()
        {
            Debug.Log("jumpState: " + jumpState);

            if (jumpState != 0)
                return;

            jumpState = 1;
        }
    }
}
using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;

namespace Game.App
{
    /**
     * @brief 数据系统
     */
    public class AppSys : Object
    {
        public void Awake(Transform transform)
        {
            Ctx.m_instance = new Ctx();
            Ctx.m_instance.Awake();
            Ctx.m_instance.m_netMgr = new NetworkMgr();
            Ctx.m_instance.m_cfg = new Config();
            Ctx.m_instance.m_log = new Logger();
            Ctx.m_instance.m_resMgr = new ResMgr();
            Ctx.m_instance.m_inputMgr = new InputMgr();
            Ctx.m_instance.m_dataTrans = transform;
        }

        // Use this for initialization
        public void Start()
        {
            Ctx.m_instance.Start();
        }

        // Update is called once per frame
        public void Update()
        {
            Ctx.m_instance.Update();
        }

        public void OnApplicationQuit()
        {

        }

        public void setNoDestroyObject()
        {
            GameObject nodestroy = GameObject.FindGameObjectWithTag("GoNoDestroy");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("RootLayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("SceneLayer");
            DontDestroyOnLoad(nodestroy);

            nodestroy = GameObject.FindGameObjectWithTag("GameLayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("UILayer");
            DontDestroyOnLoad(nodestroy);
            nodestroy = GameObject.FindGameObjectWithTag("UIRoot");
            DontDestroyOnLoad(nodestroy);

            nodestroy = GameObject.FindGameObjectWithTag("Camera");
            DontDestroyOnLoad(nodestroy);
            // NGUI 2.7.0 之前的版本，编辑器会将 width and height 作为 transform 的 local scale ，因此需要手工重置
            nodestroy = GameObject.FindGameObjectWithTag("UIFirstLayer");
            //nodestroy.transform.localScale = Vector3.one;
            DontDestroyOnLoad(nodestroy);
        }

        // 加载游戏模块
        public void loadGame()
        {
            // 初始化完成，开始加载自己的游戏场景
            LoadParam param = (Ctx.m_instance.m_resMgr as ResMgr).loadParam;
            param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathScene] + "Game.unity3d";
            param.m_type = ResPackType.eLevelType;
            param.m_lvlName = "Game";
            param.m_cb = onGameLoaded;
            Ctx.m_instance.m_resMgr.load(param);
        }

        public void onGameLoaded(IRes res)
        {
            // 游戏模块也不释放
            GameObject nodestroy = GameObject.FindGameObjectWithTag("GameModule");
            DontDestroyOnLoad(nodestroy);
        }
    }
}
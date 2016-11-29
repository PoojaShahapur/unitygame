using KBEngine;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief KBEngine 包装，直接修改 KBEMain.cs 文件
     */
    public class MKBEMainEntry
    {
        public KBEngineApp gameapp = null;

        // 在unity3d界面中可见选项
        public DEBUGLEVEL debugLevel = DEBUGLEVEL.DEBUG;
        //public bool isMultiThreads = true;
        public bool isMultiThreads = false;
        public string ip = "127.0.0.1";
        public int port = 20013;
        public KBEngineApp.CLIENT_TYPE clientType = KBEngineApp.CLIENT_TYPE.CLIENT_TYPE_MINI;
        public string persistentDataPath = "Application.persistentDataPath";
        public bool syncPlayer = true;
        public int threadUpdateHZ = 10;
        public int SEND_BUFFER_MAX = (int)KBEngine.NetworkInterface.TCP_PACKET_MAX;
        public int RECV_BUFFER_MAX = (int)KBEngine.NetworkInterface.TCP_PACKET_MAX;
        public bool useAliasEntityID = true;
        public bool isOnInitCallPropertysSetMethods = true;

        public MWorld_KBE mMWorld_KBE;  // 场景

        public void init()
        {
            installEvents();
            initKBEngine();

            mMWorld_KBE = new MWorld_KBE();
            mMWorld_KBE.Start();
        }

        public void dispose()
        {

        }

        public virtual void installEvents()
        {
        }

        public virtual void initKBEngine()
        {
            // 如果此处发生错误，请查看 Assets\Scripts\kbe_scripts\if_Entity_error_use______git_submodule_update_____kbengine_plugins_______open_this_file_and_I_will_tell_you.cs

            Dbg.debugLevel = debugLevel;

            KBEngineArgs args = new KBEngineArgs();

            args.ip = ip;
            args.port = port;
            args.clientType = clientType;

            if (persistentDataPath == "Application.persistentDataPath")
                args.persistentDataPath = Application.persistentDataPath;
            else
                args.persistentDataPath = persistentDataPath;

            args.syncPlayer = syncPlayer;
            args.threadUpdateHZ = threadUpdateHZ;
            args.useAliasEntityID = useAliasEntityID;
            args.isOnInitCallPropertysSetMethods = isOnInitCallPropertysSetMethods;

            args.SEND_BUFFER_MAX = (UInt32)SEND_BUFFER_MAX;
            args.RECV_BUFFER_MAX = (UInt32)RECV_BUFFER_MAX;

            args.isMultiThreads = isMultiThreads;

            if (isMultiThreads)
                gameapp = new KBEngineAppThread(args);
            else
                gameapp = new KBEngineApp(args);
        }

        public void OnDestroy()
        {
            MonoBehaviour.print("clientapp::OnDestroy(): begin");
            if (KBEngineApp.app != null)
            {
                KBEngineApp.app.destroy();
                KBEngineApp.app = null;
            }
            MonoBehaviour.print("clientapp::OnDestroy(): end");
        }



        public void FixedUpdate()
        {
            KBEUpdate();
            if(null != this.mMWorld_KBE)
            {
                this.mMWorld_KBE.Update();
            }
        }

        public virtual void KBEUpdate()
        {
            // 单线程模式必须自己调用
            if (!isMultiThreads)
                gameapp.processNew();

            KBEngine.Event.processOutEvents();
        }
    }
}
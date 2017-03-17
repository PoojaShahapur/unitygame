using GameBox.Framework;
using GameBox.Service.GiantLightServer;

namespace SDK.Lib
{
    /**
     * @brief 不依赖模块的网络事件处理
     */
    public class NetEventHandle
    {
        public NetEventHandle()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void Disconnect()
        {
            var server = ServiceCenter.GetService<IGiantLightServer>();
            server.Disconnect();
            OnDisconnect();
        }

        public void OnDisconnect()
        {
            
        }
    }
}
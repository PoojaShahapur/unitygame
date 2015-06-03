using Mono.Xml;
using SDK.Common;
using SDK.Lib;
using System.Collections;
using System.Security;

namespace BehaviorLibrary
{
    /**
     * @brief 行为树资源
     */
    public class BehaviorTreeRes : InsResBase
    {
        public BehaviorTreeRes()
        {
            
        }

        override public void init(ResItem res)
        {
            string text = res.getText(GetPath());
            Ctx.m_instance.m_aiSystem.behaviorTreeMgr.parseXml(text);

            base.init(res);
        }

        override public void failed(ResItem res)
        {
            base.failed(res);
        }

        override public void unload()
        {

        }
    }
}
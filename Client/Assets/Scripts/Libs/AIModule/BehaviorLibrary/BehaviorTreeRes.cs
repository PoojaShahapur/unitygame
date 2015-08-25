using Mono.Xml;
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

        override protected void initImpl(ResItem res)
        {
            string text = res.getText(GetPath());
            Ctx.m_instance.m_aiSystem.behaviorTreeMgr.parseXml(text);

            base.initImpl(res);
        }

        override public void failed(ResItem res)
        {
            base.failed(res);
        }

        override public void unload()
        {
            base.unload();
        }
    }
}
using Game.Msg;
using SDK.Common;

namespace Game.Login
{
    public class PropertyUserCmdHandle : NetCmdHandleBase
    {
        public PropertyUserCmdHandle()
        {
            m_id2HandleDic[stPropertyUserCmd.REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER] = psstRemoveObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER] = psstRefCountObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.USEUSEROBJECT_PROPERTY_USERCMD_PARAMETER] = psstUseObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER] = psstAddMobileObjectListPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER] = psstAddMobileObjectPropertyUserCmd;
            m_id2HandleDic[stPropertyUserCmd.REQ_BUY_MARKET_MOBILE_OBJECT_CMD] = psstReqBuyMobileObjectPropertyUserCmd;
        }

        protected void psstRemoveObjectPropertyUserCmd(IByteArray msg)
        {

        }

        protected void psstRefCountObjectPropertyUserCmd(IByteArray msg)
        {

        }

        protected void psstUseObjectPropertyUserCmd(IByteArray msg)
        {

        }

        protected void psstAddMobileObjectListPropertyUserCmd(IByteArray msg)
        {

        }

        protected void psstAddMobileObjectPropertyUserCmd(IByteArray msg)
        {

        }

        protected void psstReqBuyMobileObjectPropertyUserCmd(IByteArray msg)
        {

        }
    }
}
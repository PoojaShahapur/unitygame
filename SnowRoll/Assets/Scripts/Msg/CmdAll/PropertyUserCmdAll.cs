using SDK.Lib;
using System.Collections.Generic;

namespace Game.Msg
{
    public class stRemoveObjectPropertyUserCmd : stPropertyUserCmd
    {
        public uint qwThisID;

        public stRemoveObjectPropertyUserCmd()
        {
            byParam = REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref qwThisID);
        }
    }

    //#define REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER 2
    //struct stRemoveObjectPropertyUserCmd : public stPropertyUserCmd {
    //stRemoveObjectPropertyUserCmd()
    //{
    //    byParam = REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
    //}
    //DWORD qwThisID;        /**< 物品唯一ID */
    //};

    public class stRefCountObjectPropertyUserCmd : stPropertyUserCmd
    {
        public uint qwThisID;
        public uint dwNum;
        public byte type;

        public stRefCountObjectPropertyUserCmd()
        {
            byParam = REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref qwThisID);
            bu.readUnsignedInt32(ref dwNum);
            bu.readUnsignedInt8(ref type);
        }
    }

    //#define REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER 6
    //struct stRefCountObjectPropertyUserCmd : public stPropertyUserCmd{
    //stRefCountObjectPropertyUserCmd()
    //{
    //    byParam = REFCOUNTOBJECT_PROPERTY_USERCMD_PARAMETER;
    //    type = 255;
    //}
    //DWORD qwThisID;        /**< 物品唯一ID */
    //DWORD dwNum;        /**< 数量 */
    //BYTE type;
    //};

    // 请求使用道具，主要是开礼包
    public class stUseObjectPropertyUserCmd : stPropertyUserCmd
    {
        public uint qwThisID;
        public uint dwNumber;
        public byte useType;
        public byte flag;

        public stUseObjectPropertyUserCmd()
        {
            byParam = USEUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt32(qwThisID);
            bu.writeUnsignedInt32(dwNumber);
            bu.writeUnsignedInt8(useType);
            bu.writeUnsignedInt8(flag);
        }
    }

    //#define USEUSEROBJECT_PROPERTY_USERCMD_PARAMETER 7
    //struct stUseObjectPropertyUserCmd : public  stPropertyUserCmd{
    //stUseObjectPropertyUserCmd()
    //{
    //    byParam = USEUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
    //    qwThisID = 0;
    //    dwNumber = 0;
    //    useType = 0;
    //    flag = 0;
    //}
    //DWORD qwThisID;        /**< 物品唯一ID */
    //DWORD dwNumber;        /**< 使用的数量 */
    //BYTE useType;
    //BYTE flag;
    //};

    public struct stObjectOperator
    {
        public byte byActionType;
        public t_Object_mobile mobject;

        public void derialize(ByteBuffer bu)
        {
            bu.readUnsignedInt8(ref byActionType);
            mobject = new t_Object_mobile();
            mobject.derialize(bu);
        }
    }

    public class stAddMobileObjectListPropertyUserCmd : stPropertyUserCmd
    {
        public ushort num;
        public List<stObjectOperator> list;

        public stAddMobileObjectListPropertyUserCmd()
        {
            byParam = ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt16(ref num);

            list = new List<stObjectOperator>();
            stObjectOperator item;
            int idx = 0;
            while(idx < num)
            {
                item = new stObjectOperator();
                item.derialize(bu);
                list.Add(item);
                ++idx;
            }
        }
    }

    //const BYTE ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER = 42;
    //struct stAddMobileObjectListPropertyUserCmd : public stPropertyUserCmd{
    //stAddMobileObjectListPropertyUserCmd()
    //{
    //    byParam = ADDUSER_MOBJECT_LIST_PROPERTY_USERCMD_PARAMETER;
    //    num=0;
    //}
    //WORD num;
    //struct
    //{
    //    BYTE byActionType;      /**< 物品动作类型 */
    //    t_Object_mobile object;      /**< 物品数据 */
    //}list[0];
    //};

    public class stAddMobileObjectPropertyUserCmd : stPropertyUserCmd
    {
        public byte byActionType;
        public t_Object_mobile mobject;

        public stAddMobileObjectPropertyUserCmd()
        {
            byParam = ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt8(ref byActionType);
            mobject = new t_Object_mobile();
            mobject.derialize(bu);
        }
    }


    //const BYTE ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER = 43;
    //struct stAddMobileObjectPropertyUserCmd : public stPropertyUserCmd{
    //stAddMobileObjectPropertyUserCmd()
    //{
    //    byParam = ADDUSER_MOBJECT_PROPERTY_USERCMD_PARAMETER;
    //}
    //BYTE byActionType;      /**< 物品动作类型 */
    //t_Object_mobile object;      /**< 物品数据 */
    //};

    public class stReqBuyMobileObjectPropertyUserCmd : stPropertyUserCmd
    {
        public ushort index;

        public stReqBuyMobileObjectPropertyUserCmd()
        {
            byParam = REQ_BUY_MARKET_MOBILE_OBJECT_CMD;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt16(index);
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt16(ref index);
        }
    }

    //const BYTE REQ_BUY_MARKET_MOBILE_OBJECT_CMD = 44;
    //struct stReqBuyMobileObjectPropertyUserCmd : public stPropertyUserCmd{
    //stReqBuyMobileObjectPropertyUserCmd()
    //{    
    //byParam = REQ_BUY_MARKET_MOBILE_OBJECT_CMD;
    //index = 0; 
    //}    
    //WORD index;
    //};

    public class stNotifyMarketAllObjectPropertyUserCmd : stPropertyUserCmd
    {
        public ushort count;
        public List<ushort> id;

        public stNotifyMarketAllObjectPropertyUserCmd()
        {
            byParam = NOFITY_MARKET_ALL_OBJECT_CMD;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt16(ref count);
            if(count > 0)
            {
                id = new List<ushort>(count);
                int idx = 0;
                ushort ret = 0;
                while(idx < count)
                {
                    bu.readUnsignedInt16(ref ret);
                    id.Add(ret);
                    ++idx;
                }
            }
        }
    }

    //const BYTE NOFITY_MARKET_ALL_OBJECT_CMD = 45;
    //struct stNotifyMarketAllObjectPropertyUserCmd : public stPropertyUserCmd
    //{
    //    stNotifyMarketAllObjectPropertyUserCmd()
    //    {    
    //        byParam = NOFITY_MARKET_ALL_OBJECT_CMD;
    //        count = 0; 
    //    }    
    //    WORD count;
    //    WORD id[0];    //商城售卖索引
    //};

    public class stReqMarketObjectInfoPropertyUserCmd : stPropertyUserCmd
    {
        public stReqMarketObjectInfoPropertyUserCmd()
        {
            byParam = REQ_MARKET_OBJECT_INFO_CMD;
        }
    }

    //const BYTE REQ_MARKET_OBJECT_INFO_CMD = 46;
    //struct stReqMarketObjectInfoPropertyUserCmd : public stPropertyUserCmd
    //{
    //    stReqMarketObjectInfoPropertyUserCmd()
    //    {    
    //        byParam = REQ_MARKET_OBJECT_INFO_CMD;
    //    }    
    //};

    public class stReqUserBaseDataInfoPropertyUserCmd : stPropertyUserCmd
    {
        public stReqUserBaseDataInfoPropertyUserCmd()
        {
            byParam = REQ_USER_BASE_DATA_INFO_CMD;
        }

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
        }
    }

    //const BYTE REQ_USER_BASE_DATA_INFO_CMD = 47;
    //struct stReqUserBaseDataInfoPropertyUserCmd : public stPropertyUserCmd
    //{
    //    stReqUserBaseDataInfoPropertyUserCmd()
    //    {    
    //        byParam = REQ_USER_BASE_DATA_INFO_CMD;
    //    }    
    //};
}
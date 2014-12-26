using SDK.Common;
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            qwThisID = ba.readUnsignedInt();
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            qwThisID = ba.readUnsignedInt();
            dwNum = ba.readUnsignedInt();
            type = ba.readUnsignedByte();
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            qwThisID = ba.readUnsignedInt();
            dwNumber = ba.readUnsignedInt();
            useType = ba.readUnsignedByte();
            flag = ba.readUnsignedByte();
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

        public void derialize(IByteArray ba)
        {
            byActionType = ba.readUnsignedByte();
            mobject = new t_Object_mobile();
            mobject.derialize(ba);
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            num = ba.readUnsignedShort();

            list = new List<stObjectOperator>();
            stObjectOperator item;
            int idx = 0;
            while(idx < num)
            {
                item = new stObjectOperator();
                item.derialize(ba);
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            byActionType = ba.readUnsignedByte();
            mobject = new t_Object_mobile();
            mobject.derialize(ba);
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

        public override void derialize(IByteArray ba)
        {
            base.derialize(ba);
            index = ba.readUnsignedShort();
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
}
using Game.UI;

namespace SDK.Lib
{
    public class t_MainUserData
    {
        public uint m_dwUserTempID;             // 用户临时 id
        public string m_name = "";                   //玩家名字
        public uint m_gold;                     //玩家的金币

        public void derialize(ByteBuffer ba)
        {
            ba.readMultiByte(ref m_name, CVMsg.MAX_NAMESIZE + 1, GkEncode.UTF8);
            ba.readUnsignedInt32(ref m_gold);
        }
    }
}
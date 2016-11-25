namespace SDK.Lib
{
    public class TableStateItemBody : TableItemBodyBase
    {
        public string mName;           // 名称
        public string mRes;            // 资源
        public int mEffectId;          // 特效 Id

        override public void parseBodyByteBuffer(ByteBuffer bytes, uint offset)
        {
            bytes.position = offset;
            UtilTable.readString(bytes, ref mName);
            UtilTable.readString(bytes, ref mRes);
            bytes.readInt32(ref mEffectId);

            initDefaultValue();
        }

        protected void initDefaultValue()
        {
            if(mEffectId == 0)
            {
                mEffectId = 0;
            }
        }
    }
}
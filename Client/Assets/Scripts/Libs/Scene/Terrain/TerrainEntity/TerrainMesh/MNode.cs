namespace SDK.Lib
{
    public class MNode : AuxComponent
    {
        public void setPosition(MVector3 pos)
        {
            UtilApi.setPos(this.selfGo.transform, pos.toNative());
        }
    }
}
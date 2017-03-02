using SDK.Lib;

namespace Game.UI
{
    public class PackScrollView : AuxScrollView
    {
        override public AuxScrollViewItemBase createDataItem()
        {
            return new PackItem();
        }
    }
}
namespace SDK.Lib
{
    public class BeingControlBase : ControlBase
    {
        public BeingControlBase(BeingEntity rhv) : 
            base(rhv)
        {

        }

        public BeingEntity m_being
        {
            get
            {
                return m_entity as BeingEntity;
            }
        }
    }
}
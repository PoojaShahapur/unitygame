namespace SDK.Lib
{
    public class AuxButtonUserData : AuxUserData, UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IPointerUpHandler, UnityEngine.EventSystems.IPointerExitHandler
    {
        protected AuxButton mData;

        //public AuxButton getUserData()
        //{
        //    return this.getUserData<AuxButton>();
        //}

        //public AuxButton addUserData()
        //{
        //    if (mData == null)
        //    {
        //        mData = new AuxButton(this.gameObject);
        //    }

        //    return mData as AuxButton;
        //}

        public AuxButton getUserData()
        {
            return this.mData;
        }

        public AuxButton addUserData()
        {
            if (mData == null)
            {
                mData = new AuxButton(this.gameObject);
            }

            return mData as AuxButton;
        }

        public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if(null != this.mData)
            {
                this.mData.OnPointerDown(eventData);
            }
        }

        public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (null != this.mData)
            {
                this.mData.OnPointerUp(eventData);
            }
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (null != this.mData)
            {
                this.mData.OnPointerExit(eventData);
            }
        }
    }
}
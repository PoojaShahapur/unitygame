using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief GameObject
     */
    public class AuxGo
    {
        protected GameObject mSelfGo;

        public AuxGo(GameObject go_ = null)
        {
            this.mSelfGo = go_;
        }

        public void setSelfGo(GameObject go_)
        {
            this.mSelfGo = go_;
        }

        public void setSelfGo(GameObject pntNode, string path)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        }

        public void setGo(GameObject pntNode, string path)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        }

        public void setVisible(bool isShow)
        {
            UtilApi.SetActive(this.mSelfGo, isShow);
        }
    }
}
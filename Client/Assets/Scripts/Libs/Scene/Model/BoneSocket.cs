using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 骨头上的 socket
     */
    public class BoneSocket : AuxComponent
    {
        protected string m_bonePath;

        public BoneSocket()
        {
            this.bNeedPlaceHolderGo = true;
        }

        public string bonePath
        {
            get
            {
                return m_bonePath;
            }
            set
            {
                m_bonePath = value;
            }
        }
    }

    public class BoneSockets
    {
        protected BoneSocket[] m_boneSocketArr;

        public BoneSockets(int boneNum)
        {
            m_boneSocketArr = new BoneSocket[boneNum];

            for(int idx = 0; idx < boneNum; ++idx)
            {
                m_boneSocketArr[idx] = new BoneSocket();
            }
        }

        public BoneSocket[] boneSocketArr
        {
            get
            {
                return m_boneSocketArr;
            }
            set
            {
                m_boneSocketArr = value;
            }
        }

        public void addSocket(int id, string path)
        {
            m_boneSocketArr[id].bonePath = path;
        }

        public void addSocketLinkObj(int id, GameObject go_)
        {
            UtilApi.SetParent(go_, m_boneSocketArr[id].placeHolderGo);
        }

        public BoneSocket getSocket(int id)
        {
            return m_boneSocketArr[id];
        }

        public void setSkelAnim(GameObject go_)
        {
            GameObject _go = null;
            foreach(var socket in m_boneSocketArr)
            {
                _go = UtilApi.TransFindChildByPObjAndPath(go_, socket.bonePath);
                socket.pntGo = _go;
                socket.linkPlaceHolder2Parent();
            }
        }
    }
}
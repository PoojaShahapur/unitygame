using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class Res : MonoBehaviour
    {
        protected ResType m_type;
        protected string m_path;

        protected delegate void Init();
        protected Init onInited;

        public Res(string path)
        {
            m_path = path;
        }

        public ResType type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        public string path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        virtual public void init(LoadItem item)
        {

        }

        virtual public IEnumerator initAsset()
        {
            return null;
        }
    }
}
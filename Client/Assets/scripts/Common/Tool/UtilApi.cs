using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief 对 api 的进一步 wrap 
     */
    public class UtilApi
    {
        public const string TRUE = "true";
        public const string FALSE = "false";

        // 仅仅根据名字查找 GameObject ，注意如果 GameObject 设置 SetActive 为 false ，就会查找不到，如果有相同名字的 GameObject ，不保证查找到正确的。
        static public GameObject GoFindChildByPObjAndName(string name)
        {
            return GameObject.Find(name);
        }

        // 通过父对象和完整的目录查找 child 对象
        static public GameObject TransFindChildByPObjAndPath(GameObject pObject, string path)
        {
            return pObject.transform.Find(path).gameObject;
        }

        // 从 Parent 获取一个组件
        static public T getComByP<T>(GameObject go, string path) where T : Component
        {
            return go.transform.Find(path).GetComponent<T>();
        }

        // 从 Parent 获取一个组件
        static public T getComByP<T>(GameObject go) where T : Component
        {
            return go.GetComponent<T>();
        }

        // 设置 Label 的显示
        static public void setLblStype(Text textWidget, TextStyleID styleID)
        {

        }

        // 按钮点击统一处理
        static public void onBtnClkHandle(BtnStyleID btnID)
        {

        }

        // 添加事件处理
        //public static void addEventHandle(GameObject go, string path, UIEventListener.VoidDelegate handle)
        //{
        //    UIEventListener.Get(go.transform.Find(path).gameObject).onClick = handle;
        //}
        public static void addEventHandle(GameObject go, string path, UnityAction handle)
        {
            go.transform.Find(path).GetComponent<Button>().onClick.AddListener(handle);
        }


        // 销毁对象
        public static void Destroy(Object obj)
        {
            GameObject.Destroy(obj);
        }

        // 立即销毁对象
        public static void DestroyImmediate(Object obj)
        {
            GameObject.DestroyImmediate(obj);
        }

        public static void DestroyImmediate(Object obj, bool allowDestroyingAssets)
        {
            GameObject.DestroyImmediate(obj, allowDestroyingAssets);
        }

        public static void DontDestroyOnLoad(Object target)
        {
            Object.DontDestroyOnLoad(target);
        }

        public static void SetActive(GameObject target, bool bshow)
        {
            target.SetActive(bshow);
        }

        public static Object Instantiate(Object original)
        {
            return Object.Instantiate(original);
        }

        public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(original, position, rotation);
        }

        public static void normalPosScale(Transform tran)
        {
            //tran.localPosition = Vector3.zero;
            tran.localPosition = new Vector3(0, 0, 0);
            tran.localScale = Vector3.one;
        }

        static public bool getXmlAttrBool(XmlAttribute attr)
        {
            if (attr != null)
            {
                if (TRUE == attr.Value)
                {
                    return true;
                }
                else if (FALSE == attr.Value)
                {
                    return false;
                }
            }

            return false;
        }

        static public string getXmlAttrStr(XmlAttribute attr)
        {
            if (attr != null)
            {
                return attr.Value;
            }

            return "";
        }

        static public uint getXmlAttrUInt(XmlAttribute attr)
        {
            uint ret = 0;
            if (attr != null)
            {
                uint.TryParse(attr.Value, out ret);
            }

            return ret;
        }

        // 转换行为状态到生物状态
        static public BeingState convBehaviorState2BeingState(BehaviorState behaviorState)
        {
            BeingState retState = BeingState.BSIdle;
            switch(behaviorState)
            {
                case BehaviorState.BSIdle:
                    {
                        retState = BeingState.BSIdle;
                    }
                    break;
                case BehaviorState.BSWander:
                    {
                        retState = BeingState.BSWalk;
                    }
                    break;
                case BehaviorState.BSFollow:
                    {
                        retState = BeingState.BSWalk;
                    }
                    break;
            }

            return retState;
        }

        static public ActState convBeingState2ActState(BeingState beingState, BeingSubState beingSubState)
        {
            switch(beingState)
            {
                case BeingState.BSIdle:
                    {
                        return ActState.ActIdle;
                    }
                case BeingState.BSWalk:
                    {
                        return ActState.ActWalk;
                    }
                case BeingState.BSRun:
                    {
                        return ActState.ActRun;
                    }
            }

            return ActState.ActIdle;
        }

        // 剔除字符串末尾的空格
        public static void trimEndSpace(ref string str)
        {
            str.TrimEnd('\0');
        }
    }
}
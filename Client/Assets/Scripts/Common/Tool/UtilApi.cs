using SDK.Lib;
using System.IO;
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

        public static GameObject[] FindGameObjectsWithTag(string tag)
        {
            return GameObject.FindGameObjectsWithTag("Untagged");
        }

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

        // 从 Parent 获取一个组件
        static public T getComByP<T>(string path) where T : Component
        {
            return GameObject.Find(path).GetComponent<T>();
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
        public static void addEventHandle(GameObject go, string path, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go.transform.Find(path).gameObject).onClick = handle;
        }

        public static void addEventHandle(GameObject go, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go).onClick = handle;
        }

        public static void addHoverHandle(GameObject go, UIEventListener.BoolDelegate handle)
        {
            UIEventListener.Get(go).onHover = handle;
        }

        public static void addEventHandle(GameObject go, string path, UnityAction handle)
        {
            go.transform.Find(path).GetComponent<Button>().onClick.AddListener(handle);
        }

        public static void addEventHandle(GameObject go, UnityAction handle)
        {
            go.GetComponent<Button>().onClick.AddListener(handle);
        }

        // 销毁对象
        public static void Destroy(Object obj)
        {
            if (obj as GameObject)
            {
                (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
            }
            GameObject.Destroy(obj);
            obj = null;
        }

        // 立即销毁对象
        public static void DestroyImmediate(Object obj)
        {
            if (obj as GameObject)
            {
                (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
            }
            GameObject.DestroyImmediate(obj);
        }

        public static void DestroyImmediate(Object obj, bool allowDestroyingAssets)
        {
            if (obj as GameObject)
            {
                (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
            }
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

        public static void normalPos(Transform tran)
        {
            tran.localPosition = Vector3.zero;
        }

        public static void normalRot(Transform tran)
        {
            tran.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // 卸载内部 Resources 管理的共享的那块资源，注意这个是异步事件
        public static AsyncOperation UnloadUnusedAssets()
        {
            AsyncOperation opt = Resources.UnloadUnusedAssets();
            return opt;
        }

        // 小心使用这个资源，这个函数把共享资源卸载掉了，如果有引用，就会有问题
        public static void UnloadAsset(Object assetToUnload)
        {
            Resources.UnloadAsset(assetToUnload);
        }

        // 从场景图中移除,  worldPositionStays 是否在两个 local 中移动保持 world 信息不变，如果要保持 local 信息不变，就设置成 false ，通常 UI 需要设置成  false
        public static void removeFromSceneGraph(Transform trans, bool worldPositionStays = true)
        {
            trans.SetParent(null);      // 这个仅仅是移除场景中
        }

        public static void SetParent(Transform child, Transform parent, bool worldPositionStays = true)
        {
            child.SetParent(parent, worldPositionStays);
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

        // 判断两个 GameObject 地址是否相等
        public static bool isAddressEqual(GameObject a, GameObject b)
        {
            return a == b;
        }

        // 赋值卡牌显示
        public static void updateCardDataNoChange(TableCardItemBody cardTableItem, GameObject gameObject)
        {
            Text text;
            text = UtilApi.getComByP<Text>(gameObject, "name/Text");         // 名字
            text.text = cardTableItem.m_name;

            text = UtilApi.getComByP<Text>(gameObject, "description/Text");  // 描述
            string desc = "";
            if (cardTableItem.m_chaoFeng == 1)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.ChaoFeng;
            }
            if (cardTableItem.m_chongFeng == 1)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.ChongFeng;
            }
            if (cardTableItem.m_fengNu == 1)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.FengNu;
            }
            if (cardTableItem.m_qianXing == 1)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.QianXing;
            }
            if (cardTableItem.m_shengDun == 1)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.ShengDun;
            }

            if (cardTableItem.m_magicConsume > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.MoFaXiaoHao;
            }
            if (cardTableItem.m_attack > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.GongJiLi;
            }
            if (cardTableItem.m_hp > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.Xueliang;
            }
            if (cardTableItem.m_Durable > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.NaiJiu;
            }
            if (cardTableItem.m_mpAdded > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.FaShuShangHai;
            }
            if (cardTableItem.m_guoZai > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                desc += TableCardAttrName.GuoZai;
            }

            TableSkillItemBody tableSkillItem;
            // 技能
            if (cardTableItem.m_faShu > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)cardTableItem.m_faShu).m_itemBody as TableSkillItemBody;
                desc += tableSkillItem.m_desc;
            }
            if (cardTableItem.m_zhanHou > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)cardTableItem.m_zhanHou).m_itemBody as TableSkillItemBody;
                desc += tableSkillItem.m_desc;
            }
            if (cardTableItem.m_wangYu > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)cardTableItem.m_wangYu).m_itemBody as TableSkillItemBody;
                desc += tableSkillItem.m_desc;
            }
            if (cardTableItem.m_jiNu > 0)
            {
                if (desc.Length > 0)
                {
                    desc += "\n";
                }
                tableSkillItem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SKILL, (uint)cardTableItem.m_jiNu).m_itemBody as TableSkillItemBody;
                desc += tableSkillItem.m_desc;
            }

            text.text = desc;
        }

        public static void updateCardDataChange(TableCardItemBody cardTableItem, GameObject gameObject)
        {
            Text text;
            text = UtilApi.getComByP<Text>(gameObject, "attack/Text");       // 攻击
            text.text = cardTableItem.m_attack.ToString();
            text = UtilApi.getComByP<Text>(gameObject, "cost/Text");         // Magic
            text.text = cardTableItem.m_magicConsume.ToString();
            text = UtilApi.getComByP<Text>(gameObject, "health/Text");       // HP
            text.text = cardTableItem.m_hp.ToString();
        }

        static long scurTime;
        static System.TimeSpan ts;

        // 返回 UTC 秒
        public static uint getUTCSec()
        {
            scurTime = System.DateTime.Now.Ticks;
            ts = new System.TimeSpan(scurTime);
            return (uint)(ts.TotalSeconds);
        }

        // 获取当前时间的文本可读形式
        public static string getUTCFormatText()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public static void loadRes<T>(string path, System.Action<IDispatchObject> onload, System.Action unload, InsResBase res)
        {
            bool needLoad = true;

            if (res != null)
            {
                if (res.GetPath() != path)
                {
                    unload();
                }
                else
                {
                    needLoad = false;
                }
            }
            if (needLoad)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.m_path = path;
                    param.m_loaded = onload;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }

        public static int Range(int min, int max)
        {
            Random.seed = (int)UtilApi.getUTCSec();
            return Random.Range(min, max);
        }

        // 递归创建子目录
        public static void recureCreateSubDir(string rootPath, string subPath, bool includeLast = false)
        {
            normalPath(ref subPath);
            if(!includeLast)
            {
                if(subPath.IndexOf('/') == -1)
                {
                    return;
                }
                subPath = subPath.Substring(0, subPath.LastIndexOf('/'));
            }

            if(Directory.Exists(Path.Combine(rootPath, subPath)))
            {
                return;
            }

            int startIdx = 0;
            int splitIdx = 0;
            while((splitIdx  = subPath.IndexOf('/', startIdx)) != -1)
            {
                if (!Directory.Exists(Path.Combine(rootPath, subPath.Substring(0, startIdx + splitIdx))))
                {
                    Directory.CreateDirectory(Path.Combine(rootPath, subPath.Substring(0, startIdx + splitIdx)));
                }

                startIdx += splitIdx;
                startIdx += 1;
            }

            Directory.CreateDirectory(Path.Combine(rootPath, subPath));
        }

        public static void normalPath(ref string path)
        {
            path = path.Replace('\\', '/');
        }

        // 添加版本的文件名，例如 E:/aaa/bbb/ccc.txt?v=1024
        public static string versionPath(string path, string version)
        {
            if (!string.IsNullOrEmpty(version))
            {
                return string.Format("{0}?v={1}", path, version);
            }
            else
            {
                return path;
            }
        }

        // 删除所有除去版本号外相同的文件，例如 E:/aaa/bbb/ccc.txt?v=1024 ，只要 E:/aaa/bbb/ccc.txt 一样就删除，参数就是 E:/aaa/bbb/ccc.txt ，没有版本号的文件
        public static void delFileNoVer(string path)
        {
            normalPath(ref path);
            DirectoryInfo TheFolder = new DirectoryInfo(path.Substring(0, path.LastIndexOf('/')));
            FileInfo[] allFiles = TheFolder.GetFiles(string.Format("{0}*", path));
            foreach (var item in allFiles)
            {
                item.Delete();
            }
        }

        public static bool fileExistNoVer(string path)
        {
            normalPath(ref path);
            DirectoryInfo TheFolder = new DirectoryInfo(path.Substring(0, path.LastIndexOf('/')));
            FileInfo[] allFiles = TheFolder.GetFiles(string.Format("{0}*", path));

            return allFiles.Length > 0;
        }

        public static string getRelPath(string path)
        {
            if(path.IndexOf(Ctx.m_instance.m_cfg.m_webIP) != -1)
            {
                return path.Substring(Ctx.m_instance.m_cfg.m_webIP.Length);
            }

            return path;
        }

        public static string getPathNoVer(string path)
        {
            if (path.IndexOf('?') != -1)
            {
                return path.Substring(0, path.IndexOf('?'));
            }

            return path;
        }

        // 判断一个 unicode 字符是不是汉字
        public static bool IsChineseLetter(string input, int index)
        {
            int code = 0;
            int chfrom = System.Convert.ToInt32("4e00", 16); //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = System.Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = System.Char.ConvertToUtf32(input, index); //获得字符串input中指定索引index处字符unicode编码

                if (code >= chfrom && code <= chend)
                {
                    return true; //当code在中文范围内返回true
                }
                else
                {
                    return false; //当code不在中文范围内返回false
                }
            }
            return false;
        }

        public static bool IsIncludeChinese(string input)
        {
            int idx = 0;
            for(idx = 0; idx < input.Length; ++idx)
            {
                if(IsChineseLetter(input, idx))
                {
                    return true;
                }
            }

            return false;
        }

        // 判断 unicode 字符个数，只判断字母和中文吗，中文算 2 个字节
        public static int CalcCharCount(string str)
        {
            int charCount = 0;
            int idx = 0;
            for(idx = 0; idx < str.Length; ++idx)
            {
                if(IsChineseLetter(str, idx))
                {
                    charCount += 2;
                }
                else
                {
                    charCount += 1;
                }
            }

            return charCount;
        }

        public static string getPakPathAndExt(string path, string extName)
        {
            return string.Format("{0}.{1}", path, extName);
        }
    }
}
using SDK.Common;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SDK.Common
{
    public class UtilLogic
    {
        // 转换行为状态到生物状态
        static public BeingState convBehaviorState2BeingState(BehaviorState behaviorState)
        {
            BeingState retState = BeingState.BSIdle;
            switch (behaviorState)
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
            switch (beingState)
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

        // 赋值卡牌显示
        public static void updateCardDataNoChangeByTable(TableCardItemBody cardTableItem, GameObject gameObject)
        {
            AuxLabel text;
            text = new AuxLabel(gameObject, "UIRoot/NameText");         // 名字
            text.text = cardTableItem.m_name;
            text.setSelfGo(gameObject, "UIRoot/DescText");  // 描述
            text.text = cardTableItem.m_cardDesc;
        }

        public static void updateCardDataChangeByTable(TableCardItemBody cardTableItem, GameObject gameObject)
        {
            AuxLabel text;
            text = new AuxLabel(gameObject, "UIRoot/AttText");       // 攻击
            text.text = cardTableItem.m_attack.ToString();
            text.setSelfGo(gameObject, "UIRoot/MpText");         // Magic
            text.text = cardTableItem.m_magicConsume.ToString();
            text.setSelfGo(gameObject, "UIRoot/HpText");       // HP
            text.text = cardTableItem.m_hp.ToString();
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
                    param.m_loadEventHandle = onload;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }

        public static string combineVerPath(string path, string ver)
        {
            return string.Format("{0}_v={1}", path, ver);
        }

        public static string webFullPath(string path)
        {
            return string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_webIP, path);
        }

        public static string getRelPath(string path)
        {
            if (path.IndexOf(Ctx.m_instance.m_cfg.m_webIP) != -1)
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
            for (idx = 0; idx < input.Length; ++idx)
            {
                if (IsChineseLetter(input, idx))
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
            for (idx = 0; idx < str.Length; ++idx)
            {
                if (IsChineseLetter(str, idx))
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

        public static string convScenePath2LevelName(string path)
        {
            int slashIdx = path.LastIndexOf("/");
            int dotIdx = path.IndexOf(".");
            string retLevelName = "";
            if (slashIdx != -1)
            {
                if (dotIdx != -1)
                {
                    retLevelName = path.Substring(slashIdx + 1, dotIdx - slashIdx - 1);
                }
                else
                {
                    retLevelName = path.Substring(slashIdx + 1);
                }
            }
            else
            {
                retLevelName = path;
            }

            return retLevelName;
        }

        // 加载一个表完成
        public static void onLoaded(IDispatchObject dispObj, Action<IDispatchObject> loadEventHandle)
        {
            ResItem res = dispObj as ResItem;
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), loadEventHandle);
        }

        public static void onFailed(IDispatchObject dispObj, Action<IDispatchObject> loadEventHandle)
        {
            ResItem res = dispObj as ResItem;
            Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, res.GetPath());

            // 卸载资源
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), loadEventHandle);
        }

        // 通过下划线获取最后的数字，例如 asdf_23 获取 23
        public static int findIdxByUnderline(string name)
        {
            int idx = name.LastIndexOf("_");
            int ret = 0;
            if (-1 != idx)
            {
                bool bSuccess = Int32.TryParse(name.Substring(idx + 1, name.Length - 1 - idx), out ret);
            }

            return ret;
        }

        public static string getImageByPinZhi(int pinzhi)
        {
            return string.Format("pinzhi_kapai_{0}", pinzhi);
        }

        // 从数字获取 5 位字符串
        public static string get5StrFromDigit(int digit)
        {
            string ret = "";
            if (digit < 10)
            {
                ret = string.Format("{0}{1}", "0000", digit.ToString());
            }
            else if (digit < 100)
            {
                ret = string.Format("{0}{1}", "000", digit.ToString());
            }

            return ret;
        }
    }
}
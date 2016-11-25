using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class UtilLogic
    {
        /* 
         * @brief 上半球分割
         * @param inter 矩形分割的时候表示间隔
         * @param radius 半径
         * @param splitCnt 分裂多少分
         * @param posList 返回的位置列表
         * @param rotList 返回的旋转列表
         */
        static protected void splitPos(int up, Transform trans, float inter, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            if (splitCnt > 3)        // 只有大于 3 个的时候才分割
            {
                if (up == 0)
                {
                    upHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                }
                else
                {
                    downHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                }
            }
            else
            {
                rectSplit(trans, inter, splitCnt, ref posList, ref rotList);
            }
        }

        static protected void rectSplit(Transform trans, float inter, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            float totalLen = splitCnt * inter;
            float startPos = -totalLen / 2;

            Vector3 pos;
            Quaternion rot;

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                pos.x = trans.localPosition.x + startPos + inter * listIdx;
                pos.y = trans.localPosition.y;
                pos.z = trans.localPosition.z;
                posList.Add(pos);

                rot = trans.localRotation;
                rotList.Add(rot);

                ++listIdx;
            }
        }

        // 180 - 360 度区间
        static protected void upHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            //float radianSector = 0;         // 每一个弧形的弧度
            float degSector = 0;            // 度
            //float curRadian = 0;
            float curDeg = 0;

            //float startRadian = 0;          // 开始的弧度
            float startDeg = 0;             // 开始的角度

            float yDelta = 0.4f;

            Vector3 pos;
            Quaternion rot;

            // 总共 10 张牌
            //radianSector = Mathf.PI / 11;           // 这个地方需要加 1 
            degSector = 180 / 11;

            //startRadian = Mathf.PI + radianSector;
            startDeg = 180 + degSector;

            Vector3 orign = new Vector3(radius, 0, 0);

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                //curRadian = startRadian + radianSector * listIdx;
                curDeg = startDeg + degSector * listIdx;

                pos = new Vector3();
                pos = Quaternion.AngleAxis(curDeg, Vector3.up) * orign;
                pos += trans.localPosition;
                pos.y += listIdx * yDelta;

                posList.Add(pos);

                rot = Quaternion.Euler(0, curDeg + trans.eulerAngles.y + 90, 0);            // +90 就是为了是竖直的变成水平的，起始都偏移 90 度，这样就可以认为是 0 度了
                rotList.Add(rot);

                ++listIdx;
            }
        }

        // 0 - 180 度区间
        static protected void downHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            //float radianSector = 0;         // 每一个弧形的弧度
            float degSector = 0;            // 度
            //float curRadian = 0;
            float curDeg = 0;

            //float startRadian = 0;          // 开始的弧度
            float startDeg = 0;             // 开始的角度

            float yDelta = 0.4f;

            Vector3 pos;
            Quaternion rot;

            // 总共 10 张牌
            //radianSector = Mathf.PI / 11;           // 这个地方需要加 1 
            degSector = 180 / 11;

            //startRadian = Mathf.PI - radianSector;
            startDeg = 180 - degSector;

            Vector3 orign = new Vector3(radius, 0, 0);

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                //curRadian = startRadian - radianSector * listIdx;
                curDeg = startDeg - degSector * listIdx;

                pos = new Vector3();
                pos = Quaternion.AngleAxis(curDeg, Vector3.up) * orign;
                pos += trans.localPosition;
                pos.y += listIdx * yDelta;

                posList.Add(pos);

                rot = Quaternion.Euler(0, curDeg, 0);            // +90 就是为了是竖直的变成水平的，起始都偏移 90 度，这样就可以认为是 0 度了
                rotList.Add(rot);

                ++listIdx;
            }
        }

        static public float xzDis(Vector3 aPos, Vector3 bPos)
        {
            return Mathf.Sqrt((aPos.x - bPos.x) * (aPos.x - bPos.x) + (aPos.z - bPos.z) * (aPos.z - bPos.z));
        }

        /**
         * @param trans 起始位置
         * @param unitWidth 单元宽度
         * @param tileWidth 区域宽度
         * @param splitCnt 划分数量
         * @param posList 返回的位置列表
         */
        static public void newRectSplit(Transform trans, float unitWidth, float tileRadius, float fYDelta, int splitCnt, ref List<Vector3> posList)
        {
            Vector3 pos;
            int listIdx = 0;
            float halfUnitWidth = unitWidth / 2;
            if (unitWidth * splitCnt > 2 * tileRadius)       // 如果当前区域不能完整放下所有的单元
            {
                float plusOneWidth = (tileRadius * 2) - unitWidth;          // 最后一个必然要放在最后一个，并且不能超出边界
                float splitCellWidth = plusOneWidth / (splitCnt - 1);
                while (listIdx < splitCnt - 1)  // 最后一个位置左边界就是 plusOneWidth ，已经计算好了
                {
                    pos.x = trans.localPosition.x + splitCellWidth * listIdx - tileRadius;  // 这个是左边的位置
                    pos.x += halfUnitWidth;           // 调整中心点的位置
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }

                // 计算最后一个位置
                pos.x = trans.localPosition.x + plusOneWidth - tileRadius;  // 这个是左边的位置
                pos.x += halfUnitWidth;           // 调整中心点的位置
                pos.y = trans.localPosition.y + fYDelta * listIdx;
                pos.z = trans.localPosition.z;
                posList.Add(pos);
            }
            else            // 全部能放下，就居中显示
            {
                float halfWidth = (float)((unitWidth * splitCnt) * 0.5);        // 占用的区域的一半宽度
                while (listIdx < splitCnt)
                {
                    pos.x = trans.localPosition.x + unitWidth * listIdx - halfWidth;    // 这个是左边的位置
                    pos.x += halfUnitWidth;           // 调整中心点的位置
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }
            }
        }

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

        static public eBeingActId convBeingState2ActState(BeingState beingState, BeingSubState beingSubState)
        {
            switch (beingState)
            {
                case BeingState.BSIdle:
                    {
                        return eBeingActId.ActIdle;
                    }
                case BeingState.BSWalk:
                    {
                        return eBeingActId.ActWalk;
                    }
                case BeingState.BSRun:
                    {
                        return eBeingActId.ActRun;
                    }
            }

            return eBeingActId.ActIdle;
        }

        // 赋值卡牌显示
        public static void updateCardDataNoChangeByTable(TableCardItemBody cardTableItem, GameObject gameObject)
        {
            AuxLabel text;
            text = new AuxLabel(gameObject, "UIRoot/NameText");         // 名字
            text.text = cardTableItem.mName;
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

        public static void loadRes<T>(string path, MAction<IDispatchObject> onload, System.Action unload, InsResBase res)
        {
            bool needLoad = true;

            if (res != null)
            {
                if (res.getLogicPath() != path)
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
                    param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                    param.setPath(path);
                    param.mLoadEventHandle = onload;
                    Ctx.mInstance.mModelMgr.load<ModelRes>(param);
                    Ctx.mInstance.mPoolSys.deleteObj(param);
                }
            }
        }

        public static string combineVerPath(string path, string ver)
        {
            return string.Format("{0}_v={1}", path, ver);
        }

        public static string webFullPath(string path)
        {
            return string.Format("{0}{1}", Ctx.mInstance.mCfg.mWebIP, path);
        }

        public static string getRelPath(string path)
        {
            if (path.IndexOf(Ctx.mInstance.mCfg.mWebIP) != -1)
            {
                return path.Substring(Ctx.mInstance.mCfg.mWebIP.Length);
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
        public static void onLoaded(IDispatchObject dispObj, MAction<IDispatchObject> loadEventHandle)
        {
            ResItem res = dispObj as ResItem;
            Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem0, res.getLoadPath());

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), loadEventHandle);
        }

        public static void onFailed(IDispatchObject dispObj, MAction<IDispatchObject> loadEventHandle)
        {
            ResItem res = dispObj as ResItem;
            Ctx.mInstance.mLogSys.debugLog_1(LangItemID.eItem1, res.getLoadPath());

            // 卸载资源
            Ctx.mInstance.mResLoadMgr.unload(res.getResUniqueId(), loadEventHandle);
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

        // 格式化时间，显示格式为 00年00天00时00分00秒
        static public string formatTime(int second)
        {
            string ret = "";

            int left = 0;
            int year = second / (356 * 24 * 60 * 60);
            left = second % (356 * 24 * 60 * 60);
            int day = left / (24 * 60 * 60);
            left = left % (24 * 60 * 60);
            int hour = left / (60 * 60);
            left = left % (60 * 60);
            int min = left / 60;
            left = left % 60;
            int sec = left;

            if(year != 0)
            {
                ret = string.Format("{0}{1}年", ret, year);
            }
            if (day != 0)
            {
                ret = string.Format("{0}{1}天", ret, day);
            }
            if (hour != 0)
            {
                ret = string.Format("{0}{1}时", ret, hour);
            }
            if (min != 0)
            {
                ret = string.Format("{0}{1}分", ret, min);
            }
            if (sec != 0)
            {
                ret = string.Format("{0}{1}秒", ret, sec);
            }

            return ret;
        }
    }
}
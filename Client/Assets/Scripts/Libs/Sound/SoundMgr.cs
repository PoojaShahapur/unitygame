using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Lib;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 播放音乐和音效
     */
    public class SoundMgr 
    {
        protected List<SoundItem> m_audioList = new List<SoundItem>();
        protected Dictionary<string, SoundItem> m_path2SoundDic = new Dictionary<string,SoundItem>();
        protected TimerItemBase m_timer;
        protected List<SoundItem> m_clearList = new List<SoundItem>();

        public SoundMgr()
        {

        }

        public void play(SoundParam soundParam)
        {
            if (!soundParam.m_bLoop)
            {
                addTimer();
            }

            soundParam.m_path = Ctx.m_instance.m_pPakSys.getCurResPakPathByResPath(soundParam.m_path, null);

            if (m_path2SoundDic.ContainsKey(soundParam.m_path))      // 如果已经有了直接返回
            {
                if (!m_path2SoundDic[soundParam.m_path].bInCurState(SoundPlayState.eSS_Play))
                {
                    m_path2SoundDic[soundParam.m_path].Play();
                }
            }
            else
            {
                // 创建
                if (isPrefab(soundParam.m_path))
                {
                    m_path2SoundDic[soundParam.m_path] = new SoundPrefabItem();
                    m_path2SoundDic[soundParam.m_path].m_soundResType = SoundResType.eSRT_Prefab;
                }
                else
                {
                    m_path2SoundDic[soundParam.m_path] = new SoundClipItem();
                    m_path2SoundDic[soundParam.m_path].m_soundResType = SoundResType.eSRT_Clip;
                }
                m_audioList.Add(m_path2SoundDic[soundParam.m_path]);
                m_path2SoundDic[soundParam.m_path].initParam(soundParam);

                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = soundParam.m_path;
                param.m_loadEventHandle = onLoadEventHandle;
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void play(string path, bool loop_ = true)
        {
            if (m_path2SoundDic.ContainsKey(path))
            {
                //m_path2SoundDic[path].m_bLoop = loop_;
                m_path2SoundDic[path].Play();
            }
            else
            {
                SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
                param.m_path = path;
                param.m_bLoop = loop_;
                play(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void stop(string path)
        {
            path = Ctx.m_instance.m_pPakSys.getCurResPakPathByResPath(path, null);
            unload(path);
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());

                if (m_path2SoundDic.ContainsKey(res.GetPath()))      // 如果有，说明还没被停止
                {
                    if (m_path2SoundDic[res.GetPath()].m_soundResType == SoundResType.eSRT_Prefab)
                    {
                        m_path2SoundDic[res.GetPath()].setResObj(res.InstantiateObject(res.GetPath()));
                    }
                    else
                    {
                        m_path2SoundDic[res.GetPath()].setResObj(res.getObject(res.GetPath()));
                    }
                }
                // 播放音乐
                play(res.GetPath());
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, res.GetPath());
                delSoundItem(m_path2SoundDic[res.GetPath()]);
            }
            // 卸载数据
            Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);
        }

        protected void unload(string path)
        {
            if (m_path2SoundDic.ContainsKey(path))
            {
                m_path2SoundDic[path].unload();
                delSoundItem(m_path2SoundDic[path]);
            }
        }

        // 不要遍历中使用这个函数
        protected void delSoundItem(SoundItem item)
        {
            m_path2SoundDic.Remove(item.m_path);
            m_audioList.Remove(item);
        }

        // 定时释放资源
        public void onTimer(TimerItemBase time)
        {
            bool hasNoLoop = false;
            // 遍历看有没有播放完成的
            foreach(SoundItem sound in m_audioList)
            {
                if(sound.isEnd())
                {
                    m_clearList.Add(sound);
                }
                else if (!sound.m_bLoop)
                {
                    hasNoLoop = true;
                }
            }

            foreach(SoundItem sound in m_clearList)
            {
                unload(sound.m_path);
            }

            m_clearList.Clear();

            if (!hasNoLoop)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_timer);
            }
        }

        public void addTimer()
        {
            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
                m_timer.m_internal = 3;        // 一分钟遍历一次
                m_timer.m_bInfineLoop = true;
                m_timer.m_timerDisp = onTimer;
            }

            // 检查是否要加入定时器
            Ctx.m_instance.m_timerMgr.addObject(m_timer);
        }

        protected bool isPrefab(string path)
        {
            if (path.Substring(path.IndexOf(".") + 1) == "prefab")
            {
                return true;
            }

            return false;
        }

        // 卸载所有的资源
        public void unloadAll()
        {
            if (m_timer != null)
            {
                Ctx.m_instance.m_timerMgr.delObject(m_timer);
            }

            // 遍历看有没有播放完成的
            foreach (SoundItem sound in m_audioList)
            {
                sound.unload();
            }

            m_audioList.Clear();
            m_path2SoundDic.Clear();
        }
    }
}
using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public enum SpritePlayState
    {
        eNone,
        ePlaying,
        ePause,
        eStop,
    }

    /**
     * @brief 精灵动画，因为这个可以作为独立的渲染器存在是，因此继承 AuxComponent ，UI 直接使用这个渲染器就行了，不用使用具体的 Effect
     */
    public class SpriteAni : AuxComponent, IDispatchObject
    {
        protected float m_leftTime;     // 播放完成一帧后还剩余的时间
        protected int m_curFrame;       // 当前播放到第几帧
        protected bool m_bLoop;         // 是否是循环播放动画
        protected string m_goName;
        protected SpritePlayState m_playState;

        protected int m_tableID;            // 表中 id
        protected bool m_bNeedReloadRes;    // 是否需要重新加载资源
        protected TableSpriteAniItemBody m_tableBody;
        protected AtlasScriptRes m_atlasScriptRes;
        protected EventDispatch m_playEndEventDispatch;         // 特效播放完成事件分发
        protected bool m_bClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的
        protected bool m_bKeepLastFrame;        // 停止特效后，是否保留最后一帧的内容

        public SpriteAni()
        {
            m_leftTime = 0;
            m_curFrame = 0;
            m_bLoop = false;
            m_bNeedReloadRes = false;
            m_playState = SpritePlayState.eNone;
            m_playEndEventDispatch = new AddOnceAndCallOnceEventDispatch();
            m_bKeepLastFrame = false;
        }

        public bool bLoop
        {
            get
            {
                return m_bLoop;
            }
            set
            {
                m_bLoop = value;
            }
        }

        public string goName
        {
            set
            {
                m_goName = value;
            }
        }

        public int tableID
        {
            set
            {
                if (m_tableID != value)
                {
                    m_bNeedReloadRes = true;

                    m_tableID = value;
                    m_tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SPRITEANI, (uint)m_tableID).m_itemBody as TableSpriteAniItemBody;

                    onSpritePrefabChanged();
                }
            }
        }

        public SpritePlayState playState
        {
            get
            {
                return m_playState;
            }
            set
            {
                m_playState = value;
            }
        }

        public EventDispatch playEndEventDispatch
        {
            get
            {
                return m_playEndEventDispatch;
            }
            set
            {
                m_playEndEventDispatch = value;
            }
        }

        public bool bKeepLastFrame
        {
            get
            {
                return m_bKeepLastFrame;
            }
            set
            {
                m_bKeepLastFrame = value;
            }
        }

        virtual public void setClientDispose()
        {
            m_bClientDispose = true;
        }

        virtual public bool getClientDispose()
        {
            return m_bClientDispose;
        }

        // 特效对应的精灵 Prefab 改变
        virtual public void onSpritePrefabChanged()
        {

        }

        override public void dispose()
        {
            if (m_atlasScriptRes != null)
            {
                Ctx.m_instance.m_atlasMgr.unload(m_atlasScriptRes.GetPath(), null);
                m_atlasScriptRes = null;
            }
            m_playEndEventDispatch.clearEventHandle();
            base.dispose();
        }

        virtual public void play()
        {
            if(SpritePlayState.ePlaying != m_playState)
            {
                syncUpdateCom();
                m_playState = SpritePlayState.ePlaying;
            }
        }

        // 停止播放就是不显示了
        virtual public void stop()
        {
            if (SpritePlayState.eStop != m_playState)
            {
                m_playState = SpritePlayState.eStop;
                // 停止后，从 0 开始播放
                m_curFrame = 0;
                m_leftTime = 0;
            }
        }

        virtual public void pause()
        {
            if (SpritePlayState.eStop != m_playState)
            {
                m_playState = SpritePlayState.ePause;
            }
        }

        public bool bPlay()
        {
            return (SpritePlayState.ePlaying == m_playState);
        }

        // 自己发生改变
        override protected void onSelfChanged()
        {
            base.onSelfChanged();
            findWidget();
        }

        // 查找 UI 组件
        virtual public void findWidget()
        {
            
        }

        public void syncUpdateCom()
        {
            if (m_bNeedReloadRes)
            {
                if(m_atlasScriptRes != null)
                {
                    Ctx.m_instance.m_atlasMgr.unload(m_atlasScriptRes.GetPath(), null);
                    m_atlasScriptRes = null;
                }
                m_atlasScriptRes = Ctx.m_instance.m_atlasMgr.getAndSyncLoad<AtlasScriptRes>(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSpriteAni], m_tableBody.m_aniResName));
            }
            
            m_bNeedReloadRes = false;
        }

        public void onTick(float delta)
        {
            if (SpritePlayState.ePlaying == m_playState)
            {
                m_leftTime += delta;
                if (m_leftTime >= m_tableBody.m_invFrameRate)
                {
                    ++m_curFrame;

                    m_curFrame %= m_tableBody.m_frameCount;
                    m_leftTime -= m_tableBody.m_invFrameRate;

                    updateImage();

                    if (m_curFrame == m_tableBody.m_frameCount - 1)
                    {
                        if (!m_bLoop)
                        {
                            stop();
                            dispEndEvent();         // 只有被动停止才会发送播放结束事件，如果是主动停止的，不会发送播放结束事件
                        }
                    }
                }
            }
        }

        virtual protected void dispEndEvent()
        {
            m_playEndEventDispatch.dispatchEvent(this);
        }

        virtual public void updateImage()
        {
            
        }

        virtual public bool checkRender()
        {
            return false;
        }
    }
}
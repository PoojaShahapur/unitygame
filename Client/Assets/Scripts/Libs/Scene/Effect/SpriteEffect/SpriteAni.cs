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

    public enum SpriteComType
    {
        eSpriteRenderer,    // SpriteRenderer 组件
        eImage,     // Image UI 组件
    }

    /**
     * @brief 精灵动画
     */
    public class SpriteAni : AuxComponent, IDispatchObject, ISceneEntity
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
        protected EventDispatch m_endEventDispatch;

        public SpriteAni()
        {
            m_leftTime = 0;
            m_curFrame = 0;
            m_bLoop = false;
            m_bNeedReloadRes = false;
            m_playState = SpritePlayState.eNone;
            m_endEventDispatch = new AddOnceEventDispatch();
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
                }

                m_tableID = value;
                m_tableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SPRITEANI, (uint)m_tableID).m_itemBody as TableSpriteAniItemBody;
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

        public EventDispatch endEventDispatch
        {
            get
            {
                return m_endEventDispatch;
            }
            set
            {
                m_endEventDispatch = value;
            }
        }

        override public void dispose()
        {
            if (m_atlasScriptRes != null)
            {
                Ctx.m_instance.m_atlasMgr.unload(m_atlasScriptRes.GetPath(), null);
                m_atlasScriptRes = null;
            }
            m_endEventDispatch.clearEventHandle();
            Ctx.m_instance.m_spriteAniMgr.removeFromeList(this);
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
                        }
                        m_endEventDispatch.dispatchEvent(this);
                    }
                }
            }
        }

        virtual protected void updateImage()
        {
            
        }
    }
}
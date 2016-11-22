namespace SDK.Lib
{
    public enum SpritePlayState
    {
        eNone,
        ePlaying,
        ePause,
        eStop,
    }

    public enum eSpriteLoopType
    {
        eSequence,
        ePingPang,
    }

    public enum ePlayDirection
    {
        ePositive,  // 正方向
        eNegative,  // 反方向
    }

    /**
     * @brief 精灵动画，因为这个可以作为独立的渲染器存在是，因此继承 AuxComponent ，UI 直接使用这个渲染器就行了，不用使用具体的 Effect
     */
    public class SpriteAni : AuxComponent, IDispatchObject
    {
        protected float mLeftTime;     // 播放完成一帧后还剩余的时间
        protected int mCurFrame;       // 当前播放到第几帧
        protected bool mIsLoop;         // 是否是循环播放动画
        protected string mGoName;
        protected SpritePlayState mPlayState;

        protected int mTableID;            // 表中 id
        protected bool mIsNeedReloadRes;    // 是否需要重新加载资源
        protected TableSpriteAniItemBody mTableBody;
        protected AtlasScriptRes mAtlasScriptRes;
        protected EventDispatch mPlayEndEventDispatch;         // 特效播放完成事件分发
        protected bool mIsClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的
        protected bool mIsKeepLastFrame;        // 停止特效后，是否保留最后一帧的内容
        protected eSpriteLoopType mLoopType;
        protected ePlayDirection mCurPlayDirection;        // 如果是 PingPang 播放的时候当前播放方向

        public SpriteAni()
        {
            this.mLeftTime = 0;
            this.mCurFrame = 0;
            this.mIsLoop = false;
            this.mIsNeedReloadRes = false;
            this.mPlayState = SpritePlayState.eNone;
            this.mPlayEndEventDispatch = new AddOnceAndCallOnceEventDispatch();
            this.mIsKeepLastFrame = false;
            this.mLoopType = eSpriteLoopType.eSequence;
            this.mCurPlayDirection = ePlayDirection.ePositive;
        }

        public bool bLoop
        {
            get
            {
                return this.mIsLoop;
            }
            set
            {
                this.mIsLoop = value;
            }
        }

        public string goName
        {
            set
            {
                this.mGoName = value;
            }
        }

        public int tableID
        {
            set
            {
                //if (m_tableID == 11)
                //{
                //    Ctx.m_instance.m_logSys.log("asdfasdasdfas");
                //}
                if (this.mTableID != value)
                {
                    this.mIsNeedReloadRes = true;

                    this.mTableID = value;
                    this.mTableBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_SPRITEANI, (uint)this.mTableID).m_itemBody as TableSpriteAniItemBody;

                    onSpritePrefabChanged();
                }
            }
        }

        public SpritePlayState playState
        {
            get
            {
                return this.mPlayState;
            }
            set
            {
                this.mPlayState = value;
            }
        }

        public EventDispatch playEndEventDispatch
        {
            get
            {
                return this.mPlayEndEventDispatch;
            }
            set
            {
                this.mPlayEndEventDispatch = value;
            }
        }

        public bool bKeepLastFrame
        {
            get
            {
                return this.mIsKeepLastFrame;
            }
            set
            {
                this.mIsKeepLastFrame = value;
            }
        }

        virtual public void setKeepLastFrame(bool bKeep)
        {
            this.mIsKeepLastFrame = bKeep;
        }

        virtual public void setLoopType(eSpriteLoopType type)
        {
            this.mLoopType = type;
        }

        virtual public void setClientDispose()
        {
            this.mIsClientDispose = true;
        }

        virtual public bool getClientDispose()
        {
            return this.mIsClientDispose;
        }

        // 特效对应的精灵 Prefab 改变
        virtual public void onSpritePrefabChanged()
        {

        }

        override public void dispose()
        {
            if (mAtlasScriptRes != null)
            {
                Ctx.m_instance.m_atlasMgr.unload(this.mAtlasScriptRes.getResUniqueId(), null);
                this.mAtlasScriptRes = null;
            }
            this.mPlayEndEventDispatch.clearEventHandle();
            base.dispose();
        }

        virtual public void play()
        {
            if(SpritePlayState.ePlaying != this.mPlayState)
            {
                syncUpdateCom();
                this.mPlayState = SpritePlayState.ePlaying;
            }
        }

        // 停止播放就是不显示了
        virtual public void stop()
        {
            if (SpritePlayState.eStop != this.mPlayState)
            {
                this.mPlayState = SpritePlayState.eStop;
                // 停止后，从 0 开始播放
                this.mCurFrame = 0;
                this.mLeftTime = 0;
            }
        }

        virtual public void pause()
        {
            if (SpritePlayState.eStop != this.mPlayState)
            {
                this.mPlayState = SpritePlayState.ePause;
            }
        }

        public bool bPlay()
        {
            return (SpritePlayState.ePlaying == this.mPlayState);
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
            if (this.mIsNeedReloadRes)
            {
                if(this.mAtlasScriptRes != null)
                {
                    Ctx.m_instance.m_atlasMgr.unload(this.mAtlasScriptRes.getResUniqueId(), null);
                    this.mAtlasScriptRes = null;
                }
                this.mAtlasScriptRes = Ctx.m_instance.m_atlasMgr.getAndSyncLoad<AtlasScriptRes>(string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSpriteAni], this.mTableBody.m_aniResName));
            }

            this.mIsNeedReloadRes = false;
        }

        public void onTick(float delta)
        {
            if (SpritePlayState.ePlaying == this.mPlayState)
            {
                this.mLeftTime += delta;

                if (this.mLeftTime >= this.mTableBody.m_invFrameRate)
                {
                    if (eSpriteLoopType.eSequence == this.mLoopType)    // 顺序播放
                    {
                        ++this.mCurFrame;
                        this.mCurFrame %= this.mTableBody.m_frameCount;
                    }
                    else        // pingpang 播放
                    {
                        if(ePlayDirection.ePositive == this.mCurPlayDirection)
                        {
                            if (this.mCurFrame == mTableBody.m_frameCount - 1) // 如果上一帧是最后一帧
                            {
                                this.mCurPlayDirection = ePlayDirection.eNegative;
                                --this.mCurFrame;
                                if(this.mCurFrame < 0)  // 如果就一帧
                                {
                                    this.mCurFrame = 0;
                                    this.mCurPlayDirection = ePlayDirection.ePositive;
                                }
                            }
                            else
                            {
                                ++this.mCurFrame;
                            }
                        }
                        else    // 如果向反方向播放
                        {
                            if(this.mCurFrame == 0)     // 如果上一帧是第一帧
                            {
                                this.mCurPlayDirection = ePlayDirection.ePositive;
                                ++this.mCurFrame;
                                if(this.mCurFrame == this.mTableBody.m_frameCount)  // 如果总共就一帧
                                {
                                    this.mCurFrame = 0;
                                    this.mCurPlayDirection = ePlayDirection.eNegative;
                                }
                            }
                            else
                            {
                                --this.mCurFrame;
                            }
                        }
                    }

                    this.mLeftTime -= this.mTableBody.m_invFrameRate;

                    updateImage();

                    if (this.mCurFrame == this.mTableBody.m_frameCount - 1)
                    {
                        if (!this.mIsLoop)
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
            this.mPlayEndEventDispatch.dispatchEvent(this);
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
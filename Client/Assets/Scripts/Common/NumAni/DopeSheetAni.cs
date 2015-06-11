using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
     /**
      * @brief Unity 自己的 DopeSheet 动画
      */
    public class DopeSheetAni : NumAniBase
    {
        protected ControllerRes m_controlRes;
        protected bool m_bNeedReload;       // 需要重新加载资源
        protected string m_controlPath;
        protected AnimatorControl m_animatorControl;    // 动画控制器
        protected int m_stateId;        // 播放状态 Id

        public DopeSheetAni()
        {
            m_bNeedReload = false;
            m_controlPath = null;
            m_animatorControl = new AnimatorControl();
            m_animatorControl.oneAniPlayEndDisp.addEventHandle(onOneAniPlayEnd);
            m_stateId = 0;
        }

        // 释放资源
        override public void dispose()
        {
            if(m_controlRes != null)
            {
                Ctx.m_instance.m_controllerMgr.unload(m_controlRes.GetPath(), null);
                m_controlRes = null;
            }

            m_animatorControl.dispose();
            m_animatorControl = null;
        }

        public AnimatorControl animatorControl
        {
            get
            {
                return m_animatorControl;
            }
        }

        public int stateId
        {
            get
            {
                return m_stateId;
            }
            set
            {
                m_stateId = value;
            }
        }

        public void setControlInfo(string path)
        {
            if(m_controlPath != path)
            {
                m_controlPath = path;
                m_bNeedReload = true;
            }
        }

        override public void setGO(GameObject go)
        {
            base.setGO(go);
        }

        // 同步更新控制器
        public void syncUpdateControl()
        {
             if(m_bNeedReload)
             {
                 if(m_controlRes != null)
                 {
                     Ctx.m_instance.m_controllerMgr.unload(m_controlRes.GetPath(), null);
                     m_controlRes = null;

                     if(m_animatorControl.animator != null)
                     {
                         UtilApi.Destroy(m_animatorControl.animator.runtimeAnimatorController);
                     }
                 }

                 m_controlRes = Ctx.m_instance.m_controllerMgr.getAndSyncLoad<ControllerRes>(m_controlPath);

                 if(m_animatorControl.animator == null)
                 {
                     m_animatorControl.animator = m_go.AddComponent<Animator>();
                 }

                 m_animatorControl.animator.runtimeAnimatorController = m_controlRes.InstantiateController();
             }

             m_bNeedReload = false;
        }

        // 一个动画播放结束
        public void onOneAniPlayEnd(IDispatchObject dispObj)
        {
            if(bAniEndDispNotNull())
            {
                m_aniEndDisp(this);
            }

            stop();
        }

        override public void play()
        {
            base.play();
            m_animatorControl.SetInteger(0, stateId);
        }

        override public void stop()
        {
            base.stop();
            m_animatorControl.SetInteger(0, 0);
        }

        override public void pause()
        {
            base.pause();
        }
    }
}
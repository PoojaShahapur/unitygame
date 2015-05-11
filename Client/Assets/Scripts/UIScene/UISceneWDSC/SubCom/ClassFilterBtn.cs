using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.UI;
using SDK.Lib;

namespace Game.UI
{
    /// <summary>
    /// 职业过滤面板
    /// </summary>
    public class ClassFilterBtn : InterActiveEntity
    {
        protected SceneWDSCData m_sceneWDSCData;
        protected bool m_isUp = false;          // 当前是否在 Up 状态
        protected EnPlayerCareer m_myClass;     // 当前表示的职业
        protected Vector3 m_lastPostion = new Vector3();    // 保存上一次的位置

        public SceneWDSCData sceneWDSCData
        {
            get
            {
                return m_sceneWDSCData;
            }
            set
            {
                m_sceneWDSCData = value;
            }
        }

        public EnPlayerCareer myClass
        {
            get
            {
                return m_myClass;
            }
            set
            {
                m_myClass = value;
            }
        }

        // Use this for initialization
        public override void Start()
        {
            UtilApi.addEventHandle(gameObject, onBtnClk);
        }

        // 按钮起的动画
        public void btnUpAni()
        {
            if (m_sceneWDSCData.m_tabBtnIdx == m_tag && !m_isUp)
            {
                //up
                iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3.forward * 0.1f,
                                                       "space", Space.World,
                                                       "time", 0.1f));
                //变大
                iTween.ScaleBy(gameObject, Vector3.one * 1.2f, 0.1f);
                m_isUp = true;
            }
        }

        // 按钮按下的动画
        public void btnDownAni()
        {
            if (m_sceneWDSCData.m_tabBtnIdx != m_tag && m_isUp)
            {
                //down
                iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3.forward * -0.1f,
                                                       "space", Space.World,
                                                       "time", 0.1f));
                //变 小
                iTween.ScaleTo(gameObject, Vector3.one * 0.0254f, 0.1f);
                m_isUp = false;
            }
        }

        protected void onBtnClk(GameObject go)
        {
            OnMouseUpAsButton();
        }

        protected void OnMouseUpAsButton()
        {
            if (!m_isUp)
            {
                if (m_myClass < EnPlayerCareer.ePCTotal)
                {
                    UISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI<UISceneWDSC>(UISceneFormID.eUISceneWDSC);
                    uiSC.onclass(m_myClass);
                }
            }
        }

        protected void classDown(string sendname)
        {
            if (sendname == name)
            {
                return;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.05055332f);
            m_isUp = false;
        }

        public void classFilterHide(EnPlayerCareer c)
        {
            m_lastPostion = transform.localPosition;

            if (m_myClass == EnPlayerCareer.HERO_OCCUPATION_NONE)     // 这个是中性职业，必然显示
            {
                gotoTwo();
                return;
            }
            if (m_myClass != c)
            {
                cHide();
            }
            else
            {
                gotoOne();
            }
        }

        protected void cHide()
        {
            transform.Translate(Vector3.forward * 10, Space.World);
        }

        protected void gotoTwo()
        {
            transform.localPosition = new Vector3(-1.594784f, transform.localPosition.y, transform.localPosition.z);
        }

        protected void gotoOne()
        {
            transform.localPosition = new Vector3(-2.006191f, transform.localPosition.y, transform.localPosition.z);
            OnMouseUpAsButton();
        }

        public void gotoBack()
        {
            transform.localPosition = new Vector3(m_lastPostion.x, 0.07995506f, 0.05055332f);

            if (m_sceneWDSCData.m_tabBtnIdx == m_tag)
            {
                m_isUp = false;
                OnMouseUpAsButton();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 加入一个新的内容，其它的内容都向下滑动
     */
    public class SlideList : AuxParentComponent
    {
        protected GameObject m_localGo;   // 局部的 GameObject

        protected List<SlideListItem> m_waitlist;       // 将要加入列表中的内容
        protected SlideListItem m_addingItem;           // 正在加入的
        protected List<SlideListItem> m_addedList;           // 已经加入的
        protected float m_XOff = -0.3f;       // 开始的时候 X 偏移
        protected NumAniSequence m_numAniSequence = new NumAniSequence();       // 数字动画
        protected PosAni m_posAni = new PosAni();                               // 位置动画

        protected NumAniSequence m_downNumAniSequence = new NumAniSequence();       // 数字动画
        protected PosAni m_downPosAni = new PosAni();                               // 位置动画
        protected float m_ZOff = -0.3f;

        public SlideList(GameObject pnt)
        {
            m_waitlist = new List<SlideListItem>();
            m_addedList = new List<SlideListItem>();
            setPntGo(pnt);
            m_localGo = new GameObject();
            m_localGo.name = "m_localGo";
            m_localGo.transform.SetParent(pntGo.transform, false);

            m_numAniSequence.setAniSeqEndDisp(onAddingEnd);
            m_downNumAniSequence.setAniSeqEndDisp(onDownEnd);
        }

        // 加入一个 item
        public void addItem(SlideListItem item)
        {
            if(m_addingItem != null)        // 如果有正在加入的
            {
                m_waitlist.Add(item);
            }
            else
            {
                m_addingItem = item;
                startDownAni();
                startAddingAni();
            }
        }

        // 开始加入动画
        protected void startAddingAni()
        {
            m_addingItem.setPntGo(pntGo);
            m_addingItem.selfLocalGo.transform.localPosition = new Vector3(m_XOff, 0, 0);
            m_posAni.setGO(m_addingItem.selfLocalGo);
            m_posAni.destPos = new Vector3(0, 0, 0);
            m_posAni.setEaseType(iTween.EaseType.easeInExpo);
            m_posAni.setTime(0.5f);
            m_numAniSequence.addOneNumAni(m_posAni);
            m_numAniSequence.play();

            m_addingItem.loadRes();         // 加载资源
        }

        // 开始向下移动动画
        protected void startDownAni()
        {
            if (m_addedList.Count > 0)
            {
                m_localGo.transform.localPosition = new Vector3(0, 0, 0);
                m_downPosAni.setGO(m_localGo);
                m_downPosAni.destPos = new Vector3(0, 0, m_ZOff);
                m_downPosAni.setEaseType(iTween.EaseType.easeInExpo);
                m_downPosAni.setTime(0.2f);
                m_downNumAniSequence.addOneNumAni(m_downPosAni);
                m_downNumAniSequence.play();
            }
        }

        protected void onAddingEnd(NumAniSeqBase ani)
        {
            m_addedList.Add(m_addingItem);
            adjustPos();

            if(m_waitlist.Count > 0)
            {
                nextAddingItem();
            }
            else
            {
                m_addingItem = null;
            }
        }

        protected void onDownEnd(NumAniSeqBase ani)
        {

        }

        // 调整位置
        protected void adjustPos()
        {
            m_localGo.transform.localPosition = new Vector3(0, 0, 0);
            float curheight = 0;
            int idx = m_addedList.Count - 1;
            m_addedList[m_addedList.Count - 1].setPntGo(m_localGo);
            for (; idx >= 0; --idx)
            {
                m_addedList[idx].selfLocalGo.transform.localPosition = new Vector3(0, 0, curheight);
                curheight -= m_addedList[idx].height;
            }
        }

        protected void nextAddingItem()
        {
            m_addingItem = m_waitlist[0];
            m_waitlist.RemoveAt(0);
            startDownAni();
            startAddingAni();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 加入一个新的内容，其它的内容都向下滑动
     */
    public class SlideList : AuxComponent
    {
        protected GameObject mLocalGo;   // 局部的 GameObject

        protected List<SlideListItem> mWaitlist;        // 将要加入列表中的内容
        protected SlideListItem mAddingItem;            // 正在加入的
        protected List<SlideListItem> mAddedList;       // 已经加入的
        protected float mXOff;                          // 开始的时候 X 偏移
        protected NumAniSequence mNumAniSequence;       // 数字动画
        protected PosAni mPosAni;                       // 位置动画

        protected NumAniSequence mDownNumAniSequence;   // 数字动画
        protected PosAni mDownPosAni;                   // 位置动画
        protected float mZOff;

        public SlideList(GameObject pnt)
        {
            this.mNumAniSequence = new NumAniSequence();
            this.mDownNumAniSequence = new NumAniSequence();
            this.mDownPosAni = new PosAni();

            this.mXOff = -0.3f;

            this.mPosAni = new PosAni();
            this.mDownPosAni = new PosAni();

            this.mZOff = -0.3f;

            this.mWaitlist = new List<SlideListItem>();
            this.mAddedList = new List<SlideListItem>();
            setPntGo(pnt);
            this.mLocalGo = UtilApi.createGameObject("SlideList");
            this.mLocalGo.name = "m_localGo";
            this.mLocalGo.transform.SetParent(pntGo.transform, false);

            this.mNumAniSequence.setAniSeqEndDisp(onAddingEnd);
            this.mDownNumAniSequence.setAniSeqEndDisp(onDownEnd);
        }

        // 加入一个 item
        public void addItem(SlideListItem item)
        {
            if(this.mAddingItem != null)        // 如果有正在加入的
            {
                this.mWaitlist.Add(item);
            }
            else
            {
                this.mAddingItem = item;
                startDownAni();
                startAddingAni();
            }
        }

        // 开始加入动画
        protected void startAddingAni()
        {
            this.mAddingItem.setPntGo(pntGo);
            UtilApi.setPos(this.mAddingItem.selfLocalGo.transform, new Vector3(this.mXOff, 0, 0));
            this.mPosAni.setGO(this.mAddingItem.selfLocalGo);
            this.mPosAni.destPos = new Vector3(0, 0, 0);
            this.mPosAni.setEaseType(iTween.EaseType.easeInExpo);
            this.mPosAni.setTime(0.5f);
            this.mNumAniSequence.addOneNumAni(this.mPosAni);
            this.mNumAniSequence.play();

            this.mAddingItem.loadRes();         // 加载资源
        }

        // 开始向下移动动画
        protected void startDownAni()
        {
            if (this.mAddedList.Count > 0)
            {
                UtilApi.setPos(this.mLocalGo.transform, new Vector3(0, 0, 0));
                this.mDownPosAni.setGO(this.mLocalGo);
                this.mDownPosAni.destPos = new Vector3(0, 0, this.mZOff);
                this.mDownPosAni.setEaseType(iTween.EaseType.easeInExpo);
                this.mDownPosAni.setTime(0.2f);
                this.mDownNumAniSequence.addOneNumAni(this.mDownPosAni);
                this.mDownNumAniSequence.play();
            }
        }

        protected void onAddingEnd(NumAniSeqBase ani)
        {
            this.mAddedList.Add(this.mAddingItem);
            adjustPos();

            if(this.mWaitlist.Count > 0)
            {
                nextAddingItem();
            }
            else
            {
                this.mAddingItem = null;
            }
        }

        protected void onDownEnd(NumAniSeqBase ani)
        {

        }

        // 调整位置
        protected void adjustPos()
        {
            UtilApi.setPos(this.mLocalGo.transform, new Vector3(0, 0, 0));
            float curheight = 0;
            int idx = this.mAddedList.Count - 1;
            this.mAddedList[this.mAddedList.Count - 1].setPntGo(this.mLocalGo);
            for (; idx >= 0; --idx)
            {
                UtilApi.setPos(this.mAddedList[idx].selfLocalGo.transform, new Vector3(0, 0, curheight));
                curheight -= this.mAddedList[idx].height;
            }
        }

        protected void nextAddingItem()
        {
            this.mAddingItem = this.mWaitlist[0];
            this.mWaitlist.RemoveAt(0);
            startDownAni();
            startAddingAni();
        }
    }
}
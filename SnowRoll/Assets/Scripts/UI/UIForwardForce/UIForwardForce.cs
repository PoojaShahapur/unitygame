using System;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 摇杆
     */
    public class UIForwardForce : Form, ITickedObject, IDelayHandleItem
    {
        private RectTransform ForceBtn;

        private bool isTouchBegin = false;
        private bool isTouchHold = false;
        private bool isTouchEnd = false;
        private Vector2 CurMousePos; //鼠标初次点击位置

        private int HalfWidth;
        private int HalfBGWidth;
        private int Height;

        private Vector2 LPos;
        private Vector2 LOldPos;
        private Vector2 RPos;
        private Vector2 ROldPos;
        private Vector2 MoveVec;

        private JoyStickTouchInfo mTouchInfo;

        public UIForwardForce()
        {
            this.mTouchInfo = new JoyStickTouchInfo();
        }

        public override void onInit()
        {
            base.onInit();

            HalfWidth = Screen.width / 2;
            Height = Screen.height;

            //Ctx.mInstance.mInputMgr.addMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEDOWN_EVENT, onTouchBegin);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHBEGIN_EVENT, onTouchBegin);

            //Ctx.mInstance.mInputMgr.addMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEPRESS_EVENT, onTouchHold);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHSTATIONARY_EVENT, onTouchHold);

            //Ctx.mInstance.mInputMgr.addMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEUP_EVENT, onTouchEnd);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHENDED_EVENT, onTouchEnd);

            Ctx.mInstance.mTickMgr.addTick(this as ITickedObject, TickPriority.eTPForwardForce);
        }

        override public void onShow()
        {
            base.onShow();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();

            ForceBtn = UtilApi.TransFindChildByPObjAndPath(this.mGuiWin.mUiRoot, "Force_BtnTouch").GetComponent<RectTransform>();
            
            HalfBGWidth = (int)ForceBtn.sizeDelta.x / 2;
            ForceBtn.gameObject.SetActive(false);
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
        {
            base.onHide();
        }

        protected void findWidget()
        {

        }

        protected void addEventHandle()
        {

        }

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();

            //Ctx.mInstance.mInputMgr.removeMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEDOWN_EVENT, onTouchBegin);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHBEGIN_EVENT, onTouchBegin);

            //Ctx.mInstance.mInputMgr.removeMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEPRESS_EVENT, onTouchHold);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHSTATIONARY_EVENT, onTouchHold);

            //Ctx.mInstance.mInputMgr.removeMouseListener(MMouseDevice.MouseLeftButton, EventId.MOUSEUP_EVENT, onTouchEnd);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHENDED_EVENT, onTouchEnd);

            Ctx.mInstance.mTickMgr.removeTick(this as ITickedObject);
        }

        void ITickedObject.onTick(float delta)
        {
            OneTouchUpdate();
        }

        private void OneTouchUpdate()
        {
            if (Ctx.mInstance.mPlayerMgr != null && Ctx.mInstance.mPlayerMgr.getHero() != null)
            {
                if (isTouchBegin)
                {
                    SetOldPos(CurMousePos);
                }

                MoveVec.x = Input.acceleration.x;
                if (isTouchHold)
                {
                    SetPos(CurMousePos);
                }
                else
                {
                    MoveVec.y = Input.acceleration.y;
                }
                Ctx.mInstance.mPlayerMgr.setMoveVec(MoveVec);

                if (MoveVec.x == 0 && MoveVec.y == 0)
                {
                    Ctx.mInstance.mPlayerMgr.getHero().stopMove();
                }
                else
                {
                    Move(Ctx.mInstance.mPlayerMgr.getMoveVec());
                }
            }
        }

        public void onTouchBegin(IDispatchObject dispObj)
        {
            MMouseOrTouch touch = dispObj as MMouseOrTouch;

            if (this.mTouchInfo.onTouchBegin(touch))
            {
                if (touch.mPos.x < Screen.width / 2)
                {
                    CurMousePos = touch.mPos;
                    isTouchBegin = true;
                    isTouchEnd = false;
                    isTouchHold = false;
                }
                else
                {
                    isTouchBegin = false;
                    isTouchEnd = true;
                    isTouchHold = false;
                    if (ForceBtn != null) ForceBtn.gameObject.SetActive(false);
                }
            }
        }

        public void onTouchHold(IDispatchObject dispObj)
        {
            MMouseOrTouch touch = dispObj as MMouseOrTouch;

            if (mTouchInfo.onTouchMove(touch))
            {
                if (touch.mPos.x < Screen.width / 2)
                {
                    CurMousePos = touch.mPos;
                    isTouchBegin = false;
                    isTouchEnd = false;
                    isTouchHold = true;
                }
                else
                {
                    isTouchBegin = false;
                    isTouchEnd = true;
                    isTouchHold = false;
                    if (ForceBtn != null) ForceBtn.gameObject.SetActive(false);
                }
            }
        }

        public void onTouchEnd(IDispatchObject dispObj)
        {
            MMouseOrTouch touch = dispObj as MMouseOrTouch;

            if (this.mTouchInfo.onTouchEnd(touch))
            {
                isTouchBegin = false;
                isTouchEnd = true;
                isTouchHold = false;

                if (ForceBtn != null) ForceBtn.gameObject.SetActive(false);
            }
        }

        private void SetOldPos(Vector2 Pos)
        {
            if (Pos.x > HalfWidth)
            {
                ROldPos = Pos;
            }
            else
            {
                //防止按钮出了屏幕
                if (Pos.x < HalfBGWidth) Pos.x = HalfBGWidth;
                if (Pos.x > HalfWidth - HalfBGWidth) Pos.x = HalfWidth - HalfBGWidth;
                if (Pos.y < HalfBGWidth) Pos.y = HalfBGWidth;
                if (Pos.y > Height - HalfBGWidth) Pos.y = Height - HalfBGWidth;

                LOldPos = Pos;
                Vector3 JPos = Ctx.mInstance.mCamSys.mUguiCam.ScreenToWorldPoint(new Vector3(Pos.x, Pos.y, 100f));
                ForceBtn.gameObject.SetActive(true);
                ForceBtn.position = new Vector3(JPos.x, JPos.y, ForceBtn.position.z);
            }
        }
        private void SetPos(Vector2 Pos)
        {
            if (Pos.x > HalfWidth)
            {
                RPos = Pos;
            }
            else
            {
                MoveVec.y = 1;
            }
        }

        private void Move(Vector2 MoveVec)
        {
            if (Ctx.mInstance.mPlayerMgr != null && Ctx.mInstance.mPlayerMgr.getHero() != null)
            {
                if (Vector2.Equals(MoveVec, Vector2.zero))
                {
                    Ctx.mInstance.mPlayerMgr.getHero().stopMove();
                }
                else
                {
                    if (!Ctx.mInstance.mPlayerMgr.getHero().getCanMove())
                    {
                        Ctx.mInstance.mLuaSystem.exitForm(10003);//关闭重生界面
                    }

                    Ctx.mInstance.mPlayerMgr.getHero().moveForwardByOrient(MoveVec);
                }
            }
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }
    }
}

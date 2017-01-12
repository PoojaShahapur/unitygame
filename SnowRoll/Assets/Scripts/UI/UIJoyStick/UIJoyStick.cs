using System;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 摇杆
     */
    public class UIJoyStick : Form, ITickedObject, IDelayHandleItem
    {
        private RectTransform Joystick;
        private RectTransform BackGrounds;
        private bool StillTouch = false;
        private bool isStop = true;

        private bool isTouchBegin = false;
        private bool isTouchMove = false;
        private bool isTouchEnd = false;
        private Vector2 CurMousePos; //鼠标初次点击位置

        private int HalfWidth;
        private int HalfBGWidth;
        private int HalfJoyWidth;
        private int Height;

        private Vector2 LPos;
        private Vector2 LOldPos;
        private Vector2 RPos;
        private Vector2 ROldPos;
        private Vector2 MoveVec; //移动方向
        private Vector2 OldMousePos; //鼠标初次点击位置

        public UIJoyStick()
        {
        }

        public override void onInit()
        {
            base.onInit();

            HalfWidth = Screen.width / 2;
            Height = Screen.height;

            Ctx.mInstance.mInputMgr.addMouseListener(MMouse.MouseLeftButton, EventId.MOUSEDOWN_EVENT, onTouchBegin);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHBEGIN_EVENT, onTouchBegin);

            Ctx.mInstance.mInputMgr.addMouseListener(MMouse.MouseLeftButton, EventId.MOUSEPRESS_MOVE_EVENT, onTouchMove);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHMOVED_EVENT, onTouchMove);

            Ctx.mInstance.mInputMgr.addMouseListener(MMouse.MouseLeftButton, EventId.MOUSEUP_EVENT, onTouchEnd);
            Ctx.mInstance.mInputMgr.addTouchListener(EventId.TOUCHENDED_EVENT, onTouchEnd);

            Ctx.mInstance.mTickMgr.addTick(this as ITickedObject, TickPriority.eTPJoyStick);
        }

        override public void onShow()
        {
            base.onShow();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();

            GameObject _BackGroundsImage = UtilApi.TransFindChildByPObjAndPath(this.mGuiWin.mUiRoot, "Background");
            BackGrounds = _BackGroundsImage.GetComponent<RectTransform>();
            Joystick = UtilApi.TransFindChildByPObjAndPath(_BackGroundsImage, "JoyStick").GetComponent<RectTransform>();
            HalfBGWidth = (int)BackGrounds.sizeDelta.x / 2;
            HalfJoyWidth = (int)Joystick.sizeDelta.x / 2;
            Joystick.gameObject.SetActive(false);
            _BackGroundsImage.SetActive(false);
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

            Ctx.mInstance.mInputMgr.removeMouseListener(MMouse.MouseLeftButton, EventId.MOUSEDOWN_EVENT, onTouchBegin);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHBEGIN_EVENT, onTouchBegin);

            Ctx.mInstance.mInputMgr.removeMouseListener(MMouse.MouseLeftButton, EventId.MOUSEPRESS_MOVE_EVENT, onTouchMove);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHMOVED_EVENT, onTouchMove);

            Ctx.mInstance.mInputMgr.removeMouseListener(MMouse.MouseLeftButton, EventId.MOUSEUP_EVENT, onTouchEnd);
            Ctx.mInstance.mInputMgr.removeTouchListener(EventId.TOUCHENDED_EVENT, onTouchEnd);

            Ctx.mInstance.mTickMgr.delTick(this as ITickedObject);
        }

        void ITickedObject.onTick(float delta)
        {
            OneTouchUpdate();
        }

        private void OneTouchUpdate()
        {
            if (isTouchBegin)
            {
                SetOldPos(CurMousePos);
            }

            if (isTouchMove && StillTouch)
            {
                SetPos(CurMousePos);
            }

            //拖动摇杆放开后按最后的方向继续移动，直到点击停止            
            if (!isStop && !isTouchMove)
            {
                Move(MoveVec);
            }
        }

        public void onTouchBegin(IDispatchObject dispObj)
        {
            MMouseOrTouch touch = dispObj as MMouseOrTouch;
            
            if (touch.mPos.x < Screen.width / 2)
            {
                isStop = true;
                CurMousePos = touch.mPos;
                isTouchBegin = true;
                isTouchEnd = false;
                isTouchMove = false;
            }
            else
            {
                StillTouch = false;
                if (Joystick != null) Joystick.gameObject.SetActive(false);
                if (BackGrounds != null) BackGrounds.gameObject.SetActive(false);
            }
        }

        public void onTouchMove(IDispatchObject dispObj)
        {
            MMouseOrTouch touch = dispObj as MMouseOrTouch;
            if (touch.mPos.x < Screen.width / 2)
            {
                CurMousePos = touch.mPos;
                isTouchBegin = false;
                isTouchEnd = false;
                isTouchMove = true;
            }
            else
            {
                StillTouch = false;
                if (Joystick != null) Joystick.gameObject.SetActive(false);
                if (BackGrounds != null) BackGrounds.gameObject.SetActive(false);
            }
        }

        public void onTouchEnd(IDispatchObject dispObj)
        {
            isTouchBegin = false;
            isTouchEnd = true;
            isTouchMove = false;

            StillTouch = false;
            if (Joystick != null) Joystick.gameObject.SetActive(false);
            if (BackGrounds != null)  BackGrounds.gameObject.SetActive(false);
        }

        private void SetOldPos(Vector2 Pos)
        {
            if (OldMousePos.Equals(Pos)) return;
            
            OldMousePos = Pos;
            if (Pos.x > HalfWidth)
            {
                ROldPos = Pos;
            }
            else
            {
                Ctx.mInstance.mPlayerMgr.getHero().stopMove();

                //防止摇杆出了屏幕
                if (Pos.x < HalfBGWidth + HalfJoyWidth) Pos.x = HalfBGWidth + HalfJoyWidth;
                if (Pos.x > HalfWidth - HalfBGWidth - HalfJoyWidth) Pos.x = HalfWidth - HalfBGWidth - HalfJoyWidth;
                if (Pos.y < HalfBGWidth + HalfJoyWidth) Pos.y = HalfBGWidth + HalfJoyWidth;
                if (Pos.y > Height - HalfBGWidth - HalfJoyWidth) Pos.y = Height - HalfBGWidth - HalfJoyWidth;

                LOldPos = Pos;
                Vector3 JPos = Ctx.mInstance.mCamSys.mUguiCam.ScreenToWorldPoint(new Vector3(Pos.x, Pos.y, 100f));
                Joystick.gameObject.SetActive(true);
                BackGrounds.gameObject.SetActive(true);
                StillTouch = true;
                BackGrounds.position = new Vector3(JPos.x, JPos.y, BackGrounds.position.z);
                Joystick.position = new Vector3(JPos.x, JPos.y, Joystick.position.z);
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
                if (OldMousePos.Equals(Pos))//初始点击，还未滑动JoyStick
                {
                    isStop = true;
                    StillTouch = true;
                    Joystick.position = new Vector3(BackGrounds.position.x, BackGrounds.position.y, Joystick.position.z);
                }
                else
                {
                    //防止摇杆出了屏幕
                    if (Pos.x < HalfJoyWidth) Pos.x = HalfJoyWidth;
                    if (Pos.x > HalfWidth - HalfJoyWidth) Pos.x = HalfWidth - HalfJoyWidth;
                    if (Pos.y < HalfJoyWidth) Pos.y = HalfJoyWidth;
                    if (Pos.y > Height - HalfJoyWidth) Pos.y = Height - HalfJoyWidth;

                    LPos = Pos;
                    StillTouch = true;
                    Vector3 JPos = Ctx.mInstance.mCamSys.mUguiCam.ScreenToWorldPoint(new Vector3(Pos.x, Pos.y, 100f));

                    if (Mathf.Pow(JPos.x - BackGrounds.position.x, 2) + Mathf.Pow(JPos.y - BackGrounds.position.y, 2) > 0.1f)
                    {
                        isStop = false;
                        MoveVec = new Vector2(JPos.x - BackGrounds.position.x, JPos.y - BackGrounds.position.y).normalized;
                        //移动事件分发
                        Move(MoveVec);

                        float _joystick_x = JPos.x;
                        float _joystick_y = JPos.y;
                        float _radius = 1.2f;
                        if (Mathf.Pow(JPos.x - BackGrounds.position.x, 2) + Mathf.Pow(JPos.y - BackGrounds.position.y, 2) > Mathf.Pow(_radius, 2))
                        {
                            if (JPos.x > BackGrounds.position.x)
                            {
                                _joystick_x = BackGrounds.position.x + _radius * MoveVec.x;
                            }
                            if (JPos.x < BackGrounds.position.x)
                            {
                                _joystick_x = BackGrounds.position.x + _radius * MoveVec.x;
                            }
                            if (JPos.y > BackGrounds.position.y)
                            {
                                _joystick_y = BackGrounds.position.y + _radius * MoveVec.y;
                            }
                            if (JPos.y < BackGrounds.position.y)
                            {
                                _joystick_y = BackGrounds.position.y + _radius * MoveVec.y;
                            }
                        }
                        //Debug.Log("MoveVec: " + MoveVec + "   JPos: " + JPos.y + "   BackGrounds.position: " + BackGrounds.position.y);
                        Joystick.position = new Vector3(_joystick_x, _joystick_y, Joystick.position.z);
                    }
                    else
                    {
                        Joystick.position = new Vector3(JPos.x, JPos.y, Joystick.position.z);
                        Move(MoveVec);
                    }
                }
            }
        }

        private void Move(Vector2 MoveVec)
        {
            if (!Ctx.mInstance.mPlayerMgr.getHero().getCanMove())
            {
                Ctx.mInstance.mLuaSystem.exitForm(10003);//关闭重生界面
            }

            Ctx.mInstance.mPlayerMgr.getHero().moveForwardByOrient(MoveVec);
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

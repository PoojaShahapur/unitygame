using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 卡牌组删除按钮，就是在每一个卡组的右上角的叉号
     */
    public class CardSetDelBtn : InterActiveEntity
    {
        protected bool m_bInDel = false;

        public override void Start()
        {
            UtilApi.addHoverHandle(gameObject, OnMouseHover);
        }

        public void OnMouseHover(GameObject go, bool state)
        {
            if (true == state)
            {
                OnMouseEnter();
            }
            else
            {
                OnMouseExit();
            }
        }

        protected void OnMouseEnter()
        {
            Debug.Log("enter");
            m_bInDel = true;
        }

        //放弃显示控制
        protected void OnMouseExit()
        {
            m_bInDel = false;
        }

        public IEnumerator hide()
        {
            Debug.Log("hide");
            yield return new WaitForSeconds(0.1f);
            if (!m_bInDel)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
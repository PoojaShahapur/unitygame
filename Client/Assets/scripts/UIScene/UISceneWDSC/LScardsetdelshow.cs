using UnityEngine;
using System.Collections;
using SDK.Lib;
using SDK.Common;

namespace Game.UI
{
    public class cardsetdelshow : InterActiveEntity
    {
        bool isindel = false;

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

        void OnMouseEnter()
        {
            Debug.Log("enter");
            isindel = true;
        }

        //放弃显示控制
        void OnMouseExit()
        {
            isindel = false;
        }

        public IEnumerator hide()
        {
            Debug.Log("hide");
            yield return new WaitForSeconds(0.1f);
            if (!isindel)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
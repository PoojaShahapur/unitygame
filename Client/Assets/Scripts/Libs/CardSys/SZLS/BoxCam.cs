using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 主控制
    /// </summary>
    public class BoxCam : InterActiveEntity
    {
        // Use this for initialization
        public override void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void push()
        {
            //animation["boxcampush"].speed = 1;
            //animation.Play("boxcampush");
        }

        public void back()
        {
            //animation["boxcampush"].speed = -1;
            //animation["boxcampush"].time = animation["boxcampush"].length;
            //animation.Play("boxcampush");
        }
    }
}
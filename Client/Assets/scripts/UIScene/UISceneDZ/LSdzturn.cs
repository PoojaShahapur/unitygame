using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 点击结束当前一局
     */
    public class dzturn : InterActiveEntity
    {
        bool ismyturn = false;
        // Use this for initialization
        public override void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnMouseUpAsButton()
        {
            if (dzcam.ismyturn)
            {
                animation["dzturn"].speed = 1;
                dzcam.ismyturn = false;
                animation.Play("dzturn");
                //endturn
                //Camera.main.SendMessage("endturn");
                Ctx.m_instance.m_camSys.m_dzcam.endturn();
            }
        }

        void myturn()
        {
            animation["dzturn"].speed = -1;
            animation["dzturn"].time = 1;
            animation.Play("dzturn");
            dzcam.ismyturn = true;
        }
    }
}
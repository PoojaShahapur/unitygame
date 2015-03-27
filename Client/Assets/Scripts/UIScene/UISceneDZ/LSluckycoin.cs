using UnityEngine;
using System.Collections;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 对战场景中的幸运币
     */
    public class luckycoin : InterActiveEntity
    {
        // Use this for initialization
        public override void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void first()
        {

            transform.Rotate(Vector3.left, 20);
            gameObject.AddComponent<Rigidbody>();
        }

        void cost()
        {
            transform.Rotate(Vector3.left, -20);
            gameObject.AddComponent<Rigidbody>();
        }
    }
}
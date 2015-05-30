using UnityEngine;
using System.Collections;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 对战场景中的幸运币
     */
    public class LuckCoinCard : SceneComponent
    {
        // Use this for initialization
        public override void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {

        }

        public void first()
        {
            transform.Rotate(Vector3.left, 20);
            gameObject.AddComponent<Rigidbody>();
        }

        public void cost()
        {
            transform.Rotate(Vector3.left, -20);
            gameObject.AddComponent<Rigidbody>();
        }
    }
}
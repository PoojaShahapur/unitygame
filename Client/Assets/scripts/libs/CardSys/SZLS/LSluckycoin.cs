using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class luckycoin : InterActiveEntity
    {
        // Use this for initialization
        void Start()
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
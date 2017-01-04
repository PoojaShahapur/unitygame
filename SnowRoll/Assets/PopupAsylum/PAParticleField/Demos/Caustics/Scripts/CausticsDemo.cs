//
// CausticsDemo
// Mark Hogan
// @markeahogan
// www.popupasylum.co.uk
//

using UnityEngine;
using System.Collections;

namespace PA.ParticleField.Samples {

    /// <summary>
    /// Updates a transforms position so that it stays in front of the target at a specified distance and world y position
    /// </summary>
    [ExecuteInEditMode]
    public class CausticsDemo : MonoBehaviour {
        //public Transform target;
        public float cameraDistance = 50f;
        public float waterSurfaceHeight = 0f;

        void Update() {
            UpdatePosition();
        }

        void UpdatePosition() {
            //Reset the caustics rotation
            transform.rotation = Quaternion.identity;
            //Position the caustics in front of the camera
            //transform.position = target.position + target.forward * cameraDistance;
            //cache its world position
            Vector3 pos = transform.position;
            //set its Y height to the waters surface
            pos.y = waterSurfaceHeight;
            //apply the modified position
            transform.position = pos;
        }
    }
}
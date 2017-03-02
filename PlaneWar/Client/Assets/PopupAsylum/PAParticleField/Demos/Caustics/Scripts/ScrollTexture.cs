//
// Scroll texture
// Mark Hogan
// @markeahogan
// www.popupasylum.co.uk
//

using UnityEngine;
using System.Collections;

namespace PA.ParticleField.Samples {

    /// <summary>
    /// Scrolls a texture over time
    /// </summary>
    public class ScrollTexture : MonoBehaviour {

        public string textureName;
        public Vector2 speed;
        Vector2 offset;

        // Update is called once per frame
        void FixedUpdate() {

            offset += speed;

            if (GetComponent<Renderer>() && GetComponent<Renderer>().material && GetComponent<Renderer>().material.HasProperty(textureName)) {
                GetComponent<Renderer>().material.SetTextureOffset(textureName, offset);
            }
        }
    }
}

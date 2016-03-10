using UnityEngine;

public class Boyancy : MonoBehaviour
{
    protected Ocean ocean;
    // Water plane at y = 0
    private float mag = 1.0f;

    private float ypos = 0.0f;
    private Vector3[] blobs;
    private float ax = 2.0f;
    private float ay = 2.0f;

    private float dampCoeff = .2f;

    private bool engine = false;

    void Start()
    {
        //	mag = rigidbody.mass / (ax * ay) * 10;
        this.GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);

        var bounds = this.GetComponent<MeshCollider>().sharedMesh.bounds.size;
        var length = bounds.z;
        var width = bounds.x;

        blobs = new Vector3[(int)(ax * ay)];
        int i = 0;
        float xstep = (float)(1.0 / (ax - 1));
        float ystep = (float)(1.0 / (ay - 1));

        for (int x = 0; x < ax; x++)
        {
            for (int y = 0; y < ay; y++)
            {
                blobs[i] = new Vector3((-0.5f + x * xstep) * width, 0.0f, (-0.5f + y * ystep) * length) + Vector3.up * ypos;
                i++;
            }
        }
    }

    void Update()
    {
        //	if (Input.GetButton ("Fire1")){
        //		engine = !engine;
        //blobs[0] = null;
        //	}
    }

    void FixedUpdate()
    {
        for (int i = 0; i < blobs.Length; i++)
        {
            Vector3 blob = blobs[i];
            if (blob != null)
            {
                Vector3 wpos = transform.TransformPoint(blob);
                float damp = this.GetComponent<Rigidbody>().GetPointVelocity(wpos).y;
                if (ocean)
                    this.GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * (mag * (wpos.y - ocean.GetWaterHeightAtLocation(wpos.x, wpos.z)) + dampCoeff * damp), wpos);
                else
                    this.GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.up * (mag * (wpos.y) + dampCoeff * damp), wpos);

            }
            if (engine)
                this.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 40.0f, transform.TransformPoint(new Vector3(0.0f, -1.0f, -7.5f)));

        }
    }
}
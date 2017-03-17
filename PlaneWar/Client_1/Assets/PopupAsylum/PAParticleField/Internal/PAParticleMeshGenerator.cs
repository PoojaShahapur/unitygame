using UnityEngine;
using System.Collections.Generic;

public class PAParticleMeshGenerator : MonoBehaviour
#if UNITY_4_6 || UNITY_5
	, ISerializationCallbackReceiver
#endif
{
	[HideInInspector]
	[SerializeField]
	protected Vector3[] verts;
	[HideInInspector]
	[SerializeField]
	protected Vector3[] normals;
	[HideInInspector]
	[SerializeField]
	protected Vector4[] tangents;
	[HideInInspector]
	[SerializeField]
	protected Vector2[] uv0;
	[HideInInspector]
	[SerializeField]
	protected Vector2[] uv1;
	[HideInInspector]
	[SerializeField]
	protected Color[] colors;
	[HideInInspector]
	[SerializeField]
	protected int[] triangles;
	
	int cachedSeed = 0;

	#region ISerializationCallbackReceiver implementation

	public void OnBeforeSerialize ()
	{
		PAParticleField papf = GetComponent<PAParticleField> ();
		if (papf && papf.clearCacheInBuilds) {
			ClearCache();
		}
	}

	public void OnAfterDeserialize ()
	{
		return;
	}

	#endregion
	
	protected void CacheSeed(){
		cachedSeed = Random.seed;
	}
	
	void SetSeed(int seed){
		Random.seed = seed;
	}
	
	protected float GetRandomAndIncrement(float min, float max){
		float result = Random.Range (min, max);
		Random.seed += 1;
		return result;
	}
	
	protected void ResetSeed(){
		Random.seed = cachedSeed;
	}
	
	/// <summary>
	/// Gets the maximum particle count.
	/// </summary>
	/// <returns>The maximum particle count.</returns>
	public virtual int GetMaximumParticleCount(){return 16250;}

    /// <summary>
    /// returns the maximum distance in any direction of a particle whose size is 1 unit
    /// </summary>
    /// <returns></returns>
    public virtual float GetParticleBaseSize() { return 1f; }
	
	protected void SkipRandomCalls(int callsPerParticle, int count){
		for (int i = 0; i < callsPerParticle * count; i++) {
			GetRandomAndIncrement(0f, 0f);
		}
	}
	
	/// <summary>
	/// Populates the mesh.
	/// </summary>
	/// <param name="mesh">Mesh.</param>
	/// <param name="settings">Settings.</param>
	public virtual void UpdateMesh(Mesh mesh, PAParticleField settings){
		
		CacheSeed ();

		int count = GetClampedParticleCount (settings.particleCount);
		int startAt = -1;

		if (verts == null || verts.Length == 0) {
			verts = new Vector3[0];
			normals = new Vector3[0];
			tangents = new Vector4[0];
			uv0 = new Vector2[0];
			uv1 = new Vector2[0];
			colors = new Color[0];
			triangles = new int[0];

			startAt = 0;
		}

		if ((settings.meshIsDirtyMask & MeshFlags.Count)!=0 || startAt == 0) {
			startAt = SetParticleCapacity (count);

			if (startAt == count){
				FillMesh(mesh);
				return;
			}else{
				UpdateTriangles(settings, startAt);
			}
		}
		
		if ((settings.meshIsDirtyMask & MeshFlags.Seed) != 0 || startAt != -1) {
			SetSeed (settings.seed);
			UpdateDirection (settings, Mathf.Max(0,startAt));
		}
		
		if ((settings.meshIsDirtyMask & MeshFlags.Surface) != 0 || startAt != -1) {
			SetSeed (settings.seed);
			UpdateSurface (settings, Mathf.Max(0,startAt));
		}
		
		if ((settings.meshIsDirtyMask & MeshFlags.Speed) != 0 || startAt != -1) {
			SetSeed (settings.seed);
			UpdateSpeed (settings, Mathf.Max(0,startAt));
		}
		
		if ((settings.meshIsDirtyMask & MeshFlags.Color) != 0 || startAt != -1) {
			SetSeed (settings.seed);
			UpdateColor (settings, Mathf.Max(0,startAt));
		}
		
		ResetSeed ();

		FillMesh (mesh);
	}

	void FillMesh(Mesh mesh){		
		mesh.Clear ();
		
		mesh.vertices = verts;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.uv = uv0;
		mesh.uv2 = uv1;
		mesh.colors = colors;
		mesh.triangles = triangles;
	}

	public int GetClampedParticleCount(int count){
		int max = GetMaximumParticleCount();
		if (count > max) {
			return max;
		} else {
			return count;
		}
	}
	
	/// <summary>
	/// Sets the particle count.
	/// </summary>
	/// <returns>The particle index to regenerate from</returns>
	/// <param name="count">Count.</param>
	protected virtual int SetParticleCapacity(int count){
		return -1;
	}

	protected void SetArraySizes(int vertCount, int triCount){
		Vector3[] verts = new Vector3[vertCount];
		Vector3[] normals = new Vector3[vertCount];
		Vector4[] tangents = new Vector4[vertCount];
		Vector2[] uv0 = new Vector2[vertCount];
		Vector2[] uv1 = new Vector2[vertCount];
		Color[] colors = new Color[vertCount];
		int[] triangles = new int[triCount];

		int copyLength = Mathf.Min (this.verts.Length, vertCount);

		for (int i = 0; i < copyLength; i++) {
			verts[i] = this.verts[i];
			normals[i]=this.normals[i];
			tangents[i]=this.tangents[i];
			uv0[i]=this.uv0[i];
			uv1[i]=this.uv1[i];
			colors[i]=this.colors[i];
		}

		copyLength = Mathf.Min (this.triangles.Length, triCount);

		for (int i=0; i<copyLength; i++) {
			triangles[i]=this.triangles[i];
		}

		this.verts = verts;
		this.normals = normals;
		this.tangents = tangents;
		this.uv0 = uv0;
		this.uv1 = uv1;
		this.colors = colors;
		this.triangles = triangles;
	}
	
	protected virtual void UpdateDirection(PAParticleField settings, int startAt){
		
	}
	
	protected virtual void UpdateSurface(PAParticleField settings, int startAt){
		
	}
	
	protected virtual void UpdateSpeed(PAParticleField settings, int startAt){
		
	}
	
	protected virtual void UpdateColor(PAParticleField settings, int startAt){
		
	}

	protected virtual void UpdateTriangles(PAParticleField settings, int startAt){

	}

    public void ClearCache()
    {
        verts = new Vector3[0];
        normals = new Vector3[0];
        tangents = new Vector4[0];
        uv0 = new Vector2[0];
        uv1 = new Vector2[0];
        colors = new Color[0];
        triangles = new int[0];
    }
}

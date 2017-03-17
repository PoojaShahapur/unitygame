using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PAExclusionZone : MonoBehaviour {

	public static List<PAExclusionZone> exclusionZones;

	public LayerMask affectsLayers = -1;
	public Vector3 edgeThreshold = new Vector3 (0.9f, 0.9f, 0.9f);
	public bool important = false;

	public static void RegisterZone(PAExclusionZone zone){
		if (exclusionZones == null){
			exclusionZones = new List<PAExclusionZone>();
		}
		if (!exclusionZones.Contains (zone)) {
			exclusionZones.Add(zone);
		}
	}

	public static void UnregisterZone(PAExclusionZone zone){
		if (exclusionZones != null && exclusionZones.Contains (zone)) {
			exclusionZones.Remove(zone);
		}
		exclusionZones.RemoveAll((obj) => obj == null);
	}

	void OnEnable(){
		RegisterZone (this);
	}

	void OnDisable(){
		UnregisterZone (this);
	}

	void OnDrawGizmos(){
#if UNITY_EDITOR
		Vector3 half = Vector3.one * 0.5f;
		
		Vector3 a, b, c, d, e, f, g, h;
		a = new Vector3 (-half.x, -half.y, half.z);
		b = new Vector3 (half.x, -half.y, half.z);
		c = new Vector3 (half.x, half.y, half.z);
		d = new Vector3 (-half.x, half.y, half.z);
		
		e = new Vector3 (-half.x, -half.y, -half.z);
		f = new Vector3 (half.x, -half.y, -half.z);
		g = new Vector3 (-half.x, half.y, -half.z);
		h = new Vector3 (half.x, half.y, -half.z);

		a = transform.TransformPoint(a);
		b = transform.TransformPoint(b);
		c = transform.TransformPoint(c);
		d = transform.TransformPoint(d);
		e = transform.TransformPoint(e);
		f = transform.TransformPoint(f);
		g = transform.TransformPoint(g);
		h = transform.TransformPoint(h);
		
		Color col = Gizmos.color;
		
		Gizmos.color = new Color(1,1,1, 0.3f);
		
		// draw front
		Gizmos.DrawLine(a, b);
		Gizmos.DrawLine(a, d);
		Gizmos.DrawLine(c, b);
		Gizmos.DrawLine(c, d);
		// draw back
		Gizmos.DrawLine(e, f);
		Gizmos.DrawLine(e, g);
		Gizmos.DrawLine(h, f);
		Gizmos.DrawLine(h, g);
		// draw corners
		Gizmos.DrawLine(e, a);
		Gizmos.DrawLine(f, b);
		Gizmos.DrawLine(g, d);
		Gizmos.DrawLine(h, c);
		
		Gizmos.color = col;
#endif
	}

#if UNITY_EDITOR
	void Update(){
		edgeThreshold = new Vector3 (Mathf.Clamp01 (edgeThreshold.x), Mathf.Clamp01 (edgeThreshold.y), Mathf.Clamp01 (edgeThreshold.z));
		RegisterZone (this);
	}
#endif

	Bounds bounds{
		get{
			Vector3 max = Vector3.Max(transform.TransformPoint(Vector3.right * 0.5f),transform.TransformPoint(Vector3.left * 0.5f));
			max = Vector3.Max(transform.TransformPoint(Vector3.up * 0.5f), max);
			max = Vector3.Max(transform.TransformPoint(Vector3.down * 0.5f), max);
			max = Vector3.Max(transform.TransformPoint(Vector3.forward * 0.5f), max);
			max = Vector3.Max(transform.TransformPoint(Vector3.back * 0.5f), max);

			return new Bounds(transform.position, (max - transform.position) * 2f);
		}
	}

	static Vector3 ClosestPointOnBounds(Bounds bounds, Vector3 point){
#if UNITY_5_0
		return bounds.ClosestPoint(point);
#else
		if (bounds.Contains (point)) {
			return point;
		} else {
			Vector3 closestPoint = new Vector3();
			closestPoint.x = (point.x < bounds.min.x) ? bounds.min.x : (point.x > bounds.max.x) ? bounds.max.x : point.x;
			closestPoint.y = (point.y < bounds.min.y) ? bounds.min.y : (point.y > bounds.max.y) ? bounds.max.y : point.y;
			closestPoint.z = (point.z < bounds.min.z) ? bounds.min.z : (point.z > bounds.max.z) ? bounds.max.z : point.z;
			return closestPoint;
		}
#endif
	}

	public static bool GetExclusionZones(ref PAExclusionZone[] zones, Vector3 position, Bounds checkBounds, int layer){

		bool foundExclusionZones = false;

		if (exclusionZones == null) {
			return foundExclusionZones;
		}

		List<PAExclusionZone> zonesInBounds = exclusionZones.FindAll ((obj) => checkBounds.Intersects (obj.bounds) && (1<<layer & obj.affectsLayers) != 0);

		if (zonesInBounds.Count > 3) {
			zonesInBounds.Sort((x, y) => Vector3.SqrMagnitude((x.important ? position : ClosestPointOnBounds(x.bounds, position)) - position).CompareTo(Vector3.SqrMagnitude((y.important ? position : ClosestPointOnBounds(y.bounds, position)) - position)));
		}

		for (int i = 0; i < 3; i++) {
			if (zonesInBounds.Count>i){
				zones[i] = zonesInBounds[i];
				foundExclusionZones = true;
			}else{
				zones[i] = null;
			}
		}

		return foundExclusionZones;
	}

	/// <summary>
	/// Create a particle field with the specified name.
	/// </summary>
	/// <param name="name">Name.</param>
	public static PAExclusionZone Create(string name){
		return (new GameObject (name)).AddComponent<PAExclusionZone> ();
	}
}

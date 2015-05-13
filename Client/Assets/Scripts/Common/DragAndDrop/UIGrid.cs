//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>

public class UIGrid : LSBehaviour
{
	public delegate void OnReposition ();

    /// <summary>
    /// Maximum children per line.
    /// If the arrangement is horizontal, this denotes the number of columns.
    /// If the arrangement is vertical, this stands for the number of rows.
    /// </summary>

    public int maxPerLine = 1;

	/// <summary>
	/// The width of each of the cells.
	/// </summary>

	public float cellWidth = 1f;

	/// <summary>
	/// The height of each of the cells.
	/// </summary>

	public float cellHeight = 1f;

	/// <summary>
	/// Callback triggered when the grid repositions its contents.
	/// </summary>

	public OnReposition onReposition;

	protected bool mReposition = false;

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public bool repositionNow { set { if (value) { mReposition = true; } } }

    // 最后一次位置
    Vector3 m_lastpostion;
    // 隐藏位置
    public float m_hideZPos;

	/// <summary>
	/// Get the current list of the grid's children.
	/// </summary>

	public List<Transform> GetChildList ()
	{
		Transform myTrans = transform;
		List<Transform> list = new List<Transform>();

		for (int i = 0; i < myTrans.childCount; ++i)
		{
			Transform t = myTrans.GetChild(i);
		    list.Add(t);
		}

		return list;
	}

    public int getChildCount()
    {
        return transform.childCount;
    }

	/// <summary>
	/// Convenience method: get the child at the specified index.
	/// Note that if you plan on calling this function more than once, it's faster to get the entire list using GetChildList() instead.
	/// </summary>

	public Transform GetChild (int index)
	{
		List<Transform> list = GetChildList();
		return (index < list.Count) ? list[index] : null;
	}

	/// <summary>
	/// Get the index of the specified item.
	/// </summary>

	public int GetIndex (Transform trans) { return GetChildList().IndexOf(trans); }

	/// <summary>
	/// Convenience method -- add a new child.
	/// </summary>

	public void AddChild (Transform trans) { AddChild(trans, true); }

	/// <summary>
	/// Convenience method -- add a new child.
	/// Note that if you plan on adding multiple objects, it's faster to GetChildList() and modify that instead.
	/// </summary>

	public void AddChild (Transform trans, bool sort)
	{
		if (trans != null)
		{
			//trans.parent = transform;
            trans.SetParent(transform, false);
			ResetPosition(GetChildList());
		}
	}

	/// <summary>
	/// Remove the specified child from the list.
	/// Note that if you plan on removing multiple objects, it's faster to GetChildList() and modify that instead.
	/// </summary>

	public bool RemoveChild (Transform t)
	{
		List<Transform> list = GetChildList();

		if (list.Remove(t))
		{
			ResetPosition(list);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Cache everything and reset the initial position of all children.
	/// </summary>

	public override void Start ()
	{
		Reposition();
	}

	/// <summary>
	/// Reset the position if necessary, then disable the component.
	/// </summary>

	protected virtual void Update ()
	{
		Reposition();
	}

	/// <summary>
	/// Reposition the content on inspector validation.
	/// </summary>

	void OnValidate () { if (!Application.isPlaying) Reposition(); }

	// Various generic sorting functions
	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }
	static public int SortHorizontal (Transform a, Transform b) { return a.localPosition.x.CompareTo(b.localPosition.x); }
	static public int SortVertical (Transform a, Transform b) { return b.localPosition.y.CompareTo(a.localPosition.y); }

	/// <summary>
	/// You can override this function, but in most cases it's easier to just set the onCustomSort delegate instead.
	/// </summary>

	protected virtual void Sort (List<Transform> list) { }

	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>

	[ContextMenu("Execute")]
	public virtual void Reposition ()
	{
		// Get the list of children in their current order
		List<Transform> list = GetChildList();

		// Reset the position and order of all objects in the list
		ResetPosition(list);

		// Notify the listener
		if (onReposition != null)
			onReposition();
	}

	/// <summary>
	/// Reset the position of all child objects based on the order of items in the list.
	/// </summary>

	protected void ResetPosition (List<Transform> list)
	{
		mReposition = false;

		int x = 0;
		int z = 0;
		//Transform myTrans = transform;

		// Re-add the children in the same order we have them in and position them accordingly
		for (int i = 0, imax = list.Count; i < imax; ++i)
		{
			Transform t = list[i];

			Vector3 pos = t.localPosition;
			float depth = pos.y;
            x = i % maxPerLine;
            z = i / maxPerLine;
            // 放在一个格子的中间
            pos = new Vector3(cellWidth * x + cellWidth / 2, depth, -cellHeight * z - cellHeight / 2);

            t.localPosition = pos;
		}
	}

    // 隐藏就是移动到很远
    public void hideGrid()
    {
        m_lastpostion = transform.localPosition;
        transform.Translate(new Vector3(0, 0, m_hideZPos));
    }

    // 显示
    public void showGrid()
    {
        transform.localPosition = m_lastpostion;
    }
}

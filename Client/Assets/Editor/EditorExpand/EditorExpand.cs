using UnityEngine;
using UnityEditor;
using System.Collections;

public class MyHierarchyMenu
{
	[MenuItem("Window/Test/Test_0")]
	static void Test()
	{
	}

	[MenuItem("Window/Test/Test_1")]
	static void Test1()
	{
	}

	[InitializeOnLoadMethod]
	static void StartInitializeOnLoadMethod()
	{
	    EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
	}
 
	static void OnHierarchyGUI(int instanceID, Rect selectionRect)
	{
	    if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
	         && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
	    {
	        GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			 //这里可以判断selectedGameObject的条件
	        if (selectedGameObject)
	        {
				Vector2 mousePosition = Event.current.mousePosition;

				EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Window/Test",null);
	            Event.current.Use();
	        }
	    }
	}
}
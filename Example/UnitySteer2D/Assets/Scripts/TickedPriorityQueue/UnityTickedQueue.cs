using System;
using UnityEngine;
using TickedPriorityQueue;
using System.Collections.Generic;
using System.Linq;

public class UnityTickedQueue : MonoBehaviour
{
	private static Dictionary<string, UnityTickedQueue> _instances;
	private static UnityTickedQueue _instance;
    
	public static UnityTickedQueue Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = CreateInstance(null);
			}
			return _instance;
		}
	}

	public static UnityTickedQueue GetInstance(string name)
	{
		if (string.IsNullOrEmpty(name)) return Instance;
		name = name.ToLower();
		
		UnityTickedQueue queue = null;
		if (_instances == null)
			_instances = new Dictionary<string, UnityTickedQueue>();
		else
		{
			_instances.TryGetValue(name, out queue);
		}
		
		if (queue == null)
		{
			queue = CreateInstance(name);
			_instances[name] = queue;
		}
		
		return queue;
	}
	
	private static UnityTickedQueue CreateInstance(string name)
	{
		if (string.IsNullOrEmpty(name)) name = "Ticked Queue";
		else name = "Ticked Queue - " + name;
		GameObject go = new GameObject(name);
		return go.AddComponent<UnityTickedQueue>();
	}
	
	private TickedQueue _queue = new TickedQueue();

	public bool IsPaused {
		get {
			return _queue.IsPaused;
		}
		set {
			_queue.IsPaused = value;

			enabled = !value;
		}
	}
		
	public TickedQueue Queue {
		get {
			return this._queue;
		}
	}
	
	void OnEnable()
	{
		_queue.TickExceptionHandler = LogException;
	}
	
	public void Add(ITicked ticked)
	{
		_queue.Add(ticked);
	}
	
	public bool  Remove(ITicked ticked)
	{
		return _queue.Remove(ticked);
	}
	
	public int MaxProcessedPerUpdate
	{
		get { return _queue.MaxProcessedPerUpdate; }
		set { _queue.MaxProcessedPerUpdate = value; }
	}
	
	private void Update()
	{
		_queue.Update();
	}

	void LogException(Exception e, ITicked ticked)
	{
		Debug.LogException(e, this);
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleInteger {
	public int data;
}

public class TestOnly : MonoBehaviour {

	List<SimpleInteger> list_of_ints;
	Queue<SimpleInteger> queue_of_ints;
	private void Awake() {
		list_of_ints = new List<SimpleInteger>(10000);
		queue_of_ints = new Queue<SimpleInteger>(10000);

		for ( int i = 0; i < 10000; i += 1 ) {
			list_of_ints.Add(new SimpleInteger { data = 1 });
			queue_of_ints.Enqueue(new SimpleInteger { data = 1 });
		}
	}

	// Start is called before the first frame update
	void Start() {
		var si = new SimpleInteger { data = 1 };
		DateTime start = DateTime.Now;

		for ( int x = 0; x < 1000000; x += 1 ) {
			list_of_ints.RemoveAt(0);
			list_of_ints.Add(si);
		}

		DateTime end = DateTime.Now;
		Debug.Log("Insert at the end and remove at 0: " + (end.Ticks - start.Ticks).ToString());

		start = DateTime.Now;

		for ( int x = 0; x < 1000000; x += 1 ) {
			queue_of_ints.Dequeue();
			queue_of_ints.Enqueue(si);
		}

		end = DateTime.Now;
		Debug.Log("Insert at the end and remove at 0: " + (end.Ticks - start.Ticks).ToString());


		start = DateTime.Now;
		foreach ( var d in list_of_ints ) {
			d.data += 1;
		}
		end = DateTime.Now;
		Debug.Log("Traverse: " + (end.Ticks - start.Ticks).ToString());


		start = DateTime.Now;
		foreach ( var d in queue_of_ints ) {
			d.data += 1;
		}
		end = DateTime.Now;
		Debug.Log("Traverse: " + (end.Ticks - start.Ticks).ToString());
	}

	// Update is called once per frame
	void Update() {

	}
}

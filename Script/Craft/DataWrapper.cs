using System;
using System.Collections.Generic;
using UnityEngine;


public class DataWrapper : MonoBehaviour {
	/// <summary>
	/// The timing is stored in the taplist
	/// Unit: second
	/// </summary>
	public List<float> taplist;
	private void Awake() {
		DontDestroyOnLoad(this);
	}
}


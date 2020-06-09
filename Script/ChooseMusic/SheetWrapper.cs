using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SheetWrapper : MonoBehaviour {
	public Sheet data;
	public AudioClip musicclip;

	private void Awake() {
		DontDestroyOnLoad(this);
	}
}
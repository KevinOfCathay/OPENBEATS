using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UILightSystem : MonoBehaviour {
	public GameObject templight;
	public GameObject lights;
	private void Awake() {
		// Instantiate a bunch of lights
		for ( int a = 0; a < 40; a += 1 ) {
			var light = Instantiate(templight);
			light.transform.SetParent(lights.transform);
			light.GetComponent<UILight>().Set(G.rng.Next(0, 20) / 10f, new Vector3(G.rng.Next(-960, 960), G.rng.Next(-540, 540), 1f), G.rng.Next(0, 60) / 10f);
		}
	}
}

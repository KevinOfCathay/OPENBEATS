using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DDO : MonoBehaviour {
	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}


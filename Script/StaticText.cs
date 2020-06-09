using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticText : MonoBehaviour {
	public Text txt;
	public Text tip;

	/// <summary>
	/// Chinese, English, Japanese
	/// </summary>
	[TextArea(0, 5)] public string[] text;
	[TextArea(0, 5)] public string[] tips;

	private void Awake() {
		txt.text = text[G.setting.language];
	}

	private void Refresh() {
		txt.text = text[G.setting.language];
	}
}

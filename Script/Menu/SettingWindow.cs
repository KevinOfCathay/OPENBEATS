using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour {
	public Slider musicvolume, sevolume;
	public GameObject window;
	public Dropdown[] dropdown;
	public void Open() {
		musicvolume.value = G.setting.musicvolume;
		sevolume.value = G.setting.sevolume;
		window.transform.localScale = new Vector3(1f, 1f, 1f);

		for ( int i = 0; i < G.LANES; i += 1 ) {
			dropdown[i].value = (int) G.setting.lanekey[i] - 97;
		}
	}

	public void ChangeLanguage(int lang) {
		G.setting.language = lang;
	}
	public void ChangeMusicVolume() {
		G.setting.musicvolume = musicvolume.value;
	}
	public void ChangeSEVolume() {
		G.setting.sevolume = sevolume.value;
	}

	public void ChangeLaneKeyBinding(int laneindex) {
		G.setting.lanekey[laneindex] = (KeyCode) (dropdown[laneindex].value + 97);
	}

	/// <summary> Save settings
	/// </summary>
	public void OK() {
		FileSystem.SaveSetting();
		window.transform.localScale = new Vector3(0f, 0f, 0f);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Nav_Menu : MonoBehaviour {
	public NowLoading loading;
	public SettingWindow sw;
	public void ChooseMusic() {
		loading.LoadNextScene("ChooseMusic");
	}
	public void Craft() {
		loading.LoadNextScene("ChooseCraftMusic");
	}
	public void Setting() {
		sw.Open();
	}
	public void Exit() {
		Application.Quit(100);
	}
}
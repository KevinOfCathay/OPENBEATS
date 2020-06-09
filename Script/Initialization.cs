using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour {
	public Scrollbar progress;
	public Language language_file;
	public NowLoading loading;

	private void Awake() {
		FileSystem.LoadSetting();

		G.lang = language_file;
		G.DATA_PATH = Application.dataPath;
		G.VERSION = Application.version;

		Debug.Log("Application.dataPath" + Application.dataPath);
	}
	/// <summary>
	/// 1. Load all music sheets in the directory, and put them in a hashset
	/// </summary>
	private void Start() {
		// If there is no data, then we create a directory to save our data
		FileSystem.Load_AllSheets();
		loading.LoadNextScene("Menu");
	}
}


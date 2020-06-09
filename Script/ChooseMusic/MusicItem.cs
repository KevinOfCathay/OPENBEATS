using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MusicItem : MonoBehaviour {
	public FileSystem.FilePath path;
	public MessageBox msgbox;
	public NowLoading loading;
	public SheetWrapper wrapper;
	public Text title, author, difficulty;

	public void SetText(string title, string author, int difficulty) {
		this.title.text = title;
		this.author.text = author;
		this.difficulty.text = difficulty.ToString();
	}

	public void Play() {
		// Load the sheet data
		try {
			Sheet sheet = FileSystem.Load_MusicSheet(path.sheetpath);
			wrapper.data = sheet;
			if ( sheet != null ) {
				// Load music using web request
				string uri = "file:///" + path.musicpath;
				Debug.Log(uri);
				UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(uri, G.CRAFT.mtype[path.musictype]);
				StartCoroutine(getaudioclip(req));
			}
			else {
				msgbox.Show(G.lang.message_load_musicdata_failure[G.setting.language]);
			}
		} catch ( Exception e ) {
			msgbox.Show(G.lang.message_onload_musicdata_error[G.setting.language]);
			Debug.Log(e.Message);
		}
	}

	private IEnumerator getaudioclip(UnityWebRequest req) {
		yield return req.Send();
		if ( req.isNetworkError ) {
			Debug.Log(req.error);
		}
		else {
			wrapper.musicclip = DownloadHandlerAudioClip.GetContent(req);
			loading.LoadNextScene("Main");
		}
	}
}

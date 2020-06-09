using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CCMusicItem : MonoBehaviour {
	public Text title;
	public string musicpath;
	public string musictype;
	public AudioSource music;
	public NowLoading loading;

	public void Set(string filename, string musicpath, string extension) {
		this.title.text = filename;
		this.musicpath = musicpath;
		this.musictype = extension;
	}

	public void Click() {
		if ( File.Exists(musicpath) ) {
			string uri = "file:///" + musicpath;
			Debug.Log(uri);
			UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(uri, G.CRAFT.mtype[musictype]);
			StartCoroutine(getaudioclip(req));
		}
	}

	private IEnumerator getaudioclip(UnityWebRequest req) {
		yield return req.Send();
		if ( req.isNetworkError ) {
			Debug.Log(req.error);
		}
		else {
			G.CRAFT.currentmusic_path = musicpath;
			music.clip = DownloadHandlerAudioClip.GetContent(req);
			loading.LoadNextScene("Craft");
		}
	}
}

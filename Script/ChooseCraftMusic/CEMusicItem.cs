using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CEMusicItem : MonoBehaviour {
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
}

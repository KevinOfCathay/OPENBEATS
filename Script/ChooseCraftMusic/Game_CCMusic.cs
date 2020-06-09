using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Game_CCMusic : MonoBehaviour {
	public GameObject musicitem, esheetitem;
	public GameObject itemgroup, eitemgroup;

	public MessageBox msgbox;
	public NowLoading loading;
	public AudioSource music;

	private Vector3 anchor;
	private readonly int columns = 4;
	private readonly float width = 340f, height = 110f;

	/// <summary> Create music items based on the files in our temp craft folder
	/// </summary>
	private void Awake() {
		anchor = musicitem.transform.localPosition;

		var musiclist = FileSystem.LoadCraftTempMusic();
		if ( musiclist != null ) {
			int r = 0, c = 0;
			foreach ( var (musicpath, musicname, extension) in musiclist ) {
				var newitem = Instantiate(musicitem);
				newitem.transform.SetParent(itemgroup.transform);

				newitem.GetComponent<CCMusicItem>().Set(musicname, musicpath, extension);
				newitem.transform.localPosition = new Vector3(anchor.x + c * width, anchor.y - r * height, anchor.z);

				c += 1;
				if ( c >= columns ) {
					c = 0;
					r += 1;
				}
			}

			r = 0; c = 0;
			foreach ( var sheet in FileSystem.musicsheet_lib ) {
				var newitem = Instantiate(esheetitem);
				newitem.transform.SetParent(eitemgroup.transform);

				newitem.GetComponent<CEMusicItem>().Set(sheet.Value.title, sheet.Key.musicpath, sheet.Key.musictype);
				newitem.transform.localPosition = new Vector3(anchor.x + c * width, anchor.y - r * height, anchor.z);
			}
		}
		Destroy(musicitem);
		music.volume = G.setting.musicvolume;
	}

	public void BacktoMenu() {
		Destroy(music.gameObject);
		loading.LoadNextScene("Menu");
	}
}


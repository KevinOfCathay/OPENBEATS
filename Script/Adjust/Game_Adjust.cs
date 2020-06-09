using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Game_Adjust : MonoBehaviour {
	// A class that stores note data
	// Name has no real meaning
	public sealed class N {
		public N(float a, int l, GameObject n) { atime = a; lane = l; noteobj = n; }
		public float atime;
		public int lane;
		public GameObject noteobj;
	}
	public GameObject tempnote;
	public GameObject musicsheet;
	public GameObject[] lanes;
	public MessageBox msgbox;
	public InputBox inbox;
	public SelectionControl sc;
	public GroupEditing ge;
	public NowLoading loading;
	public InputField author, title, difficulty, aside;
	public Button previewbtn, savebtn, autocompletebtn;
	private bool is_previewing = false;

	/// <summary> Timer and noteindex controls the music sheet preview </summary>
	/// <para>Timer unit: ms</para>
	private float timer = 0f;
	private int noteindex = 0;

	public AudioSource music;
	public AudioSource[] se;

	public List<N> taplist;

	/// <summary> Selected notes index are added in this group list </summary>
	public List<N> selected;

	/// <summary> The shift of the music sheet object </summary>
	/// <para>Unit: position</para>
	public float offset = 0f;

	/// <summary> The x coord of all the lanes </summary>
	private float[] lanes_xpos;

	/// <summary> Active Line y coord </summary>
	private readonly float activeline = -500.0f;

	private readonly Vector3 sheetshift = new Vector3(0f, 5f, 0f);
	private void Awake() {
		music = GameObject.Find("Music").GetComponent<AudioSource>();
		var obj = GameObject.Find("Data");

		selected = new List<N>(15);
		taplist = new List<N>(100);
		lanes_xpos = new float[5];
		for ( int i = 0; i < 5; i += 1 ) {
			lanes_xpos[i] = lanes[i].transform.localPosition.x;
		}
		obj.GetComponent<DataWrapper>().taplist.ForEach(singletap => taplist.Add(new N(singletap, 2, null)));

		Destroy(obj);
		SetNotesOnScreen();
		Array.ForEach(se, x => x.volume = G.setting.sevolume);
	}

	private void Start() {
		msgbox.Show(G.lang.message_sheet_edit_guides[G.setting.language]);
	}

	private void Update() {
		if ( is_previewing ) {
			var deltatime = Time.deltaTime;
			timer += deltatime;

			//Move the sheet
			musicsheet.transform.localPosition -= new Vector3(0f, G.CRAFT.NOTES_SPEED * deltatime, 0f);
			offset += G.CRAFT.NOTES_SPEED * deltatime;

			while ( taplist[noteindex].atime <= timer ) {
				Debug.Log("SE Played at " + timer.ToString() + " Note index " + noteindex.ToString());
				se[taplist[noteindex].lane].Play();
				noteindex += 1;
				if ( noteindex >= taplist.Count ) {
					is_previewing = false; previewbtn.interactable = true; music.Stop();
					break;
				}
			}
		}
		else {
			// When player press S key:
			// Shift the whole music sheet down
			if ( Input.GetKey(KeyCode.S) ) {
				sc.DeSelect();
				if ( offset >= 0f ) {
					musicsheet.transform.localPosition -= sheetshift;
					offset += 5f;
				}
			}
			// When player press W key:
			// Shift the whole music sheet up
			else if ( Input.GetKey(KeyCode.W) ) {
				sc.DeSelect();
				if ( offset > 0f ) {
					musicsheet.transform.localPosition += sheetshift;
					offset -= offset >= 5f ? 5f : offset;
				}
			}
			else if ( Input.GetKey(KeyCode.R) ) {
				ge.ShiftUp();
			}
			else if ( Input.GetKey(KeyCode.F) ) {
				ge.ShiftDown();
			}
			else if ( Input.GetKey(KeyCode.D) ) {
				ge.ShiftLeft();
			}
			else if ( Input.GetKey(KeyCode.G) ) {
				ge.ShiftRight();
			}
		}
	}

	public void ResetNote(N note) {
		note.noteobj.transform.localPosition = new Vector3(lanes_xpos[note.lane], activeline + note.atime * G.CRAFT.NOTES_SPEED, 0f);
	}

	/// <summary>
	/// Transfer the timing data (float numbre) to Note object
	/// </summary>
	public void TransferAndSave() {
		// verify input
		if ( author.text == "" || title.text == "" || difficulty.text == "" ) {
			msgbox.Show(G.lang.message_please_fill_required_fields[G.setting.language]);
			return;
		}
		else if ( taplist.Count == 0 ) {
			return;
		}

		SortN();

		int id = 0;
		var sheet = new Sheet();
		foreach ( var n in taplist ) {
			Note newn = new Note {
				atime = n.atime,
				ID = id,
				lane = n.lane
			};
			sheet.data.Add(newn);
			id += 1;
		}
		sheet.summary.author = author.text;
		sheet.summary.title = title.text;
		sheet.summary.difficulty = int.Parse(difficulty.text);
		sheet.summary.aside = aside.text;
		sheet.summary.endtime = taplist[taplist.Count - 1].atime + 2f;

		// Save the music sheet
		FileSystem.Save_MusicSheet(sheet, title.text);
	}

	/// <summary>
	/// Finish crafting the music sheet automatically
	/// </summary>
	public void AutoComplete() {
		autocompletebtn.interactable = false;
		List<N> newtaplist = new List<N>(300);
		foreach ( var n in taplist ) {
			int randomlane = G.rng.Next(0, G.LANES);
			newtaplist.Add(new N(n.atime, randomlane, null));

			// Do we want to make a copy of this note?
			if ( G.rng.Next(0, 2) == 0 ) {
				newtaplist.Add(new N(n.atime, (randomlane + G.rng.Next(1, G.LANES)) % G.LANES, null));
			}
			// Clear all notes on screen
			Destroy(n.noteobj);
		}
		taplist = null;
		taplist = newtaplist;
		sc.DeSelect();

		// Instantiate new notes
		SetNotesOnScreen();
	}

	/// <summary> Play music and beats from a certain point
	/// </summary>
	public void PreviewSheet() {
		if ( !is_previewing ) {
			previewbtn.interactable = false;

			// Calculate the music starting point
			// Offset / Notespeed(p/s) = startpoint s
			timer = offset / G.CRAFT.NOTES_SPEED;


			for ( int a = 0; a < taplist.Count; a += 1 ) {
				if ( taplist[a].atime > timer ) {
					noteindex = a - 1 > 0 ? a - 1 : 0;
					break;
				}
			}

			Debug.Log("Preview start point: " + (timer).ToString() + " " + noteindex.ToString());
			music.time = timer;
			music.Play();
			is_previewing = true;
		}
	}

	public void BacktoMenu() {
		Destroy(music.gameObject);
		loading.LoadNextScene("Menu");
	}

	public void SortN() {
		taplist.Sort(
			(a, b) => { if ( a.atime > b.atime ) { return 1; } else { return -1; } }
			);
	}

	private void SetNotesOnScreen() {
		// Put all the notes on the screen
		foreach ( var n in taplist ) {
			var instnote = Instantiate(tempnote);
			instnote.transform.SetParent(musicsheet.transform);

			// Calculate the note position based on the tap timing
			// Distance = Traval speed * Active time
			instnote.transform.localPosition = new Vector3(lanes_xpos[n.lane], activeline + n.atime * G.CRAFT.NOTES_SPEED, 0f);
			n.noteobj = instnote;
		}
	}

}
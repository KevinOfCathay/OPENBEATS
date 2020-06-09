using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Game : MonoBehaviour {
	/// <summary> The music sheet we want to play
	/// </summary>
	public Sheet stock = new Sheet();
	public Stack<Note> pending = new Stack<Note>();
	public TapArea[] lanes;

	/// <summary>active: Active note container.
	/// </summary>
	public Queue<Note>[] active;
	public List<GameObject> note_obj;

	/// <summary> To determine whether player is holding a lane
	/// <para>Not being used right now</para>
	/// </summary>
	public bool[] hold;

	/// <summary> timer, unit: ms
	/// </summary>
	public float timer = 0f;

	/// <summary> control timer's on/off
	/// </summary>
	public bool timerswitch;
	public Text t_time;

	public int points;
	public Text t_points;

	public GameObject note_template;
	public GameObject board;

	public AudioSource music;
	public AudioSource[] se;

	/// <summary> Points where the notes should be spawn.
	/// </summary>
	public GameObject[] anchors;
	public Vector3[] anchor_pos = new Vector3[G.LANES];
	public Vector3[] anchor_rot = new Vector3[G.LANES];

	public int combo = 0;

	public bool is_play = false;

	/// <summary>
	/// The y coord of the spawn-line, active-line, end-line
	/// spawn-line: where notes are spawn
	/// active-line: where notes should be activated
	/// 
	/// we are using this information to calculate the interval and deadline
	/// These two fields are NOT used right now.
	/// </summary>
	public float sline, aline;

	/// <summary> The place where the note should be destoryed.
	/// </summary>
	public GameObject activeline, spawnline;

	/// <summary> Used for debugging only
	/// </summary>
	private Note tracknote;

	/// <summary> Used on PC platform
	/// </summary>
	public bool usekeycode = true;
	private KeyCode[] lanekeycodes;

	private void Awake() {
		note_obj = new List<GameObject>(500);
		lanekeycodes = new KeyCode[G.LANES];

		timer = -G.DISTANSE_SA;

		// Create objects and initialize data
		active = new Queue<Note>[G.LANES];
		hold = new bool[5] { false, false, false, false, false };

		for ( var i = 0; i < G.LANES; i += 1 ) {
			active[i] = new Queue<Note>(15);

			anchor_pos[i] = anchors[i].transform.localPosition;
			anchor_rot[i] = anchors[i].transform.localEulerAngles;
		}

		// Load music and musicsheet from the previous scene
		var wrapper = GameObject.Find("SheetWrapper").GetComponent<SheetWrapper>();
		stock = wrapper.data;
		stock.Interpret(sline, aline);
		stock.Printout();
		music.clip = wrapper.musicclip;

		SetNotes();

		// Initialize all keycodes
		for ( int i = 0; i < G.LANES; i += 1 ) {
			lanekeycodes[i] = G.setting.lanekey[i];
		}

		// Set volume
		music.volume = G.setting.musicvolume;
		Array.ForEach(se, x => x.volume = G.setting.sevolume);
	}

	private void Update() {
		if ( timerswitch ) {
			t_time.text = timer.ToString();
			timer += Time.deltaTime;
			if ( timer > 0 && !is_play ) { music.Play(); is_play = true; }

			float dtime = Time.deltaTime;
			foreach ( var lane in active ) {
				foreach ( var anote in lane ) {
					anote.noteobj.transform.localPosition -= new Vector3(0.0f, G.NOTE_SPEED * dtime, 0.0f);
				}
			}

			Count();
			if ( stock.summary.endtime <= timer ) {
				timerswitch = false; if ( music.isPlaying ) { music.Stop(); }
			}
		}

		if ( usekeycode ) {
			for ( int i = 0; i < G.LANES; i += 1 ) {
				if ( Input.GetKeyDown(lanekeycodes[i]) ) {
					lanes[i].PointerDown();
				}
				else if ( Input.GetKeyUp(lanekeycodes[i]) ) {
					lanes[i].PointerUp();
				}
			}
		}
	}

	private void SetNotes() {
		// Instantiate all note object prior to the start, and link them to the note
		// [FIX] Creating too many objects is not efficient. Should be fixed in the future.
		foreach ( var n in stock.data ) {
			var note = Instantiate(note_template);

			note.transform.SetParent(board.transform);
			note.transform.localPosition = anchor_pos[n.lane];
			note.transform.localEulerAngles = anchor_rot[n.lane];
			note.transform.localScale = new Vector3(0f, 0f, 0f);
			note.GetComponent<ParticleSystem>().Stop();

			n.noteobj = note;

			note_obj.Add(note);
		}

		tracknote = stock.data[stock.data.Count - 1];
	}

	private void Count() {
		foreach ( var d in active ) {
			if ( d.Count != 0 ) {

				// Note reaches the deadline
				while ( timer >= d.Peek().deadline ) {
					var top = d.Dequeue();
					combo = 0;
					Debug.Log("Note " + top.ID.ToString() + " deadline " + top.deadline.ToString() + "/" + timer.ToString() + " destroyed at " + top.noteobj.transform.localPosition.y);
					Destroy(top.noteobj);

					if ( d.Count == 0 ) { break; }
				}
			}
		}

		if ( stock.data.Count != 0 ) {
			// If it's time to spawn
			while ( stock.data[stock.data.Count - 1].stime <= timer ) {

				Note topnote = stock.data[stock.data.Count - 1];
				topnote.noteobj.transform.localScale = new Vector3(1f, 1f, 1f);
				topnote.noteobj.GetComponent<ParticleSystem>().Play();
				// Pop the top note to the active container
				active[topnote.lane].Enqueue(topnote);

				// Remove the top note from the stock
				stock.data.RemoveAt(stock.data.Count - 1);

				Debug.Log(topnote.ID.ToString() + " poped");

				if ( stock.data.Count == 0 ) {
					break;
				}
			};
		}
	}

	public void start() {
		timerswitch = true;
	}
}
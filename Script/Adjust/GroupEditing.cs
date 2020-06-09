using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GroupEditing : MonoBehaviour {
	public Game_Adjust game;
	public SelectionControl sc;
	public Button p1btn, p2btn, p3btn, p4btn, mhbtn, mvbtn, esbtn;
	public void On() {
		p1btn.interactable = true; p2btn.interactable = true; p3btn.interactable = true; p4btn.interactable = true;
		mhbtn.interactable = true; mvbtn.interactable = true; esbtn.interactable = true;
	}
	public void Off() {
		p1btn.interactable = false; p2btn.interactable = false; p3btn.interactable = false; p4btn.interactable = false;
		mhbtn.interactable = false; mvbtn.interactable = false; esbtn.interactable = false;
	}

	/// <summary>
	/// Active time +
	/// </summary>
	public void ShiftUp() {
		foreach ( var n in game.selected ) {
			n.atime += 0.02f;
		}
		Reposition();
	}


	/// <summary>
	/// Active time -
	/// </summary>
	public void ShiftDown() {
		foreach ( var n in game.selected ) {
			n.atime -= 0.02f;
		}
		Reposition();
	}


	/// <summary>
	/// Lane <--
	/// </summary>
	public void ShiftLeft() {
		foreach ( var n in game.selected ) {
			n.lane = (n.lane + G.LANES - 1) % G.LANES;
		}
		Reposition();
		sc.DeSelect();
	}

	/// <summary>
	/// Lane -->
	/// </summary>
	public void ShiftRight() {
		foreach ( var n in game.selected ) {
			n.lane = (n.lane + 1) % G.LANES;
		}
		Reposition();
		sc.DeSelect();
	}

	public void Even() {
		// Calculate the space
		HashSet<float> group = new HashSet<float>();
		float min = game.selected[0].atime;
		float max = game.selected[game.selected.Count - 1].atime;

		Debug.Log("Before editing");
		foreach ( var n in game.selected ) {
			var atime = n.atime;
			group.Add(atime);
			Debug.Log(atime);
		}
		// Even requires at least 3 different atimes.
		if ( group.Count <= 2 ) { return; }

		float interval = (max - min) / (group.Count - 1);
		Debug.Log("Min: " + min.ToString() + " Max: " + max.ToString() + " Interval: " + interval.ToString());
		Debug.Log("After editing");

		var i = 0; var preatime = game.selected[0].atime;
		foreach ( var n in game.selected ) {
			if ( n.atime != preatime ) { i += 1; preatime = n.atime; }
			n.atime = min + i * interval;
			Debug.Log(n.atime);
		}
		Reposition();
	}


	/*
	 * XOXXX  <==>  XXXOX
	 */
	public void MirrorHorizontal() {
		foreach ( var n in game.selected ) {
			n.lane = G.LANES - 1 - n.lane;
		}
		Reposition();
	}


	/*
	 * XXXOX              XOXXX
	 * XXXXX               XXXXX
	 * XOXXX  <==>  XXXOX
	 */
	public void MirrorVertical() {
		float min = game.selected[0].atime;
		float max = game.selected[game.selected.Count - 1].atime;

		foreach ( var n in game.selected ) {
			n.atime = min + (max - n.atime);
		}
		Reposition();
		game.SortN();
	}

	/*
	 * Repeat Pattern
	 * XXXXO
	 * XXXOX
	 * XXOXX
	 * XOXXX
	 * OXXXX
	 */
	public void Pattern1() {
		int[] pattern = new int[] { 0, 1, 2, 3, 4 };
		int i = 0; bool c = true; var preatime = game.selected[0].atime;
		foreach ( var n in game.selected ) {
			if ( n.atime != preatime ) { i += 1; c = true; preatime = n.atime; }
			if ( c ) {
				n.lane = pattern[i % 5];
				c = false;
				Debug.Log(n.lane);
			}
			else {
				Destroy(n.noteobj);
				game.taplist.Remove(n);
			}
		}
		Reposition();
		sc.DeSelect();
	}

	/*
	 * Repeat Pattern
	 * XOXOX
	 * XXOXX
	 * XOXOX
	 * OXXXO
	 */
	public void Pattern2() {

	}

	/*
	 * XOXXX
	 * XXOXX
	 * XXXOX
	 * XXXXO
	 * XXXOX
	 * XXOXX
	 * XOXXX
	 * OXXXX
	 */
	public void Pattern3() {
		int[] pattern = new int[] { 0, 1, 2, 3, 4, 3, 2, 1 };
		int i = 0; bool c = true; var preatime = game.selected[0].atime;
		foreach ( var n in game.selected ) {
			if ( n.atime != preatime ) { i += 1; c = true; preatime = n.atime; }
			if ( c ) {
				n.lane = pattern[i % 8];
				c = false;
				Debug.Log(n.lane);
			}
			else {
				Destroy(n.noteobj);
				game.taplist.Remove(n);
			}
		}
		Reposition();
		sc.DeSelect();
	}

	/*
	 * XOXOX
	 * XXOXO
	 * XOXOX
	 * OXOXX
	 */
	public void Pattern4() {

	}

	/*
	 * XOXOX
	 * OXXXO
	 * XOXOX
	 * XXOXX
	 */
	public void Pattern5() {

	}

	private void Reposition() {
		foreach ( var n in game.selected ) {
			if ( game.taplist.Contains(n) ) { game.ResetNote(n); }
		}
	}
}
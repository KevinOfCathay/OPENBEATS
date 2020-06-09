using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Tap Area
/// </summary>
public class TapArea : MonoBehaviour {
	public int index;
	public Game game;
	public KeyCode keycode;

	private void OnMouseDown() {
		PointerDown();
	}

	private void OnMouseUp() {
		PointerUp();
	}

	public void PointerDown() {
		this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		game.hold[index] = true;

		// Tap event
		// Get the current timecount
		float ct = game.timer;

		int i = 0;

		// If ther are some notes in the lane
		if ( game.active[index].Count != 0 ) {
			Note target_active_note = game.active[index].Peek();

			// If the closest note is clicked (within the bound)
			if ( target_active_note.interval.WithinBound(ct) ) {
				var (points, text) = GetAccuracy(ct, target_active_note.atime);
				game.active[index].Dequeue();
				game.points += points * (game.combo / 10 + 1);
				game.t_points.text = game.points.ToString();

				target_active_note.noteobj.transform.localScale = new Vector3(0f, 0f, 0f);
				target_active_note.noteobj.GetComponent<ParticleSystem>().Stop();
				game.se[index].Play();
				// Debug.Log("Note " + target_active_note.ID.ToString() + " | " + target_active_note.noteobj.transform.localScale.ToString() + " hit at " + ct.ToString() + " Accuracy: " + text);
			}
		}
	}

	public void PointerUp() {
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		game.hold[index] = false;
	}


	/// <summary>
	/// Get which accuracy level it falls into
	/// </summary>
	/// <param name="count"></param>
	/// <returns></returns>
	private (int points, string text) GetAccuracy(float time, float atime) {
		float sub = Math.Abs(time - atime);
		if ( sub <= G.PERFECT_MARGIN ) {
			game.combo += 1;
			return ((int) (G.POINT_BASE / (sub + 1f)), "Perfect");
		}
		else if ( sub <= G.GREAT_MARGIN ) {
			game.combo += 1;
			return ((int) (G.POINT_BASE / (sub + 1f)), "Great");
		}
		else if ( sub <= G.GOOD_MARGIN ) {
			game.combo += 1;
			return ((int) (G.POINT_BASE / (sub + 1f)), "Good");
		}
		else {
			game.combo = 0;
			return (0, "Miss");
		}
	}

}


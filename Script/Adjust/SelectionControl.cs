using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public Game_Adjust game;

	public Image image;
	public GroupEditing ge;

	/// <summary> sy: starting y position.       ey: ending y position </summary>
	public float sy, ey;
	private readonly float width = 550f;
	private readonly float alineoffset = 40f;

	public void OnDrag(PointerEventData eventData) {
		ey = eventData.position.y;
		image.rectTransform.sizeDelta = new Vector2(width, sy - ey);
	}

	public void OnPointerDown(PointerEventData eventData) {
		DeSelect();
		sy = eventData.position.y;
		image.transform.position = new Vector2(75f, eventData.position.y);
		image.rectTransform.sizeDelta = new Vector2(0f, 0f);
		image.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void OnPointerUp(PointerEventData eventData) {

		Debug.Log((ey - 40f).ToString() + " " + (sy - 40f).ToString());

		// Calculate how many notes are selected
		// offset / Note speed = atime offset
		var satime = (ey - alineoffset + game.offset) / G.CRAFT.NOTES_SPEED;
		var eatime = (sy - alineoffset + game.offset) / G.CRAFT.NOTES_SPEED;


		foreach ( var n in game.taplist ) {
			if ( n.atime >= satime && n.atime <= eatime ) {
				game.selected.Add(n);
			}
			if ( n.atime > eatime ) { break; }
		}
		if ( game.selected.Count > 0 ) {
			ge.On();
			// Ascending
			game.selected.Sort(
				(a, b) => { if ( a.atime > b.atime ) { return 1; } else { return -1; } }
				);
			Debug.Log("satime: " + satime.ToString() + " eatime: " + eatime.ToString() + " notes: " + game.selected.Count.ToString());
		}
		else {
			ge.Off();
		}
	}

	public void DeSelect() {
		ge.Off();
		game.selected.Clear();
		image.transform.localScale = new Vector3(0f, 0f, 0f);
	}

}

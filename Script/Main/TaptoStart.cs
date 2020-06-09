using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class TaptoStart : MonoBehaviour, IPointerDownHandler {
	public Game game;

	public void OnPointerDown(PointerEventData eventData) {
		this.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		game.start();
	}

}


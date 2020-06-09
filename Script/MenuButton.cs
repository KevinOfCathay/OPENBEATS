using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	private readonly Vector3 larger = new Vector3(1.1f, 1.1f, 1.1f);
	private readonly Vector3 fullsize = new Vector3(1f, 1f, 1f);


	public void OnPointerEnter(PointerEventData eventData) {
		gameObject.transform.DOScale(larger, 0.4f);
	}

	public void OnPointerExit(PointerEventData eventData) {
		gameObject.transform.DOScale(fullsize, 0.4f);
	}

	public void OnPointerDown(PointerEventData eventData) {
		gameObject.transform.DOScale(fullsize, 0.2f);
	}

	public void OnPointerUp(PointerEventData eventData) {
		gameObject.transform.DOScale(larger, 0.2f);
	}
}

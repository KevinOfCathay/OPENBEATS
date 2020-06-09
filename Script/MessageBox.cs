using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MessageBox : MonoBehaviour, IPointerClickHandler {
	public Text message;
	private readonly Vector3 fullsize = new Vector3(1f, 1f, 1f);
	private readonly Vector3 zero = new Vector3(0f, 0f, 0f);

	public void Show(string message) {
		this.message.text = message;
		DOTween.Sequence()
			    .SetId(gameObject)
			    .Append(
				   gameObject.transform
					  .DOScale(fullsize, 0.5f)
			    )
			    .AppendInterval(3f)
			    .Append(
				   gameObject.transform
					  .DOScale(zero, 0.3f)
			);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		this.gameObject.transform.localScale = zero;
	}
}


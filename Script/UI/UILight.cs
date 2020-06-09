using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class UILight : MonoBehaviour {
	private Vector3 zero = new Vector3(0f, 0f, 0f);
	private Color transparent = new Vector4(1f, 1f, 1f, 0f);
	private Color color = new Vector4(1f, 1f, 1f, 0.4f);

	public void Set(float size, Vector3 position, float interval) {
		gameObject.transform.localScale = zero;
		gameObject.transform.localPosition = position;

		DOTween.Sequence()
	    .SetId(gameObject).AppendInterval(interval)
	    .Append(
		   gameObject.transform
			  .DOScale(size, 3f)
	    ).Append(
		   gameObject.GetComponent<Image>().DOColor(color, 3f)
	    )
	    .Append(
		  gameObject.GetComponent<Image>().DOColor(transparent, 0.5f)
		).Append(
		   gameObject.transform.DOScale(zero, 0.5f)
		).SetLoops(-1);
	}
}

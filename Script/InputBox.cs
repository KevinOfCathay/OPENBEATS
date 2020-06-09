using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InputBox : MonoBehaviour {
	public Text message;
	public InputField input;
	private readonly Vector3 fullsize = new Vector3(1f, 1f, 1f);
	private readonly Vector3 zero = new Vector3(0f, 0f, 0f);

	public void Show(string message, int charlimit = 0, InputField.ContentType contype = InputField.ContentType.Standard) {
		this.message.text = message;
		input.characterLimit = charlimit;
		input.contentType = contype;

		this.gameObject.transform.DOScale(fullsize, 0.5f);
	}

}


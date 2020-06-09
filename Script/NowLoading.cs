using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NowLoading : MonoBehaviour {
	public Scrollbar progressbar;
	private readonly Vector3 fullsize = new Vector3(1f, 1f, 1f);

	public void LoadNextScene(string scenename) {
		gameObject.transform.localScale = fullsize;
		StartCoroutine(IELoadNextScene(scenename));
	}

	private IEnumerator IELoadNextScene(string scenename) {
		AsyncOperation async = SceneManager.LoadSceneAsync(scenename);

		// Wait until the asynchronous scene fully loads
		while ( !async.isDone ) {
			progressbar.size = async.progress;
			yield return null;
		}

		async.allowSceneActivation = true;
		SceneManager.LoadScene(scenename);
	}
}


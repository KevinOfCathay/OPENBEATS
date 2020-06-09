using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_CMusic : MonoBehaviour {
	public GameObject tempitem;
	public GameObject itemgroup;
	public SheetWrapper data;


	public NowLoading loading;
	public MessageBox messagebox;

	public Slider item_slider;
	private Vector3 itemgroup_pos;
	private readonly float itemmargin = 10f;
	// Start is called before the first frame update

	void Awake() {
		// Get the initial item attribute (location,height, width)
		Vector3 anchor_pos = tempitem.transform.localPosition;
		float item_height = tempitem.GetComponent<RectTransform>().rect.height;

		item_slider.maxValue = FileSystem.musicsheet_lib.Count;

		itemgroup_pos = itemgroup.transform.localPosition;
		// Traverse all data in dictionary
		int i = 0;
		foreach ( var item in FileSystem.musicsheet_lib ) {
			var new_item = Instantiate(tempitem);
			new_item.transform.SetParent(itemgroup.transform);

			new_item.transform.localPosition = anchor_pos - new Vector3(0f, (item_height + itemmargin) * i, 0f);

			var summary = item.Value;
			new_item.GetComponent<MusicItem>().path = item.Key;
			new_item.GetComponent<MusicItem>().SetText(summary.title, summary.author, summary.difficulty);
			i += 1;
		}
		Destroy(tempitem);
	}

	private void Start() {
		messagebox.Show(G.lang.message_all_musicsheet_loaded[G.CURLANG] + FileSystem.musicsheet_lib.Count.ToString());
	}

	public void SliderValueChange() {
		itemgroup.transform.localPosition = new Vector3(itemgroup_pos.x, itemgroup_pos.y + item_slider.value * 100.0f, itemgroup_pos.y);
	}

	public void BacktoMenu() {
		Destroy(data.gameObject);
		loading.LoadNextScene("Menu");
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language")]
public class Language : ScriptableObject {
	[TextArea] public string[] message_all_musicsheet_loaded;
	[TextArea] public string[] message_load_musicdata_failure;
	[TextArea] public string[] message_onload_musicdata_error;
	[TextArea] public string[] message_please_fill_required_fields;
	[TextArea] public string[] message_sheet_edit_guides;
}


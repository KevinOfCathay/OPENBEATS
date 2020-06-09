using System;
using System.IO;

using System.Collections.Generic;
using UnityEngine;
using System.Text;

[Serializable]
public class Sheet {
	public SheetSummary summary;

	/*music sheet data*/
	public List<Note> data = new List<Note>();

	public Sheet() {
		summary = new SheetSummary {
			version = G.VERSION,
			createdate = DateTime.Now.ToLongDateString()
		};
	}

	/// <summary> Generate random notes.
	/// </summary>
	/// <param name="total_notes">How many notes are there in this sheet</param>
	public void Random_Init(int total_notes) {
		for ( int i = 0; i < total_notes; i += 1 ) {
			Note n = new Note {
				ID = i,

				// minValue (inclusive), maxValue (exclusive)
				atime = (float) G.rng.Next(0, 60000),
				lane = G.rng.Next(0, G.LANES)
			};
			data.Add(n);
		}
	}

	/// <summary> Transfer the time-based information into the count-based information.
	/// <para>We do this because count interval can be changed.</para>
	/// </summary>
	/// <param name="startline">Not used</param>
	/// <param name="activeline">Not used</param>
	public void Interpret(float startline = 0f, float activeline = 0f) {
		// Desending
		data.Sort((a, b) => { if ( a.atime > b.atime ) { return -1; } else { return 1; } });

		foreach ( var d in data ) {

			// When should this note be spawned. 
			// stime = active atime - time from active line to spawn line
			d.stime = d.atime - G.DISTANSE_SA;

			// Create some intervals
			d.interval = new Interval();
			d.interval.SetBoundary(d.atime - G.MISS_MARGIN, d.atime + G.MISS_MARGIN);

			// active count + from active line to dead line
			d.deadline = d.interval.bound.upper;
		}
	}

	/// <summary> Used for debugging. Printout all [Note] in the data.
	/// </summary>
	public void Printout() {
		foreach ( var d in data ) {
			Debug.Log(d.ToString());
		}
	}

	public SheetSummary GetSummary() {
		return summary;
	}
}

/// <summary> music sheet attributes
/// </summary>
[Serializable]
public class SheetSummary {
	public string title = "";
	public string author = "";
	public int difficulty = 0;
	public string version = "";
	public string aside = "";
	public string createdate = "";
	public float endtime = 0f;
}
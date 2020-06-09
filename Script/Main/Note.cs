using System;
using UnityEngine;


/// <summary>
/// <para>Note</para>
/// <para><c>Parameters: int type, int lane, int atime, int rtime, Interval interval, int deadline</c></para>
/// </summary>
[Serializable]
public class Note {

	public int ID;

	/// <summary> 1 = Single Click Note  2 = Slide Note
	/// </summary>
	public int type;

	/// <summary> Which lane should this note appear.
	/// </summary>
	public int lane;

	/// <summary> Active time. When should this node be active.
	/// <para>Unit: ms</para>
	/// </summary>
	public float atime;

	/// <summary> Spawn time. Calculated based on the note speed. Generalized at run time based on atime.
	/// <para>Unit: ms</para>
	/// </summary>
	[NonSerialized] public float stime = 0f;

	/// <summary> Interval for Good, Bad, Miss etc...  Calculated based on the atime
	/// </summary>
	[NonSerialized] public Interval interval;

	/// <summary> The maximum duration on the screen. If player doesn't click before this deadline, then this will be a miss.
	/// <para>Unit: counts</para>
	/// </summary>
	[NonSerialized] public float deadline = 0f;

	[NonSerialized] public GameObject noteobj = null;

	public override string ToString() {
		return "ID: " + ID.ToString() + " Lane: " + lane.ToString() + " Active time: " + atime.ToString() + " Spawn time: " + stime.ToString() + " Deadline: " + deadline.ToString() + " Intervals " + interval.ToString();
	}
}

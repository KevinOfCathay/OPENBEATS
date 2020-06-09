using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


/// <summary>
/// <para>Interval</para>
/// <para><c>Parameters: List (float lower, float upper, int priority, int points, string text) data</c></para>
/// <para><c>(float lower, float upper) bound</c></para>
/// </summary>
public class Interval {
	public (float lower, float upper) bound;

	public void SetBoundary(float lower, float upper) {
		bound.lower = lower;
		bound.upper = upper;
	}

	/// <summary>
	/// Check if a note is in the interval
	/// </summary>
	public bool WithinBound(float time) {
		return time >= bound.lower && time <= bound.upper;
	}


	public override string ToString() {
		return " Bound: [" + bound.lower.ToString() + "," + bound.upper.ToString() + "]";
	}
}
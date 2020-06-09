using System;
using UnityEngine;

[Serializable]
public class Setting {
	public int language = 0;
	public float musicvolume = 1f;
	public float sevolume = 1f;
	public (int, int) resolution = (1920, 1080);

	/*
		A = 97,
		B = 98,
		C = 99,
		D = 100,
		E = 101,
		F = 102,
		G = 103,
		H = 104,
		I = 105,
		J = 106,
		K = 107,
		L = 108,
		M = 109,
		N = 110,
		O = 111,
		P = 112,
		Q = 113,
		R = 114,
		S = 115,
		T = 116,
		U = 117,
		V = 118,
		W = 119,
		X = 120,
		Y = 121,
		Z = 122
	*/
	public KeyCode[] lanekey = new KeyCode[7] { KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L };
}

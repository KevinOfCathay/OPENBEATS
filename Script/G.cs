using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class G {
	/*  -------------------- SETTINGS -------------------- */
	public static Setting setting = null;
	public static Language lang;
	public static int CURLANG = 0;

	/* -------------------- PATHS ----------------------- */
	public static string DATA_PATH = Application.dataPath;
	public const string FILE_PATH = "music sheet";
	public const string CRAFT_PATH = "craft";
	public const string SETTING_PATH = "setting";

	public static string VERSION = Application.version;

	public static System.Random rng = new System.Random();
	public static SHA256 sha256 = SHA256.Create();


	/* -------------------- GAME ----------------------- */
	/// <summary>
	/// The interval of the timer
	/// <para>Unit: million second</para>
	/// </summary>
	public const float TIMER_INTERVAL = 20.0f;

	/// <summary>
	/// How many lanes are there.
	/// </summary>
	public const int LANES = 5;

	/// <summary>
	/// The speed of the note. 
	/// <para>Unit: pixel per second</para>
	/// </summary>
	public const float NOTE_SPEED = 2f;
	public enum POSITION { Fixed, Random };

	/// <summary>
	/// Interval and judgement related fields
	/// </summary>
	public static float PERFECT_MARGIN = 0.08f;
	public static float GREAT_MARGIN = 0.12f;
	public static float GOOD_MARGIN = 0.16f;
	public static float MISS_MARGIN = 0.2f;

	public const float POINT_BASE = 1000f;

	public const int PERFECT_PENALTY = 0;
	public const int GOOD_PENALTY = 0;
	public const int MISS_PENALTY = 10;

	public static float STARTLINE = 3f;
	public static float ACTIVELINE = -2.2f;
	public static readonly float DISTANSE_SA = (STARTLINE - ACTIVELINE) / G.NOTE_SPEED;

	public enum LANGUAGE { CH, EN, JP };

	public static class CRAFT {
		/// <summary>
		/// How fast notes move in both craft scene and adjust scene.
		/// <para>Unit: position per second</para>
		/// </summary>
		public static float NOTES_SPEED = 300f;
		public static string currentmusic_path = null;

		public readonly static Dictionary<string, AudioType> mtype = new Dictionary<string, AudioType>() {
			{ ".mp3",  AudioType.MPEG}, { ".wav",  AudioType.WAV},{ ".ogg",  AudioType.OGGVORBIS}
		};
	}
}

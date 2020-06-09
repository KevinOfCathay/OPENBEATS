using System;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Collections.Generic;


public static class FileSystem {

	/// <summary> [FIX] Strange and inefficient. This should be fixed in the future.
	/// </summary>
	public sealed class FilePath {
		public string sheetpath = "";
		public string musicpath = "";
		public string coverpath = "";
		public string checksumpath = "";
		public string musictype = "";

		public override string ToString() {
			return "sheetpath: " + sheetpath + " musicpath: " + musicpath + " coverpath: " + coverpath + " checksum path: " + checksumpath + " " + musictype;
		}

		public override int GetHashCode() { return sheetpath.GetHashCode(); }
		public override bool Equals(object obj) { return sheetpath == ((FilePath) obj).sheetpath; }
	}
	// Construct a BinaryFormatter and use it to serialize & de-serialize the data.
	private static BinaryFormatter formatter = new BinaryFormatter();
	public static string sheet_lib_directory = Path.Combine(G.DATA_PATH, G.FILE_PATH);
	public static string craft_temp_directory = Path.Combine(G.DATA_PATH, G.CRAFT_PATH);
	public static string setting_directory = Path.Combine(G.DATA_PATH, G.SETTING_PATH);
	public static string setting_path = Path.Combine(setting_directory, "setting");

	/// <summary>
	/// Path : SheetSummary
	/// </summary>
	public static Dictionary<FilePath, SheetSummary> musicsheet_lib = new Dictionary<FilePath, SheetSummary>();

	public static Sheet loadedsheet = new Sheet();

	public static void Save_MusicSheet(Sheet savedsheet, string foldername) {
		var sheet_collection_path = Path.Combine(sheet_lib_directory, foldername);
		var musicsheet_path = Path.Combine(sheet_collection_path, "musicsheet.st");
		var checksum_path = Path.Combine(sheet_collection_path, "checksum.sha");

		if ( !Directory.Exists(sheet_lib_directory) ) {
			Directory.CreateDirectory(sheet_lib_directory);
		}

		// There shouldn't be any folder with the same name
		if ( !Directory.Exists(sheet_collection_path) ) {
			Directory.CreateDirectory(sheet_collection_path);

			FileStream fstream = new FileStream(musicsheet_path, FileMode.Create);

			try {
				formatter.Serialize(fstream, savedsheet);
			} catch ( Exception e ) {
				Debug.Log(e.Message);
			} finally {
				fstream.Close();
			}

			try {
				fstream = File.OpenRead(musicsheet_path);
				var hashstring = G.sha256.ComputeHash(fstream);
				fstream.Close();

				fstream = new FileStream(checksum_path, FileMode.Create);
				fstream.Write(hashstring, 0, hashstring.Length);
				fstream.Close();

				// Copy the music file to the destination
				if ( File.Exists(G.CRAFT.currentmusic_path) ) {
					File.Copy(G.CRAFT.currentmusic_path,
						Path.Combine(sheet_collection_path, Path.GetFileName(G.CRAFT.currentmusic_path)));
				}
			} catch ( Exception e ) {
				Debug.Log(e.Message);
			}
		}
	}

	/// <summary>
	/// Load all sheets in the folder into our hashset
	/// </summary>
	public static void Load_AllSheets() {
		// If there is no save folder (which means there is no sheet available)
		if ( !Directory.Exists(sheet_lib_directory) ) {
			Directory.CreateDirectory(sheet_lib_directory);
		}
		else {
			// Get all the folders the global music sheet save path
			var flist = Directory.GetDirectories(sheet_lib_directory);

			// Each folder represents a single music sheet
			foreach ( var folder in flist ) {

				Debug.Log(folder);
				var files = Directory.GetFiles(folder);

				var path = new FilePath();
				foreach ( var f in files ) {
					Debug.Log(f);

					// We get the file based on the file extension
					var extension = Path.GetExtension(f);
					if ( extension == ".st" ) {
						path.sheetpath = f;
					}
					else if ( extension == ".sha" ) {
						path.checksumpath = f;
					}
					else if ( extension == ".mp3" || extension == ".ogg" || extension == ".wav" ) {
						path.musicpath = f;
						path.musictype = extension;
					}
					else if ( extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" || extension == ".png" ) {
						path.coverpath = f;
					}
				}

				Debug.Log(path.ToString());

				if ( path.sheetpath == "" || path.checksumpath == "" || path.musicpath == "" ) {
					continue;
				}

				try {
					// We perform a checksum on our data
					FileStream fstream = File.OpenRead(path.sheetpath);
					var hashstring = Encoding.ASCII.GetString(G.sha256.ComputeHash(fstream));
					var checksumhash = Encoding.ASCII.GetString(File.ReadAllBytes(path.checksumpath));
					if ( hashstring == checksumhash ) {
						FileStream nfstream = File.OpenRead(path.sheetpath);
						Sheet tempsheet = new Sheet();
						tempsheet = (Sheet) formatter.Deserialize(nfstream);
						musicsheet_lib.Add(path, tempsheet.GetSummary());
						Debug.Log("File check passed");
						nfstream.Close();
					}
					fstream.Close();
					Debug.Log("Sheet added to the list");
				} catch ( SerializationException e ) {
					Debug.Log(e.Message);
				}
			}
		}
	}

	public static Sheet Load_MusicSheet(string path) {
		loadedsheet = new Sheet();
		try {
			FileStream fstream = new FileStream(path, FileMode.Open);

			// Deserialize the hashtable from the file and
			// assign the reference to the local variable.
			loadedsheet = (Sheet) formatter.Deserialize(fstream);
			fstream.Close();
		} catch ( SerializationException e ) {
			Debug.Log(e.Message);
			return null;
		}
		return loadedsheet;
	}

	public static void LoadSetting() {
		// First we check if we have our setting file
		// If we can't find the file
		if ( !File.Exists(setting_path) ) {
			if ( !Directory.Exists(setting_directory) ) {
				Directory.CreateDirectory(setting_directory);
			}
			Setting setting = new Setting();
			G.setting = setting;
			FileStream fstream = new FileStream(setting_path, FileMode.Create);
			try {
				formatter.Serialize(fstream, setting);
				Debug.Log("No directory found. Create default settings");
			} catch ( Exception e ) {
				Debug.Log(e.Message);
			} finally {
				fstream.Close();
			}
		}
		else {
			FileStream fstream = new FileStream(setting_path, FileMode.Open);
			try {
				G.setting = (Setting) formatter.Deserialize(fstream);
				Debug.Log("Settings loaded");
			} catch ( Exception e ) {
				Debug.Log(e.Message);
			} finally {
				fstream.Close();
			}
		}
	}
	public static void SaveSetting() {
		if ( !Directory.Exists(setting_directory) ) {
			Directory.CreateDirectory(setting_directory);
		}
		FileStream fstream = new FileStream(setting_path, FileMode.Create);
		try {
			formatter.Serialize(fstream, G.setting);
		} catch ( Exception e ) {
			Debug.Log(e.Message);
		} finally {
			fstream.Close();
		}
	}

	public static List<(string musicpath, string musicname, string extension)> LoadCraftTempMusic() {
		if ( !Directory.Exists(craft_temp_directory) ) {
			Directory.CreateDirectory(craft_temp_directory);
			return null;
		}
		else {
			var files = Directory.GetFiles(craft_temp_directory);
			List<(string musicpath, string musicname, string extension)> result = new List<(string musicpath, string musicname, string extension)>(files.Length);
			foreach ( var file in files ) {
				Debug.Log("Craft temp music: " + file);
				var extension = Path.GetExtension(file);
				if ( extension == ".ogg" || extension == ".wav" ) {
					result.Add((file, Path.GetFileName(file), extension));
				}
			}
			return result;
		}
	}
}
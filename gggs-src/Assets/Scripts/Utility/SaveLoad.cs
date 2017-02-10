using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
	
	public static SaveLoad saveLoad;
	
	private int highScore { get; set; }
	private int cumulativeScore { get; set; }

	private List<string> itemsOwned { get; set; }
	
	private void Awake() {
		if (saveLoad == null) {
			DontDestroyOnLoad(gameObject);
			saveLoad = this;
		}
		else if (saveLoad != this) {
			Destroy(gameObject);
		}
	}
	
	private void OnEnable() {
		// autoload
		Load();
	}

	private void OnDisable() {
		// autosave
		Save();
	}
	
	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

		highScore = DataManager.HighScore;
		cumulativeScore = DataManager.CumulativeScore;
		
		PlayerData data = new PlayerData();
		data.highScore = highScore;
		data.cumulativeScore = cumulativeScore;
		
		bf.Serialize(file, data);
		file.Close();	
	}
	
	public void Load() {
		if (File.Exists(Application.persistentDataPath + "/save.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			cumulativeScore = data.cumulativeScore;
			DataManager.CumulativeScore = cumulativeScore;

			highScore = data.highScore;
			DataManager.HighScore = highScore;
		}
	}
}

[Serializable]
class PlayerData {
	public int highScore;
	public int cumulativeScore;

	public List<string> itemsOwned;
}

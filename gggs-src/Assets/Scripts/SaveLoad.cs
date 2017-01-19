using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
	
	public static SaveLoad saveLoad;
	
	public int highScore { get; set; }
	
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
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

		highScore = DataManager.HighScore;
		
		PlayerData data = new PlayerData();
		data.highScore = highScore;
		
		bf.Serialize(file, data);
		file.Close();	
	}
	
	public void Load() {
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();
			
			highScore = data.highScore;
			DataManager.HighScore = highScore;
		}
	}
}

[Serializable]
class PlayerData {
	public int highScore;
}
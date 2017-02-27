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

	private List<LevelData> levelDataList { get; set; }
	
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
		levelDataList = DataManager.LevelDataList;
		
		PlayerData data = new PlayerData();
		data.highScore = highScore;
		data.cumulativeScore = cumulativeScore;
		data.levelDataList = levelDataList;
		
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

			levelDataList = data.levelDataList;
			DataManager.LevelDataList = levelDataList;

		}
	}
}

[Serializable]
class PlayerData {
	public int highScore;
	public int cumulativeScore;

	public List<HighScoreData> highScoreList;
	public List<LevelData> levelDataList;

	// @CONTINUE: this needs to save a different HighScoreData object for 
	//            every level. so maybe a dictionary of key value pairs
	//            <string, List<HighScoreData>> where string is the level
	//            name? alternatively, nested list where the exterior index
	//            is the same as the level build index. could cause issues though
}

[Serializable]
public class LevelData {
	public string levelName;
	public List<HighScoreData> highScores;

	public LevelData(string _levelName, List<HighScoreData> _highScores) {
		levelName = _levelName;
		highScores = _highScores;
	}
}

[Serializable]
public class HighScoreData {
	public string name;
	public int score;

	public HighScoreData(string _name, int _score) {
		name = _name;
		score = _score;
	}
}

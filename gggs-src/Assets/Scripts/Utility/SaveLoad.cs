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
	private string lastEnteredName { get; set; }

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
		lastEnteredName = DataManager.LastEnteredHighScoreName;
		levelDataList = DataManager.LevelDataList;
		
		PlayerData data = new PlayerData();
		data.highScore = highScore;
		data.cumulativeScore = cumulativeScore;
		data.lastEnteredName = lastEnteredName;
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

			highScore = data.highScore;
			DataManager.HighScore = highScore;

			cumulativeScore = data.cumulativeScore;
			DataManager.CumulativeScore = cumulativeScore;

			lastEnteredName = data.lastEnteredName;
			DataManager.LastEnteredHighScoreName = lastEnteredName;

			levelDataList = data.levelDataList;
			DataManager.LevelDataList = levelDataList;

		}

		if (File.Exists(Application.persistentDataPath + "/Data/score-spreadsheet.csv")) {
			StreamReader file = new StreamReader(Application.persistentDataPath + "/Data/score-spreadsheet.csv");

			string line = "";
			string[] row = new string [3];


			while ((line = file.ReadLine()) != null) {
				row = line.Split(',');

				int j, k;

				if (Int32.TryParse(row[1], out j) && Int32.TryParse(row[2], out k)) {
					ObjectData data = new ObjectData(row[0], j, k);
					DataManager.ObjectProperties.Add(data);
				}

			}
		}
	}
}

[Serializable]
class PlayerData {
	public int highScore;
	public int cumulativeScore;

	public string lastEnteredName;

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

[Serializable]
public class ObjectData {
  public string name;
  public int mass;
  public int points;

  public ObjectData(string _name, int _mass, int _points) {
  	name = _name;
		mass = _mass;
		points = _points;
  }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager {

  private static int score = 0;
  private static int highScore = 0;

  public static int Score { get { return score; } set { score = value; } }
  public static int HighScore { get { return highScore; } set { highScore = value; } }
  public static List<LevelData> levels { get; private set; }

  public static void UpdateHighScore() {
    HighScore = Score;
  }

  public static void SetLevelData() {



    
  }





  public static void Save() {

  }

  public static void Load() {

  }


}

public class LevelData {

  public string levelName;
  public int scoreGoal;

}
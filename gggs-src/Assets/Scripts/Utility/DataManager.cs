using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager {

  private static int score = 0;
  private static int highScore = 0;

  public static int Score {
    get {
      return score;
    }
    set {
      score = value;
      if (score > highScore) highScore = score;
    }
  }


  public static Weapon SelectedWeapon { get; set; }

  public static int HighScore { get { return highScore; } set { highScore = value; } }
  public static bool NewHighScore { get; set; }
  public static int CumulativeScore { get; set; }

  public static int Combo { get; set; }

  public static bool AllowControl { get; set; }
  public static bool Paused { get; set; }
  public static bool GameOver { get; set; }
  public static bool Grounded { get; set; }

  public static bool ObjectIsStillMoving { get; set; }
  public static float ObjectMovementThreshold { get; set; }


  public static List<LevelData> levels { get; private set; }

  public static void UpdateHighScore() {
    HighScore = Score;
  }

  public static void ResetHighScore() {
    HighScore = 0;
  }

}

public class LevelData {

  public string levelName;
  public int scoreGoal;

}

public enum Weapon {
  Ball,
  Cannon
}

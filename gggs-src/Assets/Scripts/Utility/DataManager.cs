﻿using System.Collections;
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
      // if (score > highScore) highScore = score;
    }
  }

  public static Vector3 StartingPosition { get; set; }
  public static Quaternion StartingRotation { get; set; }

  public static int HighScore { get { return highScore; } set { highScore = value; } }
  public static bool NewHighScore { get; set; }
  public static int CumulativeScore { get; set; }
  public static int ScoreGoal { get; set; }
  public static int BonusScoreGoal { get; set; }

  public static string LastEnteredHighScoreName { get; set; }

  public static bool AllowControl { get; set; }
  public static bool Grounded { get; set; }
  public static bool Paused { get; set; }
  public static bool GameOver { get; set; }

  public static List<string> ObjectsScoredList { get; set; }

  public static List<ObjectData> ObjectProperties { get; set; }

  public static List<LevelData> LevelDataList { get; set; }

  public static List<HighScoreData> HighScoreList { get; set; }

  public static void UpdateHighScore() {
    HighScore = Score;
  }

  public static void ResetHighScore() {
    HighScore = 0;
  }

}


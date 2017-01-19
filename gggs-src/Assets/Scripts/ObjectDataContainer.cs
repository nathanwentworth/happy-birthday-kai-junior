using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataContainer : MonoBehaviour {

  [SerializeField]
  private int objectPoints;

  public int ObjectPoints {
    get { return objectPoints; }
  }

}

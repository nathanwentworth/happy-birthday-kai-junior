using UnityEngine;

public class ObjectDataContainer : MonoBehaviour {

  [SerializeField]
  private int objectPoints;

  public int ObjectPoints {
    get { return objectPoints; }
  }

}

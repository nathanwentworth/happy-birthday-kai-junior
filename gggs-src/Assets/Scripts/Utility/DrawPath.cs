using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour {

  [SerializeField]
  private Color lineColor;

  private void OnDrawGizmos() {
    Gizmos.color = lineColor;

    List<Transform> nodes = new List<Transform>();
    foreach (Transform child in transform) {
      nodes.Add(child);
    }

    for (int i = 0; i < nodes.Count; i++) {
      Vector3 currentNode = nodes[i].position;
      Vector3 lastNode = Vector3.zero;

      if (i > 0) {
        lastNode = nodes[i - 1].position;
      } else if (i == 0 && nodes.Count > 1) {
        lastNode = nodes[nodes.Count - 1].position;
      }

      Gizmos.DrawLine(lastNode, currentNode);
      Gizmos.DrawWireSphere(currentNode, 0.5f);
    }

  }


}

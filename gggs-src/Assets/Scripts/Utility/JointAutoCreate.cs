using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointAutoCreate : MonoBehaviour {

  private void Start() {
    GameObject obj = gameObject;
    CreateJoints(obj);
  }

  [ContextMenu ("Create Joints")]
  public void CreateJoints(GameObject parentObj) {
    Transform transform = parentObj.GetComponent<Transform>();

    List<Collider[]> colliders = new List<Collider[]>();

    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x + 1, transform.position.y, transform.position.z),
      transform.localScale)
    );
    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x - 1, transform.position.y, transform.position.z),
      transform.localScale)
    );
    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
      transform.localScale)
    );
    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x, transform.position.y - 1, transform.position.z),
      transform.localScale)
    );
    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x, transform.position.y, transform.position.z + 1),
      transform.localScale)
    );
    colliders.Add(Physics.OverlapBox(
      new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
      transform.localScale)
    );

    for (int i = 0; i < colliders.Count; i++) {
      for (int j = 0; j < colliders[i].Length; j++) {
        GameObject obj = colliders[i][j].gameObject;

        if (obj.name != "Floor") {
          if (obj.GetComponent<FixedJoint>() == null) {
            obj.AddComponent<FixedJoint>();
            Debug.Log("Fixed joint added to " + obj.gameObject.name);
          } else {
            Debug.Log("Fixed joint already on " + obj.gameObject.name);
          }

          obj.GetComponent<FixedJoint>().connectedBody = parentObj.GetComponent<Rigidbody>();

          CreateJoints(obj);

          Debug.Log(obj.gameObject.name);
        }
      }
    }

    Debug.Log("create joints called");
  }
}

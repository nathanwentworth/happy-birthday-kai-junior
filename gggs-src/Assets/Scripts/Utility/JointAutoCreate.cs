using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointAutoCreate : MonoBehaviour {

  public ContactPoint[] Contacts { get; private set; }
  private bool contactsChecked;
  [SerializeField]
  private bool root;
  [SerializeField]
  private float breakForce;

  private void Start() {
    GameObject obj = gameObject;
    if (root) {
      StartCoroutine(CheckCollisions());
    }
  }

  [ContextMenu ("Create Joints")]
  public void CreateJoints(GameObject parentObj) {
    Transform transform = parentObj.GetComponent<Transform>();
    JointAutoCreate jointAutoCreate = null;
    if ((jointAutoCreate = parentObj.GetComponent<JointAutoCreate>()) != null) {
      foreach (ContactPoint contact in jointAutoCreate.Contacts) {
        GameObject obj = contact.otherCollider.gameObject;

        Debug.Log("object checking: " + gameObject.name + ", object being checked: " + obj.name);
        Debug.DrawLine(contact.point, contact.point + contact.normal, Color.green, 5, false);

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
        } else {
          Debug.Log("The object being checked is the floor");
        }

      }
      
    }


    Debug.Log("create joints called");
  }


  private void OnCollisionEnter(Collision other) {
    if (!contactsChecked) {
      Contacts = other.contacts;
      contactsChecked = true;
      Debug.Log("contactsChecked on " + gameObject.name);
    }
  }

  private IEnumerator CheckCollisions() {
    GameObject obj = gameObject;

    while (!contactsChecked) {
      yield return new WaitForEndOfFrame();
    }

    CreateJoints(obj);
    yield return null;
  }
}

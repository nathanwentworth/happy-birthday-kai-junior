using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviors : MonoBehaviour {

  // object components

  private Rigidbody rb;

  [Header("Add/Activate Rigidbody on Collision")]

  [SerializeField]
  [Tooltip("When collided with something >= hitforce, adds or activates an attached rigidbody")]
  private bool rbOnCollision;
  [SerializeField]
  private float hitForce;

  [Header("Other Object Active Switch")]

  [SerializeField]
  [Tooltip("When this trigger is entered, toggle the active state of some other objects in the scene")]
  private bool objActiveSwitch;
  [SerializeField]
  private GameObject[] objectsToToggle;

  [Header("Object Swap")]

  [SerializeField]
  [Tooltip("When collided with, swap out this model for another specified model")]
  private bool objectSwap;
  [SerializeField]
  private GameObject objectToSwapPrefab;
  private GameObject objectToSwapScene;

  [Header("Kill Box")]

  [SerializeField]
  [Tooltip("When entered, reset the player and set non-player objects to inactive")]
  private bool killBox;

  [Header("Timed Active Toggle")]

  [SerializeField]
  [Tooltip("Automatically toggle active state on objects on a timer")]
  private bool timedToggle;

  [SerializeField]
  private float onTime;
  [SerializeField]
  private float offTime;
  [SerializeField]
  private float waitDelay;

  private MeshRenderer meshToToggle;
  private Collider[] timedToggleColliders;

  [Header("Speed Boost")]

  [SerializeField]
  [Tooltip("Add force to the object when entered")]
  private bool speedBoost;

  [SerializeField]
  private Vector3 boostDirection;
  [SerializeField]
  private float boostForce;






  // functions

  private void Awake() {
    if (objectSwap) {
      ObjectSwapInit();
    }
  }

	private void Start() {
    if (rbOnCollision) {
      RigidbodyActivateOnCollisionInit();
    }
    if (timedToggle) {
      TimedToggleInit();
    }
    if (objActiveSwitch || objectSwap || killBox || speedBoost) {
      if (GetComponent<Collider>() == null) {
        gameObject.AddComponent<Collider>();
      }
      GetComponent<Collider>().isTrigger = true;
    }
		
	}

  private void OnTriggerEnter(Collider other) {
    if (objActiveSwitch) {
      ObjectActiveSwitchRun(other);
    }
    if (objectSwap) {
      ObjectSwapRun(other);
    }
    if (killBox) {
      KillBoxRun(other);
    }
    if (speedBoost) {
      SpeedBoostRun(other);
    }
  }


  private void OnCollisionEnter(Collision other) {
    if (rbOnCollision) {
      RigidbodyActivateOnCollisionRun(other);
    }
  }

  private void RigidbodyActivateOnCollisionInit() {
    if (GetComponent<Rigidbody>() == null) {
      rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
    } else {
      rb = GetComponent<Rigidbody>();
    }
    rb.isKinematic = true;

    if (GetComponent<Collider>() == null) {
      gameObject.AddComponent<Collider>();
    }
    GetComponent<Collider>().isTrigger = false;

  }

  private void RigidbodyActivateOnCollisionRun(Collision other) {
    if (other.gameObject.tag == "Player") {
      if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > hitForce) {
        if (rb.isKinematic) {
          rb.isKinematic = false;
        }
      }
    }    
  }

  private void ObjectActiveSwitchRun(Collider other) {
    if (other.gameObject.tag == "Player") {
      for (int i = 0; i < objectsToToggle.Length; i++) {
        objectsToToggle[i].SetActive(false);
      }
    }
  }

  private void ObjectSwapInit() {
    objectToSwapScene = Instantiate(objectToSwapPrefab, transform.position, transform.rotation);
    objectToSwapScene.SetActive(false);
  }

  private void ObjectSwapRun(Collider other) {
    if (other.gameObject.GetComponent<Rigidbody>() != null) {
      if (other.gameObject.tag == "Player" || other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 2) {
        gameObject.SetActive(false);
        objectToSwapScene.SetActive(true);
        Debug.Log("Swapped " + gameObject.name + " for " + objectToSwapScene.name);
      } else {
        Debug.Log("didn't swap, other's tag is " + other.gameObject.tag + " and its velocity is " + other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
      }
    } else {
      Debug.Log(other.gameObject.name + " doesn't have a Rigidbody attached");
    }
  }

  private void KillBoxRun(Collider other) {
    if (other.gameObject.tag == "Player") {

      Vector3 respawnPoint = DataManager.StartingPosition;
      Quaternion respawnDirection = DataManager.StartingRotation;
      other.transform.position = respawnPoint;
      other.transform.rotation = respawnDirection;
      other.GetComponent<Rigidbody>().velocity = Vector3.zero;

    } else {
      other.gameObject.SetActive(false);
    }
  }

  private void TimedToggleInit() {
    timedToggleColliders = GetComponents<Collider>();
    meshToToggle = GetComponent<MeshRenderer>();

    StartCoroutine(TimedToggleTimerStart());
  }

  private IEnumerator TimedToggleTimerStart() {
    yield return new WaitForSeconds(waitDelay);
    StartCoroutine(TimedToggleRun(true));
  }

  private IEnumerator TimedToggleRun(bool onT) {
    float t = (onT) ? onTime : offTime;

    meshToToggle.enabled = onT;
    foreach (Collider c in timedToggleColliders) {
      c.enabled = onT;
    }

    while (t > 0) {
      t -= Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }

    StartCoroutine(TimedToggleRun(!onT));
  }

  private void SpeedBoostRun(Collider other) {
    Rigidbody otherRb = null;
    if ((otherRb = other.gameObject.GetComponent<Rigidbody>()) != null) {

      boostDirection = (boostDirection == Vector3.zero) ? new Vector3(0, 1, 0) : boostDirection;

      otherRb.AddForce((transform.rotation * boostDirection) * boostForce, ForceMode.VelocityChange);
    }
  }






}

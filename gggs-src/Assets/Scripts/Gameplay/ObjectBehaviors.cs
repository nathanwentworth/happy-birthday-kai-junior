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

  [Header("Other Object Anim in Zone")]

  [SerializeField]
  [Tooltip("When this trigger is entered, play an animation on another object")]
  private bool objAnimZone;
  [SerializeField]
  private GameObject[] objectsToAnim;
  [SerializeField]
  [Tooltip("Must be a bool!")]
  private string animToToggle;

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

  [Header("Random Spawn Prefabs In Area")]

  [SerializeField]
  private bool randomSpawn;
  [SerializeField]
  private float checkRadius;
  [SerializeField]
  private float numberOfCitizensToSpawn;
  [SerializeField]
  private GameObject[] citizens;
  private System.Random rnd;
  int checks = 0;


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
    if (objActiveSwitch || objAnimZone || objectSwap || killBox || speedBoost) {
      if (GetComponent<Collider>() == null) {
        gameObject.AddComponent<Collider>();
      }
      GetComponent<Collider>().isTrigger = true;
    }
    if (randomSpawn) {
      Pool();
    }

	}

  private void OnTriggerEnter(Collider other) {
    if (objActiveSwitch) {
      ObjectActiveSwitchRun(other);
    }
    if (objAnimZone) {
      ObjectAnimRun(other, true);
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

  private void OnTriggerExit(Collider other) {
    if (objAnimZone) {
      ObjectAnimRun(other, false);
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

  private void ObjectAnimRun(Collider other, bool state) {
    if (other.gameObject.tag == "Player") {
      for (int i = 0; i < objectsToAnim.Length; i++) {
        Animator anim = null;
        if ((anim = objectsToAnim[i].GetComponent<Animator>()) != null) {
          anim.SetBool(animToToggle, state);
        }
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

  private void Pool() {
    Collider collider = null;
    if ((collider = GetComponent<Collider>()) == null) {
      Debug.Log("Random spawn in area requires a collider!");
      return;
    }

    collider.isTrigger = true;

    rnd = new System.Random();
    Debug.Log("Spawning " + numberOfCitizensToSpawn + " citizens");
    for (int i = 0; i < numberOfCitizensToSpawn; i++) {
      Spawn();
    }
  }

  private void Spawn() {
    Vector3 rndPosWithin;
    rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);

    Vector3 rndRotation = new Vector3(transform.rotation.x, Random.Range(0, 360), transform.rotation.z);
    if (checkRadius == 0 || !Physics.CheckSphere(rndPosWithin, checkRadius)) {
      Instantiate(citizens[rnd.Next(citizens.Length)], rndPosWithin, Quaternion.Euler(rndRotation));
      checks = 0;
    } else if (checks < 10) {
      checks++;
      Debug.Log("Object overlapping, checking again: " + checks);
      Spawn();
    } else {
      Debug.Log("Maxed out checks, not running function anymore");
    }
  }


}

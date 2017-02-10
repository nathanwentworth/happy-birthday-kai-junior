using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

  [SerializeField]
  private float explosionForce;
  [SerializeField]
  private float explosionRadius;

  private void OnEnable() {
    StartCoroutine(Explode());
  }


  private IEnumerator Explode() {
    yield return new WaitForSeconds(3);
    Debug.Log("boom!");
    Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
    for (int i = 0; i < colliders.Length; i++) {
      if (colliders[i].GetComponent<Rigidbody>() != null) {
        colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Impulse);
      }
    }
  }
	
}

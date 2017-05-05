using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLeavesParticles : MonoBehaviour {

	private List<GameObject> treeParticles = new List<GameObject>();

	[SerializeField]
	private int particlesToSpawn = 10;
	[SerializeField]
	private GameObject particlePrefab;


	private void Start () {
		if (particlePrefab == null) {
			Debug.LogWarning ("You need to attach a prefab to the TreeLeavesParticles script!");
			return;
		}

		for (int i = 0; i < particlesToSpawn; i++) {
			GameObject obj = (GameObject)Instantiate (particlePrefab);
			obj.SetActive (false);
			treeParticles.Add (obj);
		}
	}
	

	private void OnCollisionEnter(Collision other) {
		if (particlePrefab == null) { return; }
		foreach (GameObject obj in treeParticles) {
			if (!obj.activeInHierarchy) {
				obj.transform.position = other.contacts[0].point;
				obj.SetActive (true);
				obj.GetComponent<ParticleSystem>().Play();
			}
		}
	}
}

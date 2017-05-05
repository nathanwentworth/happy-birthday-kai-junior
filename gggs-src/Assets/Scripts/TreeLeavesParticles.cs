using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLeavesParticles : MonoBehaviour {

	private List<GameObject> treeParticles = new List<GameObject>();
	private List<ParticleSystem> treeParticleSystems = new List<ParticleSystem> ();

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
			treeParticleSystems.Add(obj.GetComponent<ParticleSystem>());
			obj.SetActive (false);
			treeParticles.Add (obj);
		}
	}

	private void Update() {
		for (int i = 0; i < particlesToSpawn; i++) {
			if (!treeParticleSystems[i].isPlaying) {
				treeParticles [i].SetActive (false);
			}
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (particlePrefab == null) { return; }
		if (other.gameObject.tag == "tree") {
			for (int i = 0; i < particlesToSpawn; i++) {
				if (!treeParticles[i].activeInHierarchy) {
					treeParticles[i].transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 1.5f, other.transform.position.z);
					treeParticles[i].SetActive (true);
					treeParticleSystems[i].Play();
					break;
				}
			}
		
		}
	}
}

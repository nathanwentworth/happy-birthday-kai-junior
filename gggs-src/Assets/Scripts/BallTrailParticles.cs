using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailParticles : MonoBehaviour {

	[SerializeField]
	private GameObject particleTrailParent;
	private List <ParticleSystem> particleList = new List<ParticleSystem>();
	private List <string> particleNames = new List<string>();

	private void Start () {
		foreach (Transform child in particleTrailParent.transform) {
			particleList.Add (child.GetComponent<ParticleSystem>());
			string name = child.gameObject.name;
			name = name.Replace ("particle-trail-", "");
			particleNames.Add (name);
		}

	}

	private void FixedUpdate () {
		particleTrailParent.transform.position = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);

		RaycastHit hit;

		if (Physics.Raycast (transform.position, -Vector3.up, out hit, 3f)) {

			for (int i = 0; i < particleNames.Count; i++) {
				if (particleNames [i] == hit.transform.gameObject.tag) {
					particleList [i].Play ();
				} else {
					particleList [i].Stop ();
				}
			}

		} else {
			for (int i = 0; i < particleNames.Count; i++) {
				particleList [i].Stop ();
			}

		}

	}

}

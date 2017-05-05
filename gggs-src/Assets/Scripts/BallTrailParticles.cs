using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailParticles : MonoBehaviour {

	[SerializeField]
	private GameObject particleTrailParent;
	private List <Transform> particleList = new List<Transform>();
	private List <string> particleNames = new List<string>();


	private void Start () {
		foreach (Transform child in particleTrailParent.transform)
		{
			particleList.Add (child);
			string name = child.gameObject.name;
			name = name.Replace ("particle-trail-", "");
			particleNames.Add (name);
		}
	}

	private void FixedUpdate () {
		particleTrailParent.transform.position = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);

		RaycastHit hit;

		if (Physics.Raycast (transform.position, -Vector3.up, out hit)) {

			for (int i = 0; i < particleNames.Count; i++) {
				if (particleNames [i] == hit.transform.gameObject.tag) {
					particleList [i].gameObject.SetActive (true);
				} else {
					particleList [i].gameObject.SetActive (false);
				}
			}
			
		}
			

		
	}
}

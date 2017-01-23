using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepFlat : MonoBehaviour {
	
	private void FixedUpdate () {
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * 100);
	}

}

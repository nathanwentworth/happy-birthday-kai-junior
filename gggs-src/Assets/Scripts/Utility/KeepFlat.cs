using System.Collections;
using UnityEngine;

public class KeepFlat : MonoBehaviour {
	
	private void FixedUpdate () {
    Quaternion rot = Quaternion.Euler(0, transform.rotation.y, 0);
		transform.rotation = rot;
	}

}

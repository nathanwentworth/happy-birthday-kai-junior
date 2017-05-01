using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUVScroll : MonoBehaviour {

  [SerializeField]
  private float scrollSpeed = 0.5f;
  private Renderer rend;
  [SerializeField]
  private string materialName;

  private	void Start () {
		rend = GetComponent<Renderer>();
	}

	private void Update () {
		float offset = Time.time * scrollSpeed;
    rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));

	}
}

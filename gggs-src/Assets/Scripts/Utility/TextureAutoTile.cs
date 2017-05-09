using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextureAutoTile : MonoBehaviour {

  [SerializeField]
  private float tilingModifier = 0.1f;
  private Renderer rend;

	private void Start () {
    rend = GetComponent<Renderer>();
    rend.sharedMaterial.SetTextureScale("_MainTex", new Vector2(transform.localScale.x * tilingModifier, transform.localScale.y * tilingModifier));
	}

  #if UNITY_EDITOR

  private void Update() {
    if (rend == null) {
      rend = GetComponent<Renderer>();
    }
    rend.sharedMaterial.SetTextureScale("_MainTex", new Vector2(transform.localScale.x * tilingModifier, transform.localScale.y * tilingModifier));
  }

  #endif

}

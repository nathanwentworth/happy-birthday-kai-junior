using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterUVScroll : MonoBehaviour {

  [SerializeField]
  private float scrollSpeedX = 0.6f;
  [SerializeField]
  private float scrollLengthX = 0.1f;
  [SerializeField]
  private float scrollSpeedY = 0.6f;
  [SerializeField]
  private float scrollLengthY = 0.1f;
  [SerializeField]
  private float tilingModifier = 0.1f;
  private Renderer rend;
  private float rand;

  private	void Start () {
		rend = GetComponent<Renderer>();
    rand = Random.value;

    rend.sharedMaterial.SetTextureScale("_MainTex", new Vector2(transform.localScale.x * tilingModifier, transform.localScale.y * tilingModifier));
	}

	private void Update () {
    float offsetX = scrollSpeedX * Mathf.Sin(Time.time * 2 * Mathf.PI * scrollLengthX);
		float offsetY = scrollSpeedY * (Mathf.Sin(Time.time * 2 * Mathf.PI * scrollLengthY) + rand);
    rend.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));

	}
}

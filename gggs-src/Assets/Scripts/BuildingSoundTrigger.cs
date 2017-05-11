using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSoundTrigger : MonoBehaviour {

  private AudioSource audio;
  [SerializeField]
  private AudioClip[] clips;

	private void Start () {
    audio = GetParentObject().GetComponent<AudioSource>();
	}

  private int GetObjectDepth() {
    int parentIndex = 0;

    Transform t = gameObject.transform;
    while (t.parent != null) {
      parentIndex++;
      t = t.parent.transform;
    }

    return parentIndex - 1;
  }

  private GameObject GetParentObject() {
    Transform t = gameObject.transform;
    for (int i = 0; i < GetObjectDepth(); i++) {
      t = t.parent.transform;
    }

    return t.gameObject;
  }

  private void OnCollisionEnter(Collision other) {
    if (audio == null) { return; }

    if (other.gameObject.tag == "Player") {
      StartCoroutine(PlayAudio());
    }
  }

  private IEnumerator PlayAudio() {
    if (clips.Length > 0) {
      audio.clip = clips[Random.Range(0, clips.Length)];
    }

    audio.Play();
    Debug.Log("Audio clip " + audio.clip.name + " played");
    yield return null;
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextPopup : MonoBehaviour {
	
  private ObjectDataContainer data;
  private int points;

  private TextMeshPro m_textMeshPro;
  private TextMesh m_textMesh;

  private Transform m_transform;
  private Transform m_floatingText_Transform;
  private Transform m_cameraTransform;

  Vector3 lastPOS = Vector3.zero;
  Quaternion lastRotation = Quaternion.identity;


	private void OnEnable () {
		StartCoroutine(DisplayFloatingText());
    data = GetComponent<ObjectDataContainer>();
    points = data.ObjectPoints;
	}

  private IEnumerator DisplayFloatingText() {
    float CountDuration = 2.0f; // How long is the countdown alive.    
    float starting_Count = Random.Range(5f, 20f); // At what number is the counter starting at.
    float current_Count = starting_Count;

    Vector3 start_pos = m_floatingText_Transform.position;
    Color32 start_color = m_textMeshPro.color;
    float alpha = 255;
    //int int_counter = 0;


    float fadeDuration = 3 / starting_Count * CountDuration;

    while (current_Count > 0)
    {
      current_Count -= (Time.deltaTime / CountDuration) * starting_Count;

      if (current_Count <= 3)
      {
          //Debug.Log("Fading Counter ... " + current_Count.ToString("f2"));
          alpha = Mathf.Clamp(alpha - (Time.deltaTime / fadeDuration) * 255, 0, 255);
      }

      //int_counter = (int)current_Count;                 
      m_textMeshPro.SetText(points + "");

      // Move the floating text upward each update
      m_floatingText_Transform.position += new Vector3(0, starting_Count * Time.deltaTime, 0);

      // Align floating text perpendicular to Camera.
      if (!lastPOS.Compare(m_cameraTransform.position, 1000) || !lastRotation.Compare(m_cameraTransform.rotation, 1000))
      {
          lastPOS = m_cameraTransform.position;
          lastRotation = m_cameraTransform.rotation;
          m_floatingText_Transform.rotation = lastRotation;
          Vector3 dir = m_transform.position - lastPOS;
          m_transform.forward = new Vector3(dir.x, 0, dir.z);
      }

      yield return new WaitForEndOfFrame();
    }

  }

}

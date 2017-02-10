using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour {

  [SerializeField]
  private Dropdown weaponDropdown;
  [SerializeField]
  private Text otherButtonText;
  [SerializeField]
  private Button otherButton;
  [SerializeField]
  private InputField otherText;

  private void OnEnable() {
    weaponDropdown.value = (int)DataManager.SelectedWeapon;

    weaponDropdown.onValueChanged.AddListener(delegate{
      SetWeapon(weaponDropdown.value); 
    });

    SetWeapon(weaponDropdown.value);





  }

  private void Update() {
    otherButtonText.text = "Load " + otherText.text;
  }

  public void SetWeapon(int val) {

    DataManager.SelectedWeapon = (Weapon)val;
  }

  public void LoadOther() {
    string scene = otherText.text;
    LoadScene(scene);
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}

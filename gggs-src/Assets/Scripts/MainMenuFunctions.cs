using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour {

  [SerializeField]
  private Dropdown weaponDropdown;

  private void OnEnable() {
    weaponDropdown.value = (int)DataManager.SelectedWeapon;

    weaponDropdown.onValueChanged.AddListener(delegate{
      SetWeapon(weaponDropdown.value); 
    });

    SetWeapon(weaponDropdown.value);
  }

  public void SetWeapon(int val) {

    DataManager.SelectedWeapon = (Weapon)val;
  }

  public void LoadScene(string scene) {
    SceneManager.LoadScene(scene);
  }

}

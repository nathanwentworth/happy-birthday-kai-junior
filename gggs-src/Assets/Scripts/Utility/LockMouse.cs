using UnityEngine;
using System.Collections;

public static class LockMouse {	

  public static void Lock(bool lockCursor) {
  	Cursor.visible = !lockCursor;
    Cursor.lockState = (lockCursor) ? CursorLockMode.Locked : CursorLockMode.None;

    Debug.Log("lockCursor " + lockCursor);
  }

}
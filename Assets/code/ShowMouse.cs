using UnityEngine;
using System.Collections;

public class ShowMouse : MonoBehaviour {

	void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
	
}

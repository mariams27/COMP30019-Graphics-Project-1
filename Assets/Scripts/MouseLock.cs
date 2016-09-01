using UnityEngine;
using System.Collections;

/***
 * Simple mouse center locking.
 */
public class MouseLock : MonoBehaviour {

    public bool enable = true;

	void Start () {
        if (enable) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}

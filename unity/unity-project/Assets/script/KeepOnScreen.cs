using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnScreen : MonoBehaviour {

	void Update () {
        Vector3 screenBorder = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));

        var newPosition = transform.position;
        if (Mathf.Abs(newPosition.x) > (screenBorder.x))
        {
            newPosition.x = Mathf.Sign(newPosition.x) * screenBorder.x;
        }
        transform.position = newPosition;
    }
}

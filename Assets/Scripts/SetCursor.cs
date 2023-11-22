using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    public bool cursorOn = false;

    // Update is called once per frame
    void Update()
    {
        if (!cursorOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatingUI : MonoBehaviour
{
    bool iDown;

    public Player player;
    public CameraMove cameraMove;
    public bool openCreatingUI;

    void Update()
    {
        GetInput();

        CreatingOut();
    }

    void GetInput()
    {
        iDown = Input.GetButton("Cancel");
    }

    void CreatingOut()
    {
        if (iDown)
        {
            player.enabled = true;
            cameraMove.enabled = true;
            player.isCreaingUIOpen = false;
            this.gameObject.SetActive(false);
        }
    }
}

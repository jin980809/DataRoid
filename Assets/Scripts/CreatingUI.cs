using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatingUI : MonoBehaviour
{
    bool qDown;

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
        qDown = Input.GetButton("Cancel");
    }

    void CreatingOut()
    {
        if (qDown)
        {
            CreatingUIOut();
        }
    }

    public void CreatingUIOut()
    {
        player.enabled = true;
        cameraMove.enabled = true;
        player.isCreaingUIOpen = false;
        player.isInventoryOpen = false;
        player.cameraArm.enabled = true;
        Time.timeScale = 1.0f;
        UIManager.Instance.CreatingUI.SetActive(false);
    }
}

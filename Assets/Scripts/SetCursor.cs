using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    Player player;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<Player>();
    }

    void Update()
    {
        if(player.isInventoryOpen || player.isCreaingUIOpen || GameManager.Instance.isPlayerDead || player.isCommunicate)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

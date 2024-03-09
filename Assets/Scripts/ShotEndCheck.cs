using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEndCheck : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public void ShotAnimEnd()
    {
        player.isShotEnd = true;
    }

}

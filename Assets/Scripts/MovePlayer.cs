using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : FadeController
{
    public Transform movePos;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeInOut();
            StartCoroutine(PlayerMove(other.gameObject));
        }
    }

    IEnumerator PlayerMove(GameObject player)
    {
        yield return new WaitForSeconds(1.5f);
        player.transform.position = movePos.position;
    }
}

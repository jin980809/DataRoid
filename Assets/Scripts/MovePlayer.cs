using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Transform movePos;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerMove(other.gameObject));
        }
    }

    IEnumerator PlayerMove(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.transform.position = movePos.position;
    }
}

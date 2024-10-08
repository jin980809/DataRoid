using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourMonsterWaveEnemy : MonoBehaviour
{
    RaycastHit hit;
    Animator anim;
    public Transform pos;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpDown_Fall"))
        {
            if (Physics.Raycast(pos.position, -pos.forward, out hit, 0.2f, LayerMask.GetMask("Enviroment")))
            {
                Debug.Log("AA");
                anim.SetTrigger("isGround");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 5);

        Gizmos.DrawRay(pos.position, -pos.forward * 0.2f);
    }
}

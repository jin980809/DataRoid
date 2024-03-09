using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    [Space(10f)]
    public int aroundTargetIndex = 0;
    public Transform[] aroundTarget;
    [SerializeField] private bool aroundIndexIncre;
    [SerializeField] bool isNotAround;
    public bool isStop = false;

    void Update()
    {
        Around();

        TargetPlayer();

        Chase();

        anim.SetFloat("speed", nav.speed);

        RotationSpeedUp();

        SwitchLight();

        HackingUI();

        HackingCoolDown();
    }

    void TargetPlayer()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (hit.Length > 0)
        {
            Vector3 playerPos = new Vector3(hit[0].transform.position.x, hit[0].transform.position.y + 1, hit[0].transform.position.z);
            isStop = false;
            isShotChase = false;

            if (!IsObstacleBetween(thisPos, playerPos, LayerMask.GetMask("Enviroment")))
            {
                if (!isAttack && !isDeath && !isStun)
                {
                    //Debug.Log("target " + hit[0].transform.name);
                    nav.speed = runSpeed;
                    isChase = true;
                }
            }
            else
            {
                if (isShotChase && !isDeath && !isStun)
                {
                    nav.SetDestination(playerShotPos);
                    if (nav.remainingDistance <= 0.5f)
                        isShotChase = false;
                }
            }

            RaycastHit[] attackHit = Physics.SphereCastAll(transform.position, attackDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
            if (attackHit.Length > 0 && !isAttack && !isDeath)
            {
                Vector3 AttackPos = new Vector3(attackHit[0].transform.position.x, attackHit[0].transform.position.y + 1, attackHit[0].transform.position.z);
                if (!IsObstacleBetween(thisPos, AttackPos, LayerMask.GetMask("Enviroment")) && !isStun)
                {
                    isShotChase = false;
                    attackCoroutine = StartCoroutine(DoAttack());
                }
            }
        }
        else
        {
            if(!isAttack && !isShotChase && !isDeath && !isStun)
            {
                nav.speed = walkSpeed;
                isChase = false;

                if(isStop && isNotAround)
                {
                    nav.speed = 0;
                }

            }
            else if(!isAttack && isShotChase)
            {
                nav.SetDestination(playerShotPos);
                if (nav.remainingDistance <= 0.5f)
                    isShotChase = false;
                isStop = false;
                nav.speed = runSpeed;
            }
        }
    }

    private bool IsObstacleBetween(Vector3 start, Vector3 end, LayerMask obstacleLayer)
    {
        RaycastHit hit;
        if (Physics.Raycast(start, (end - start).normalized, out hit, Vector3.Distance(start, end), obstacleLayer))
        {
            return true;
        }
        return false;
    }

    IEnumerator DoAttack()
    {
        anim.SetBool("isAttack", true);
        isAttack = true;
        nav.isStopped = true;

        yield return new WaitForSeconds(0.5f);
        attackCollider.enabled = true;

        yield return new WaitForSeconds(1f);
        attackCollider.enabled = false;

        yield return new WaitForSeconds(0.1f);
        nav.isStopped = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void Chase()
    {
        if(isChase && !isDeath && !isStun)
        {
            nav.SetDestination(target.transform.position);
        }
    }

    void Around()
    {
        if (!isChase && !isDeath && !isStun && !isNotAround)
        {
            if (nav.remainingDistance <= 0.5f)
            {
                //Debug.Log("dectect" + aroundTarget[aroundTargetIndex].name);
                //if (aroundTargetIndex == aroundTarget.Length - 1)
                //{
                //    aroundTargetIndex = 0;
                //}
                //else
                //{
                //    aroundTargetIndex++;
                //}


                if(aroundTargetIndex == aroundTarget.Length - 1)
                {
                    aroundIndexIncre = false;
                }
                else if (aroundTargetIndex == 0)
                {
                    aroundIndexIncre = true;
                }

                if (aroundIndexIncre)
                {
                    aroundTargetIndex++;
                }
                else if(!aroundIndexIncre)
                {
                    aroundTargetIndex--;
                }

                nav.SetDestination(aroundTarget[aroundTargetIndex].position);
            }
        }

        if(!isChase && !isDeath && !isStun && isNotAround)
        {
            nav.SetDestination(aroundTarget[0].position);

            if (nav.remainingDistance <= 0.5f)
            {
                nav.speed = 0;
                isStop = true;
            }
            else
            {
                //nav.isStopped = true;
                nav.speed = walkSpeed;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, chasingDistance);
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));

        if (hit.Length > 0)
        {
            if (!IsObstacleBetween(transform.position, hit[0].transform.position, LayerMask.GetMask("Enviroment")))
            {
                Gizmos.DrawLine(transform.position, hit[0].transform.position);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    [Space(10f)]
    public int aroundTargetIndex = 0;
    public Transform[] aroundTarget;

    void Update()
    {
        Around();

        TargetPlayer();

        Chase();

        anim.SetFloat("speed", nav.speed);

        RotationSpeedUp();

        SwitchLight();
    }

    void TargetPlayer()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (hit.Length > 0)
        {
            Vector3 playerPos = new Vector3(hit[0].transform.position.x, hit[0].transform.position.y + 1, hit[0].transform.position.z);
            if (!IsObstacleBetween(thisPos, playerPos, LayerMask.GetMask("Enviroment")))
            {
                if (!isAttack && !isDeath)
                {
                    //Debug.Log("target " + hit[0].transform.name);
                    nav.speed = runSpeed;
                    isShotChase = false;
                    isChase = true;
                }
            }
            else
            {
                if (isShotChase && !isDeath)
                {
                    nav.SetDestination(playerShotPos);
                    if ((transform.position.x == playerShotPos.x) && (transform.position.z == playerShotPos.z))
                        isShotChase = false;
                }
            }

            RaycastHit[] attackHit = Physics.SphereCastAll(transform.position, attackDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
            if (attackHit.Length > 0 && !isAttack && !isDeath)
            {
                Vector3 AttackPos = new Vector3(attackHit[0].transform.position.x, attackHit[0].transform.position.y + 1, attackHit[0].transform.position.z);
                if (!IsObstacleBetween(thisPos, AttackPos, LayerMask.GetMask("Enviroment")))
                {
                    isShotChase = false;
                    StartCoroutine(DoAttack());
                }
            }
        }
        else
        {
            if(!isAttack && !isShotChase && !isDeath)
            {
                nav.speed = walkSpeed;
                isChase = false;
            }
            else if(!isAttack && isShotChase)
            {
                nav.SetDestination(playerShotPos);

                nav.speed = runSpeed;

                if ((transform.position.x == playerShotPos.x) && (transform.position.z == playerShotPos.z))
                    isShotChase = false;

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

        yield return new WaitForSeconds(0.3f);
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
        if(isChase && !isDeath)
        {
            nav.SetDestination(target.transform.position);
        }
    }

    void Around()
    {
        if (!isChase && !isDeath)
        {
            nav.SetDestination(aroundTarget[aroundTargetIndex].position);

            if ((transform.position.x == aroundTarget[aroundTargetIndex].position.x) &&
                (transform.position.z == aroundTarget[aroundTargetIndex].position.z))
            {

                //Debug.Log("dectect" + aroundTarget[aroundTargetIndex].name);
                if (aroundTargetIndex == aroundTarget.Length - 1)
                {
                    aroundTargetIndex = 0;
                }
                else
                {
                    aroundTargetIndex++;
                }
            }
        }

    }

    void RotationSpeedUp()
    {
        Vector3 lookrotation = nav.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
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

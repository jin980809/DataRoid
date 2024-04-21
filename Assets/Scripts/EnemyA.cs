using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    [Space(10f)]
    [Header("Enemy Around & Targeting")]
    public int aroundTargetIndex = 0;
    public Transform[] aroundTarget;
    [SerializeField] private bool aroundIndexIncre;
    [SerializeField] bool isNotAround;
    public bool isStop = false;
    public float subduePercentage;
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

        Subdue();

        Parrying();
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
                if (!isAttack && !isDeath && !isStun && isSubdueReady && !isPlayerSubdue && isSubdueReady)
                {
                    //Debug.Log("target " + hit[0].transform.name);
                    nav.speed = runSpeed * speedDiscountRate;
                    isChase = true;
                }
            }
            else
            {
                if (isShotChase && !isDeath && !isStun && isSubdueReady)
                {
                    nav.SetDestination(playerShotPos);
                    if (nav.remainingDistance <= 0.5f)
                        isShotChase = false;
                }
            }

            RaycastHit[] attackHit = Physics.SphereCastAll(transform.position, attackDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
            if (attackHit.Length > 0 && !isAttack && !isDeath && !isPlayerSubdue && isSubdueReady)
            {
                Vector3 AttackPos = new Vector3(attackHit[0].transform.position.x, attackHit[0].transform.position.y + 1, attackHit[0].transform.position.z);
                if (!IsObstacleBetween(thisPos, AttackPos, LayerMask.GetMask("Enviroment")) && !isStun)
                {
                    isShotChase = false;

                    if(RandPercentage(subduePercentage)) //제압기 확률 성공(제압기)
                    {
                        if (!isPlayerSubdue && isSubdueReady )
                        { 
                            DoSubdueCor();
                        }
                    }
                    else // 실패(일반공격)
                    {
                        attackCoroutine = StartCoroutine(DoAttack());
                    }

                    
                    // 제압기시 적 subdue 콜라이더 활성화(코루틴으로 약간의 딜레이주기)
                    // 제압기 성공시 플레이어 subdue활성화 및 게이지 설정
                    // 제압기 패링시 (코루틴 딜레이 사이에 근접 공격을 넣으면) 코루틴 중지, 스턴?  
                }
            }
        }
        else
        {
            if (!isAttack && !isShotChase && !isDeath && !isStun && isSubdueReady && !isPlayerSubdue)
            {
                nav.speed = walkSpeed * speedDiscountRate;
                isChase = false;
                isShotChase = false;
                nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                if (isStop && isNotAround)
                {
                    nav.speed = 0;
                }

            }
            else if (!isAttack && isShotChase && isSubdueReady && !isPlayerSubdue)
            {
                nav.SetDestination(playerShotPos);
                if (nav.remainingDistance <= 0.5f)
                {
                    isShotChase = false;
                    nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                }
                isStop = false;
                nav.speed = runSpeed * speedDiscountRate;
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
                nav.speed = walkSpeed * speedDiscountRate;
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

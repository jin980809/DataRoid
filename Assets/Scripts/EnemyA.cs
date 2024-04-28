using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Space(10f)]
    [Header("Ranged")]
    public bool isRanged; //���Ÿ� ����
    public float LazerDamange;
    public float rangedDistance;
    public float rangedAttackCoolTime;
    float curRangedAttackCoolTime = 0;
    public float rangedAttackChargingTime;
    public float rangedAttackTime;
    Coroutine rangeAttack;
    public bool isRangedAttackReady;
    public bool isRangedAttack;
    public Transform rangedPos;
    Hovl_DemoLasers Lazers;
    float stunTime = 0;

    void Update()
    {
        Around();

        if (!isRanged)
            TargetPlayer();
        else
        {
            Lazers = GetComponentInChildren<Hovl_DemoLasers>();
            RangeTargetPlayer();
            RangedAttack();
        }

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

                    if(RandPercentage(subduePercentage)) //���б� Ȯ�� ����(���б�)
                    {
                        if (!isPlayerSubdue && isSubdueReady )
                        { 
                            DoSubdueCor();
                        }
                    }
                    else // ����(�Ϲݰ���)
                    {
                        attackCoroutine = StartCoroutine(DoAttack());
                    }

                    
                    // ���б�� �� subdue �ݶ��̴� Ȱ��ȭ(�ڷ�ƾ���� �ణ�� �������ֱ�)
                    // ���б� ������ �÷��̾� subdueȰ��ȭ �� ������ ����
                    // ���б� �и��� (�ڷ�ƾ ������ ���̿� ���� ������ ������) �ڷ�ƾ ����, ����?  
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

    void RangeTargetPlayer()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, rangedDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (curRangedAttackCoolTime < rangedAttackCoolTime && !isRangedAttackReady)
        {
            curRangedAttackCoolTime += Time.deltaTime;
        }
        else
        {
            curRangedAttackCoolTime = rangedAttackCoolTime;
        }

        if (hit.Length > 0) //�÷��̾� ����
        {
            Vector3 playerPos = new Vector3(hit[0].transform.position.x, hit[0].transform.position.y + 1, hit[0].transform.position.z);
            if (!IsObstacleBetween(thisPos, playerPos, LayerMask.GetMask("Enviroment")))
            {
                nav.speed = 0;
                isChase = true;
                if (rangedAttackCoolTime <= curRangedAttackCoolTime && !isRangedAttack && !isRangedAttackReady)
                {
                    rangeAttack = StartCoroutine(RangeAttackReady());
                }
            }
        }
        else // �÷��̾� ���� x
        {
            if(isRangedAttack)
            {
                Lazers.LazerOff();
                isRangedAttackReady = false;
                curRangedAttackCoolTime = 0;
                rotateRate = 1f;
                isRangedAttack = false;
            }

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

    IEnumerator RangeAttackReady()
    {
        isRangedAttackReady = true;
        Debug.Log("RangedAttackReady");

        yield return new WaitForSeconds(rangedAttackChargingTime);
        rotateRate = 0.75f;
        isRangedAttack = true;
        stunTime += Time.deltaTime;
        Lazers.LazerOn();

        yield return new WaitForSeconds(rangedAttackTime);
        Lazers.LazerOff();
        stunTime = 0;
        isRangedAttackReady = false;
        isRangedAttack = false;
        curRangedAttackCoolTime = 0;
        rotateRate = 1f;
        Debug.Log("Ranged Attack end");
    }
    void RangedAttack()
    {
        if(isRangedAttack)
        {
            Debug.Log("RangedAttack");
            RaycastHit hit;

            Vector3 playerPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);

            if (Physics.SphereCast(rangedPos.position, 0.1f, rangedPos.forward, out hit, 100f, LayerMask.GetMask("Player")))
            {
                if (!IsObstacleBetween(rangedPos.position, playerPos, LayerMask.GetMask("Enviroment")))
                {
                    Debug.Log("player hit");
                    target.GetComponent<Player>().Damage(transform.position, LazerDamange);
                    target.GetComponent<Player>().Stun(rangedAttackTime - stunTime + 0.01f);
                }
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
        Vector3 playerPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
        Gizmos.DrawRay(rangedPos.position, rangedPos.forward * 100);

        //Gizmos.DrawWireSphere(transform.position, chasingDistance);
        //Gizmos.DrawWireSphere(transform.position, attackDistance);

        //RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));

        //if (hit.Length > 0)
        //{
        //    if (!IsObstacleBetween(transform.position, hit[0].transform.position, LayerMask.GetMask("Enviroment")))
        //    {
        //        Gizmos.DrawLine(transform.position, hit[0].transform.position);
        //    }
        //}
    }

}


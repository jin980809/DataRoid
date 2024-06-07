using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Normal : Enemy
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

    [Space(10f)]
    [Header("Melee")]
    public int attack1Percent;
    public int attack2Percent;
    public int attack3Percent;



    void Update()
    {
        //Around();

        if (!isDeath)
        {
            if (!isRanged)
                EnemyAI();
            else
            {
                Lazers = GetComponentInChildren<Hovl_DemoLasers>();
                RangeTargetPlayer();
                RangedAttack();
            }
        }
        //Chase();

        anim.SetFloat("speed", nav.speed);

        RotationSpeedUp();

        //HackingUI();

        //HackingCoolDown();

        Subdue();

        Parrying();

        if (isDeath && !getData)
        {
            //ProgressManager.Instance.curAlarmData += 1;
            //ProgressManager.Instance.curCameraData += 1;
            //ProgressManager.Instance.curSilentData += 1;
            //ProgressManager.Instance.curNetworkData += 1;
            getData = true;
        }
    }

    void EnemyAI()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (hit.Length > 0) // �÷��̾� ����
        {
            //Debug.Log("�÷��̾� ����");
            //nav.isStopped = false;
            Vector3 playerPos = new Vector3(hit[0].transform.position.x, hit[0].transform.position.y + 1, hit[0].transform.position.z);
            isShotChase = false;

            if (!IsObstacleBetween(thisPos, playerPos, LayerMask.GetMask("Enviroment", "Door", "Object"))) //Ư�� ������Ʈ ���̿� �ִ��� Ȯ��  
            {
                //Debug.Log("�÷��̾� ����");
                isChase = true;
                nav.SetDestination(target.transform.position);
                nav.speed = runSpeed * speedDiscountRate;

                RaycastHit[] attackHit = Physics.SphereCastAll(transform.position, attackDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
                if (attackHit.Length > 0 && !isAttack && !isDeath && !isPlayerSubdue && isSubdueReady) // �� ����
                {
                    Vector3 AttackPos = new Vector3(attackHit[0].transform.position.x, attackHit[0].transform.position.y + 1, attackHit[0].transform.position.z);
                    if (!IsObstacleBetween(thisPos, AttackPos, LayerMask.GetMask("Enviroment")) && !isStun)
                    {
                        isShotChase = false;

                        if (RandPercentage(subduePercentage)) //���б� Ȯ�� ����(���б�)
                        {
                            if (!isPlayerSubdue && isSubdueReady)
                            {
                                DoSubdueCor();
                            }
                        }
                        else // ����(�Ϲݰ���)
                        {
                            int attackNum = AttackPercentage();
                            //isStop = false;
                            nav.isStopped = true;
                            switch (attackNum)
                            {
                                case 1:
                                    attackCoroutine = StartCoroutine(DoAttack1());
                                    break;
                                case 2:
                                    attackCoroutine = StartCoroutine(DoAttack2());
                                    break;
                                case 3:
                                    attackCoroutine = StartCoroutine(DoAttack3());
                                    break;
                            }
                        }


                        // ���б�� �� subdue �ݶ��̴� Ȱ��ȭ(�ڷ�ƾ���� �ణ�� �������ֱ�)
                        // ���б� ������ �÷��̾� subdueȰ��ȭ �� ������ ����
                        // ���б� �и��� (�ڷ�ƾ ������ ���̿� ���� ������ ������) �ڷ�ƾ ����, ����?  
                    }
                } 
            }
        }
        else // �÷��̾� ���� �ȵɋ�
        {
            if(isShotChase) //�÷��̾ ���� ������
            {
                //Debug.Log("�÷��̾� �ѽ���ġ ��");
                //nav.isStopped = false;
                nav.SetDestination(playerShotPos);
                nav.speed = runSpeed * speedDiscountRate;

                if(nav.remainingDistance <= 0.1f && !isA)
                {
                    isShotChase = false;
                }
                isA = false;
            }
            else
            {
                isA = true;
                if (isNotAround) //���ƴٴ��� �ʴ� ��
                {
                    //Debug.Log("�� ������ġ��");
                    nav.SetDestination(aroundTarget[0].position);
                    nav.speed = walkSpeed * speedDiscountRate;
                    if (nav.remainingDistance <= 0.1f)
                    {
                        //Debug.Log("�� ����");
                        //nav.isStopped = true;
                        nav.speed = 0;
                    }
                }
                else // ���ƴٴϴ� ��
                {
                    //Debug.Log("�� ���ƴٴ�");
                    //nav.isStopped = false;
                    nav.speed = walkSpeed * speedDiscountRate;
                    if (nav.remainingDistance <= 0.1f && isA)
                    {
                        if (aroundTargetIndex == aroundTarget.Length - 1)
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
                        else if (!aroundIndexIncre)
                        {
                            aroundTargetIndex--;
                        }
                        nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                    }
                }
            }
        }

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
                    if (nav.remainingDistance <= 0f)
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

                    if (RandPercentage(subduePercentage)) //���б� Ȯ�� ����(���б�)
                    {
                        if (!isPlayerSubdue && isSubdueReady)
                        {
                            DoSubdueCor();
                        }
                    }
                    else // ����(�Ϲݰ���)
                    {
                        int attackNum = AttackPercentage();
                        isStop = false;
                        switch (attackNum)
                        {
                            case 1:
                                attackCoroutine = StartCoroutine(DoAttack1());
                                break;
                            case 2:
                                attackCoroutine = StartCoroutine(DoAttack2());
                                break;
                            case 3:
                                attackCoroutine = StartCoroutine(DoAttack3());
                                break;
                        }
                    }


                    // ���б�� �� subdue �ݶ��̴� Ȱ��ȭ(�ڷ�ƾ���� �ణ�� �������ֱ�)
                    // ���б� ������ �÷��̾� subdueȰ��ȭ �� ������ ����
                    // ���б� �и��� (�ڷ�ƾ ������ ���̿� ���� ������ ������) �ڷ�ƾ ����, ����?  
                }
            }
        }
        else
        {
            if (!isAttack && !isShotChase && !isDeath && !isStun && isSubdueReady && !isPlayerSubdue) //�������� �ʰ� ������ �ʰ� ���ƴٴҶ�
            {
                Debug.Log("BB");

                nav.speed = walkSpeed * speedDiscountRate;
                isChase = false;
                isShotChase = false;

                if (!isShotChase)
                    nav.SetDestination(aroundTarget[aroundTargetIndex].position);

                if (isStop && isNotAround)
                {
                    Debug.Log("Bb");
                    nav.speed = 0;
                }

            }

            if (!isAttack && isShotChase && isSubdueReady && !isPlayerSubdue && !isDeath && !isStun) //���� ���� �ʰ� ������
            {
                nav.SetDestination(playerShotPos);
                isStop = false;

                if (nav.remainingDistance <= 0.1f)
                {
                    nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                    //isShotChase = false;
                }

                nav.speed = runSpeed * speedDiscountRate;
            }
        }
    }

    void Around()
    {
        if (!isChase && !isDeath && !isStun && !isNotAround && !isShotChase)
        {
            Debug.Log("CC");
            if (nav.remainingDistance <= 0f)
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


                if (aroundTargetIndex == aroundTarget.Length - 1)
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
                else if (!aroundIndexIncre)
                {
                    aroundTargetIndex--;
                }

                nav.SetDestination(aroundTarget[aroundTargetIndex].position);
            }
        }

        else if (!isChase && !isDeath && !isStun && isNotAround && !isShotChase && !isStop)
        {
            Debug.Log("DD");
            if (!isShotChase)
                nav.SetDestination(aroundTarget[0].position);

            if (nav.remainingDistance <= 0.1f)
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
            if (isRangedAttack)
            {
                Lazers.LazerOff();
                isRangedAttackReady = false;
                curRangedAttackCoolTime = 0;
                rotateRate = 1f;
                isRangedAttack = false;
            }

            if (!isRangedAttack && !isShotChase && !isDeath && !isStun)
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
            else if (!isRangedAttack && isShotChase)
            {
                isChase = false;
                nav.SetDestination(playerShotPos);
                if (nav.remainingDistance <= 0f)
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
        if (isRangedAttack)
        {
            Debug.Log("RangedAttack");
            RaycastHit hit;

            Vector3 playerPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);

            if (Physics.SphereCast(rangedPos.position, 0.1f, rangedPos.forward, out hit, 100f, LayerMask.GetMask("Player")))
            {
                if (!IsObstacleBetween(rangedPos.position, playerPos, LayerMask.GetMask("Enviroment")))
                {
                    Debug.Log("player hit");
                    target.GetComponent<Player>().Damage(LazerDamange);
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

    int AttackPercentage()
    {
        int rand = Random.Range(1, 101);
        if (rand >= 1 && rand < attack1Percent)
        {
            return 1;
        }
        else if (rand >= attack1Percent && rand < attack1Percent + attack2Percent)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    IEnumerator DoAttack1()
    {
        anim.SetBool("isAttack", true);
        anim.SetFloat("attackIndex", 1f);
        isAttack = true;
        nav.isStopped = true;

        yield return new WaitForSeconds(1.1f);
        L_attackCollider.enabled = true;

        yield return new WaitForSeconds(0.22f);
        L_attackCollider.enabled = false;

        yield return new WaitForSeconds(1.17f);
        nav.isStopped = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    IEnumerator DoAttack2()
    {
        anim.SetBool("isAttack", true);
        anim.SetFloat("attackIndex", 2f);
        isAttack = true;
        nav.isStopped = true;

        yield return new WaitForSeconds(0.55f);
        R_attackCollider.enabled = true;

        yield return new WaitForSeconds(0.11f);
        R_attackCollider.enabled = false;

        yield return new WaitForSeconds(0.578f);
        L_attackCollider.enabled = true;

        yield return new WaitForSeconds(0.15f);
        L_attackCollider.enabled = false;

        yield return new WaitForSeconds(0.8f);
        nav.isStopped = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    IEnumerator DoAttack3()
    {
        anim.SetBool("isAttack", true);
        anim.SetFloat("attackIndex", 3f);
        isAttack = true;
        nav.isStopped = true;

        yield return new WaitForSeconds(1.11f);
        L_attackCollider.enabled = true;

        yield return new WaitForSeconds(0.28f);
        L_attackCollider.enabled = false;

        yield return new WaitForSeconds(1f);
        nav.isStopped = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    //void Chase()
    //{
    //    if (isChase && !isDeath && !isStun)
    //    {
    //        nav.SetDestination(target.transform.position);
    //    }
    //}



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


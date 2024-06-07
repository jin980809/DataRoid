using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Bomb : Enemy
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
    [Header("Melee")]
    public int attack1Percent;
    public int attack2Percent;
    public int attack3Percent;

    [Space(10f)]
    [Header("Bomb")]
    public bool isBomb;
    public bool isDetectBomb; // ������ ������ �ٰ����� ������
    bool bombChasing = false;
    bool isBombAttack = false ;
    public float activeHP;
    public float bombTime;
    public Collider bombRange;
    public float bombActiveSpeed;
    public GameObject bombEffect;

    void Update()
    {
        //Around();

        //TargetPlayer();

        if(!isDeath)
        {
            EnemyAI();
        }

        Chase();

        anim.SetFloat("speed", nav.speed);

        RotationSpeedUp();

        //HackingUI();

        //HackingCoolDown();

        Subdue();

        Parrying();

        BombActive();

        if (isDeath && !getData)
        {
            //ProgressManager.Instance.curAlarmData += 4;
            if (bombCor != null)
            {
                StopCoroutine(bombCor);
            }
            getData = true;
        }
    }
    void BombActive()
    {
        if(curHealth <= activeHP)
        {
            isBomb = true;
        }
    }

    void EnemyAI()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (!isDetectBomb)
        {
            // ��ź �۵���
            if (isBomb)
            {
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
                    if (isShotChase) //�÷��̾ ���� ������
                    {
                        //Debug.Log("�÷��̾� �ѽ���ġ ��");
                        //nav.isStopped = false;
                        nav.SetDestination(playerShotPos);
                        nav.speed = runSpeed * speedDiscountRate;

                        if (nav.remainingDistance <= 0.1f && !isA)
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
            else //��ź ����
            {
                bombCor = StartCoroutine(Bomb());
                isBombAttack = true;

                if (bombChasing)
                {
                    nav.SetDestination(target.transform.position);


                    if (nav.remainingDistance <= 1f)
                    {
                        nav.velocity = Vector3.zero;
                        nav.isStopped = true;
                        nav.speed = 0;
                    }
                    else
                    {
                        nav.isStopped = false;
                        nav.speed = bombActiveSpeed;
                    }
                }
            }
        }
        else
        {
            if (!isBombAttack && isDetectBomb)
                anim.SetTrigger("doDown");

            if (isDetectBomb)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3, Vector3.up, 0f, LayerMask.GetMask("Player"));
                if (hits.Length > 0 || isBombAttack)
                {
                    if (!IsObstacleBetween(thisPos, target.transform.position, LayerMask.GetMask("Enviroment", "Door")) && !isDeath)
                    {
                        if (!isBombAttack && !isDeath)
                        {
                            anim.SetTrigger("doRise");
                            nav.isStopped = true;
                            bombCor = StartCoroutine(Bomb());
                            isBombAttack = true;
                        }
                    }
                }

                if (bombChasing)
                {
                    nav.SetDestination(target.transform.position);


                    if (nav.remainingDistance <= 1f)
                    {
                        nav.velocity = Vector3.zero;
                        nav.isStopped = true;
                        nav.speed = 0;
                    }
                    else
                    {
                        nav.isStopped = false;
                        nav.speed = bombActiveSpeed;
                    }
                }
            }
        }
    }


    void TargetPlayer()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, chasingDistance, Vector3.up, 0f, LayerMask.GetMask("Player"));
        Vector3 thisPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (!isDetectBomb)
        {
            if (!isBomb)
            {
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
                            isStop = false;
                            nav.SetDestination(playerShotPos);
                            if (nav.remainingDistance <= 0.1f)
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

                    if (!isAttack && !isShotChase && !isDeath && !isStun && isSubdueReady && !isPlayerSubdue)
                    {
                        nav.speed = walkSpeed * speedDiscountRate;
                        isChase = false;
                        //isShotChase = false;
                        nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                        if (isStop && isNotAround)
                        {
                            nav.speed = 0;
                        }
                    }
                    else if (!isAttack && isShotChase && isSubdueReady && !isPlayerSubdue)
                    {
                        nav.SetDestination(playerShotPos);
                        if (nav.remainingDistance <= 0.1f)
                        {
                            isShotChase = false;
                            nav.SetDestination(aroundTarget[aroundTargetIndex].position);
                        }
                        isStop = false;
                        nav.speed = runSpeed * speedDiscountRate;
                    }
                }
            }
            else
            {
                bombCor = StartCoroutine(Bomb());
                isBombAttack = true;

                if (bombChasing)
                {
                    nav.SetDestination(target.transform.position);


                    if (nav.remainingDistance <= 1f)
                    {
                        nav.velocity = Vector3.zero;
                        nav.isStopped = true;
                        nav.speed = 0;
                    }
                    else
                    {
                        nav.isStopped = false;
                        nav.speed = bombActiveSpeed;
                    }
                }
            }
        }
        else
        {
            if(!isBombAttack && isDetectBomb)
                anim.SetTrigger("doDown");

            if (isDetectBomb)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 3, Vector3.up, 0f, LayerMask.GetMask("Player"));
                if (hits.Length > 0 || isBombAttack)
                {
                    if (!IsObstacleBetween(thisPos, target.transform.position, LayerMask.GetMask("Enviroment", "Door")) && !isDeath)
                    {
                        if (!isBombAttack && !isDeath)
                        {
                            anim.SetTrigger("doRise");
                            nav.isStopped = true;
                            bombCor = StartCoroutine(Bomb());
                            isBombAttack = true;
                        }
                    }
                }

                if(bombChasing)
                {
                    nav.SetDestination(target.transform.position);
                    

                    if (nav.remainingDistance <= 1f)
                    {
                        nav.velocity = Vector3.zero;
                        nav.isStopped = true;
                        nav.speed = 0;
                    }
                    else
                    {
                        nav.isStopped = false;
                        nav.speed = bombActiveSpeed;
                    }  
                }
            }
        }
    }

    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(3f);
        nav.speed = bombActiveSpeed;
        nav.isStopped = false;
        bombChasing = true;

        yield return new WaitForSeconds(bombTime);
        bombRange.enabled = true;
        bombEffect.SetActive(true);
        //nav.SetDestination(transform.position);

        yield return new WaitForSeconds(0.3f);
        bombRange.enabled = false;
        isDeath = true;
        OnDamage(100, playerShotPos, -1);
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

    void Chase()
    {
        if (isChase && !isDeath && !isStun)
        {
            nav.SetDestination(target.transform.position);
        }
    }

    void Around()
    {
        if (!isChase && !isDeath && !isStun && !isNotAround)
        {
            if (nav.remainingDistance <= 0.1f)
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

        if (!isChase && !isDeath && !isStun && isNotAround && !isShotChase)
        {
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 playerPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
        //Gizmos.DrawRay(rangedPos.position, rangedPos.forward * 100);

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


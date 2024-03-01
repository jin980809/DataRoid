using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    int diff = 1;

    private bool isMoveStop = false;
    private bool isMoveFar = false;
    public float moveDistance;
    public float stopDistance;

    public float nAttackCoolTime;
    public float sAttackCoolTime;
    public float sAttack2CoolTime;

    public bool isNormalAttack = false;
    public bool isSpecialAttack = false;
    public bool isNAttackReady = false;
    public bool isSAttackReady = false;
    public bool isDashAttack = false;
    public bool isMeleeAttack = false;
    bool isNCoolDown = true;
    public bool isSCoolDown = true;
    public bool isAimming = false; // �̵� ���߱� 

    public float flashAngle;
    public float flashDistance;

    [SerializeField] float curNAttackCoolTime = 0;
    [SerializeField] float curSAttack1CoolTime = 0;

    public Transform attackPos;
    public Transform spawnPoint;
    public float shotDamage;

    IEnumerator cSpecialAttack;
    IEnumerator cNormalAttack;
    IEnumerator cMeleeAttack;

    public GameObject handGun;
    public GameObject riffle;
    public Interaction p2EndInter;

    void Update()
    {
        LookAtTarget();

        HackingUI();

        HackingCoolDown();

        Targeting();

        //anim.SetFloat("speed", nav.speed);
        DiffSetting();

        Attack();

        Stun();

        //DashAttack();

        Die();
    }

    void DiffSetting()
    {
        if (!isMeleeAttack)
        {
            nav.speed = walkSpeed;
        }
        else
        {
            nav.speed = runSpeed;
        }
        diff = GameManager.Instance.ESN08Phase2Diff;
        if (diff == 1)
            handGun.SetActive(true);
        else
            riffle.SetActive(true);
        anim.SetInteger("Diff", diff);
    }

    void Die()
    {
        if(isDeath)
        {
            p2EndInter.enabled = true;
        }
    }

    void Targeting()
    {
        Vector3 playerTargeting = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        
        float distance = Vector3.Distance(enemyPos, playerTargeting);
        if (!isAimming && !isStun)
        {
            if (distance <= stopDistance) // �÷��̾�� �־���
            {
                anim.SetFloat("Vertical", -1f);
                anim.SetFloat("speed", nav.speed);
                nav.isStopped = false;
                isMoveFar = true;
                isMoveStop = false;
                Vector3 oppositeDirection = (transform.position - target.transform.position).normalized;
                nav.SetDestination(transform.position + oppositeDirection * 3.5f);
            }

            else if (distance < moveDistance && distance > stopDistance) // ����
            {
                anim.SetFloat("speed", 0);
                nav.isStopped = true;
                isMoveStop = true;
                isMoveFar = false;
            }

            else if (distance >= moveDistance) //�÷��̾�� �̵�
            {
                anim.SetFloat("Vertical", 1f);
                anim.SetFloat("speed", nav.speed);
                isMoveStop = false;
                isMoveFar = false;
                nav.isStopped = false;
                nav.SetDestination(target.transform.position);
            }
        }
        else
        {
            if(isMeleeAttack)
                nav.SetDestination(target.transform.position);
        }

    }

    void LookAtTarget()
    {
        if (target != null && !isNormalAttack && !isSpecialAttack && !isDashAttack)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPosition);
        }
    }

    void Attack()
    {
        if (isNCoolDown)
        {
            curNAttackCoolTime += Time.deltaTime;
        }

        if (isSCoolDown)
        {
            curSAttack1CoolTime += Time.deltaTime;
        }

        RaycastHit hit;

        if(isMoveFar && !isNAttackReady && !isSAttackReady && !isNormalAttack && !isSpecialAttack && !isMeleeAttack & !isDashAttack && !isStun && Physics.Raycast(attackPos.position, -attackPos.forward, out hit, 1.5f, LayerMask.GetMask("Enviroment")))
        {
            //isDashAttack = true;
            isAimming = true;
            attackCollider.enabled = true;
            //nav.SetDestination(target.transform.position);
            MeleeAttack();
            //Debug.Log("DashAttackReady");
        }
        else if ((isMoveStop || isMoveFar) && !isNormalAttack && !isSpecialAttack && !isMeleeAttack && curSAttack1CoolTime > sAttackCoolTime && !isSAttackReady && !isNAttackReady && !isDashAttack && !isStun)
        {
            isSAttackReady = true;
            isSCoolDown = false;
            
            curSAttack1CoolTime = 0;
            Debug.Log("Special1AttackReady");
            cSpecialAttack = SpecialAttackReady();
            StartCoroutine(cSpecialAttack);
        }
        else if(!isNormalAttack && !isSpecialAttack && !isMeleeAttack && curNAttackCoolTime > nAttackCoolTime && !isNAttackReady && !isSAttackReady && !isDashAttack && !isStun)
        {
            isNAttackReady = true;
            isNCoolDown = false;
            curNAttackCoolTime = 0;
            anim.SetBool("isAimming", true);
            Debug.Log("NormalAttackReady");
            cNormalAttack = NormalAttackReady();
            StartCoroutine(cNormalAttack);
        }
    }

    void DashAttack()
    {
        if (isDashAttack)
        {
            nav.SetDestination(spawnPoint.position);
        }

        if(nav.remainingDistance <= 0.5f)
        {
            nav.SetDestination(target.transform.position);
            isAimming = false;
            isDashAttack = false;
            attackCollider.enabled = false;
        }
    }

    IEnumerator NormalAttackReady()
    {
        nav.isStopped = true;
        isAimming = true;
        //���ݰ�� ǥ��
        //����Ʈ �߰�

        yield return new WaitForSeconds(3f);
        anim.SetBool("isAimming", false);
        anim.SetBool("isShotOut", false);
        isNormalAttack = true;
        isNAttackReady = false;


        for (int i = 0; i < diff + 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            NormalAttack();
        }

        yield return new WaitForSeconds(0.5f);

        isNormalAttack = false;
        isNCoolDown = true;
        isAimming = false;
        anim.SetBool("isShotOut", true);
    }

    void NormalAttack()
    {
        RaycastHit hit;
        //�ѱ� ȭ�� ����Ʈ
        anim.SetTrigger("doShot");
        if (Physics.SphereCast(attackPos.position, 0.05f, attackPos.forward, out hit, 20, LayerMask.GetMask("Player")))
        {
            if (!IsObstacleBetween(attackPos.position, hit.transform.position, LayerMask.GetMask("Enviroment")))
            {
                Debug.Log("Player Attack");
                Player player = hit.transform.gameObject.GetComponent<Player>();
                StartCoroutine(player.OnDamage(attackPos.position, shotDamage));
            }
            else
            {
                Debug.Log("Attack");
            }
        }
        else
        {
            Debug.Log("Attack");
        }
    }

    IEnumerator SpecialAttackReady()
    {
        nav.isStopped = true;
        isAimming = true;
        //���ݰ�� ǥ��

        yield return new WaitForSeconds(3f);
        isSpecialAttack = true;
        isSAttackReady = false;
        //�÷��� ����Ʈ�߰�
        yield return new WaitForSeconds(1f);
        SpecialAttack();

        //yield return new WaitForSeconds(1f);
        nav.isStopped = false;
    }

    void SpecialAttack()
    {
        isSpecialAttack = true;

        // SphereCast�� ���� ����� ����ĳ��Ʈ ����
        RaycastHit[] hits = Physics.SphereCastAll(attackPos.position, flashDistance, transform.forward, 0, LayerMask.GetMask("Player"));

        if (hits.Length > 0)
        {
            Vector3 dirToTarget = (hits[0].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < flashAngle / 2 && !IsObstacleBetween(attackPos.position, hits[0].transform.position, LayerMask.GetMask("Enviroment")))
            {
                Debug.Log("player stun");
                Player playerObject = hits[0].collider.gameObject.GetComponent<Player>();
                playerObject.Stun(5f);
                nav.SetDestination(hits[0].collider.transform.position);
                isAimming = true;
                MeleeAttack();
            }
            else
            {
                isAimming = false;
                isSCoolDown = true;
                isSpecialAttack = false;
            }
        }
        else
        {
            isAimming = false;
            isSCoolDown = true;
            isSpecialAttack = false;
        }
        //StartCoroutine(Special1AttackOut());
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

    void MeleeAttack()
    {
        Debug.Log("MeleeAttack");
        isMeleeAttack = true;
        anim.SetFloat("speed", runSpeed);
        anim.SetFloat("Vertical", 1);
        attackCollider.enabled = true;
    }

    public void Stun()
    {
        if (isStun)
        { 
            //��ų �ڷ�ƾ ���߱�
            if (cMeleeAttack != null)
            {
                StopCoroutine(cMeleeAttack);
                cMeleeAttack = null;
            }
            if (cSpecialAttack != null)
            {
                StopCoroutine(cSpecialAttack);
                cSpecialAttack = null;
            }
            if (cNormalAttack != null)
            {
                StopCoroutine(cNormalAttack);
                cNormalAttack = null;
            }

            attackCollider.enabled = false;

            isAimming = false;
            isDashAttack = false;
            isMeleeAttack = false;
            isNAttackReady = false;
            isSAttackReady = false;
            isSCoolDown = true;
            isNCoolDown = true;
            isNormalAttack = false;
            isSpecialAttack = false;
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<Player>().Stun(3f);
    //    }
    //}

    void OnDrawGizmos()
    {
        // �÷��̾��� ��ġ�� ������ ������
        Vector3 playerPosition = transform.position;
        Vector3 playerDirection = transform.forward;

        // ������ ���� ���� ���
        Vector3 coneDirection = Quaternion.Euler(0, -flashAngle / 2, 0) * playerDirection;

        // �ð������� ���� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerPosition, coneDirection * flashDistance);

        // �ð������� ������ ������ ��Ÿ���� ���� ����
        float halfAngle = flashAngle / 2;
        Quaternion leftRayRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, halfAngle, 0);

        Gizmos.DrawRay(playerPosition, leftRayRotation * playerDirection * flashDistance);
        Gizmos.DrawRay(playerPosition, rightRayRotation * playerDirection * flashDistance);
        Gizmos.DrawRay(attackPos.position, transform.forward * 20);
        Gizmos.DrawRay(attackPos.position, -transform.forward * 1.5f);

    }
    
    public void ShotAnimEnd()
    {

    }
}

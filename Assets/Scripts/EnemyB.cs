using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    int diff = 1;

    [SerializeField] private bool isMoveStop = false;
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
    public bool isAimming = false; // 이동 멈추기 

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

    void Update()
    {
        LookAtTarget();

        HackingUI();

        HackingCoolDown();

        Targeting();

        nav.speed = runSpeed;

        anim.SetFloat("speed", nav.speed);

        Attack();

        Stun();

        DashAttack();
    }

    void Targeting()
    {
        Vector3 playerTargeting = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(enemyPos, playerTargeting);
        if (!isAimming && !isStun)
        {
            if (distance <= stopDistance) // 플레이어와 멀어짐
            {
                nav.isStopped = false;
                isMoveStop = true;
                Vector3 oppositeDirection = (transform.position - target.transform.position).normalized;
                nav.SetDestination(transform.position + oppositeDirection * 3.5f);
            }

            else if (distance < moveDistance && distance > stopDistance) // 멈춤
            {
                nav.isStopped = true;
                isMoveStop = true;
            }

            else if (distance >= moveDistance) //플레이어에게 이동
            {
                isMoveStop = false;
                nav.isStopped = false;
                nav.SetDestination(target.transform.position);
            }
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

        if(isMoveStop && !isNAttackReady && !isSAttackReady && !isNormalAttack && !isSpecialAttack && !isMeleeAttack & !isDashAttack && !isStun && Physics.Raycast(attackPos.position, -attackPos.forward, out hit, 1.5f, LayerMask.GetMask("Enviroment")))
        {
            isDashAttack = true;
            isAimming = true;
            attackCollider.enabled = true;
            Debug.Log("DashAttackReady");
        }
        else if (isMoveStop && !isNormalAttack && !isSpecialAttack && !isMeleeAttack && curSAttack1CoolTime > sAttackCoolTime && !isSAttackReady && !isNAttackReady && !isDashAttack && !isStun)
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
        //공격경로 표시
        //이팩트 추가

        yield return new WaitForSeconds(3f);
        isNormalAttack = true;
        isNAttackReady = false;


        for (int i = 0; i < diff + 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            NormalAttack();
        }

        isNormalAttack = false;
        isNCoolDown = true;
        isAimming = false;
    }

    void NormalAttack()
    {
        RaycastHit hit;
        //총구 화염 이팩트

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
        //공격경로 표시

        yield return new WaitForSeconds(3f);
        isSpecialAttack = true;
        isSAttackReady = false;
        //플래쉬 이팩트추가
        yield return new WaitForSeconds(1f);
        SpecialAttack();

        yield return new WaitForSeconds(1f);
        isSpecialAttack = false;
        nav.isStopped = false;
    }

    void SpecialAttack()
    {
        isSpecialAttack = true;

        // SphereCast로 원뿔 모양의 레이캐스트 수행
        RaycastHit[] hits = Physics.SphereCastAll(attackPos.position, flashDistance, transform.forward, 0, LayerMask.GetMask("Player"));

        if (hits.Length > 0)
        {
            Vector3 dirToTarget = (hits[0].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < flashAngle / 2 && !IsObstacleBetween(attackPos.position, hits[0].transform.position, LayerMask.GetMask("Enviroment")))
            {
                Debug.Log("player stun");
                Player playerObject = hits[0].collider.gameObject.GetComponent<Player>();
                playerObject.Stun(5f);
                cMeleeAttack = MeleeAttack();
                StartCoroutine(cMeleeAttack);
            }
            else
            {
                isAimming = false;
                isSCoolDown = true;
            }
        }
        else
        {
            isAimming = false;
            isSCoolDown = true;
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

    IEnumerator MeleeAttack()
    {
        Debug.Log("MeleeAttack");
        isMeleeAttack = true;
        isAimming = true;
        nav.SetDestination(target.transform.position);
        attackCollider.enabled = true;
        yield return null;
    }

    public void Stun()
    {
        if(isStun)
        {
            //스킬 코루틴 멈추기
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Stun(3f);
        }
    }

    void OnDrawGizmos()
    {
        // 플레이어의 위치와 방향을 가져옴
        Vector3 playerPosition = transform.position;
        Vector3 playerDirection = transform.forward;

        // 원뿔의 방향 벡터 계산
        Vector3 coneDirection = Quaternion.Euler(0, -flashAngle / 2, 0) * playerDirection;

        // 시각적으로 원뿔 표현
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerPosition, coneDirection * flashDistance);

        // 시각적으로 원뿔의 각도를 나타내기 위한 레이
        float halfAngle = flashAngle / 2;
        Quaternion leftRayRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, halfAngle, 0);

        Gizmos.DrawRay(playerPosition, leftRayRotation * playerDirection * flashDistance);
        Gizmos.DrawRay(playerPosition, rightRayRotation * playerDirection * flashDistance);
        Gizmos.DrawRay(attackPos.position, transform.forward * 20);
        Gizmos.DrawRay(attackPos.position, -transform.forward * 1.5f);

    }
}

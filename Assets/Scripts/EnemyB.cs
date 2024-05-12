using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    int diff = 1;
    [Space(10f)]
    [Header("ESN08")]

    [Space(10f)]
    [Header("Move")]
    public float moveDistance;
    public float stopDistance;

    [Space(10f)]
    [Header("AttackCool")]
    public float nAttackCoolTime;
    float curNAttackCoolTime = 0;
    public float sAttackCoolTime;
    float curSAttackCoolTime = 0;

    public bool isNormalAttack = false;
    public bool isSpecialAttack = false;

    public bool isNAttackReady = false;
    public bool isSAttackReady = false;

    [Space(10f)]
    [Header("Flash")]
    public float flashAngle;
    public float flashDistance;
    public GameObject flashLens;
    public Light flashlight;

    [Space(10f)]
    [Header("else")]
    public Transform attackPos;
    public Transform spawnPoint;
    public float shotDamage;
    public GameObject handGun;
    public GameObject riffle;
    public Interaction p2EndInter;

    Coroutine cFlashAttack;
    Coroutine cNormalAttack;

    private bool isAimming = true;
    public Flash flashEffect;
    public GameObject[] muzzleFlashs;

    private GameObject muzzleFlash;
    void Update()
    {
        //LookAtTarget();

        //HackingUI();

        //HackingCoolDown();

        LookAtTarget();


        Targeting();

            
        //anim.SetFloat("speed", nav.speed);
        DiffSetting();

        Attack();

        Stun();

        //DashAttack();

        Subdue();

        Parrying();

        Die();
    }

    void DiffSetting()
    {
        diff = GameManager.Instance.ESN08Phase2Diff;
        if (diff <= 1)
        {
            diff = 1;
            handGun.SetActive(true);
            muzzleFlash = muzzleFlashs[0];
        }
        else
        {
            riffle.SetActive(true);
            muzzleFlash = muzzleFlashs[1];
        }
        anim.SetInteger("Diff", diff);
    }

    void Die()
    {
        if(isDeath)
        {
            p2EndInter.enabled = true;

            if (cFlashAttack != null)
                StopCoroutine(cFlashAttack);

            if (cNormalAttack != null)
                StopCoroutine(cNormalAttack);

        }
    }

    void Targeting()
    {
        Vector3 playerTargeting = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(enemyPos, playerTargeting);

        if (isNormalAttack || isSpecialAttack || isStun)
        {
            nav.isStopped = true;
            anim.SetFloat("speed", 0);
        }
        else
        {
            if (distance <= stopDistance) // 플레이어와 멀어짐
            {
                anim.SetFloat("Vertical", -1f);
                anim.SetFloat("speed", nav.speed);
                nav.isStopped = false;

                Vector3 oppositeDirection = (transform.position - target.transform.position).normalized;
                nav.SetDestination(transform.position + oppositeDirection * stopDistance);
            }

            else if (distance < moveDistance && distance > stopDistance) // 멈춤
            {
                anim.SetFloat("speed", 0);
                nav.isStopped = true;

            }

            else if (distance >= moveDistance) //플레이어에게 이동
            {
                anim.SetFloat("Vertical", 1f);
                anim.SetFloat("speed", nav.speed);

                nav.isStopped = false;
                nav.SetDestination(target.transform.position);
            }
        }
    }

    void LookAtTarget()
    {
        if(isAimming)
            transform.LookAt(target.transform.position);
    }

    void Attack()
    {
        if(nAttackCoolTime <= curNAttackCoolTime && !isNormalAttack && isSubdueReady)
        {
            isNAttackReady = true;
        }
        else
        {
            curNAttackCoolTime += Time.deltaTime;
            isNAttackReady = false;
        }

        if (sAttackCoolTime <= curSAttackCoolTime && !isSpecialAttack)
        {
            isSAttackReady = true;
        }
        else
        {
            curSAttackCoolTime += Time.deltaTime;
            isSAttackReady = false;
        }

        if (isSAttackReady && !isNormalAttack && !isSpecialAttack && !isStun && !isPlayerSubdue)
        {
            cFlashAttack = StartCoroutine(FlashAttackReady());
            curSAttackCoolTime = 0;
        }


        if (isNAttackReady && !isNormalAttack && !isSpecialAttack && !isStun && isSubdueReady && !isPlayerSubdue)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) >= 1.5f)
            {
                //원거리 공격
                cNormalAttack = StartCoroutine(ShotAttack());
            }
            else
            {
                DoSubdueCor();
                curNAttackCoolTime = 0;
            }

        }
    }


    IEnumerator ShotAttack()
    {
        isNormalAttack = true;
        curNAttackCoolTime = 0; 
        anim.SetBool("isShotOut", false);
        //anim.SetTrigger("doAimming");
        flashLens.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        isAimming = false;

        yield return new WaitForSeconds(0.1f);
        flashLens.SetActive(false);

        for (int i = 0; i < diff + 1; i++)
        {
            NormalAttack();
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            muzzleFlash.SetActive(false) ;
        }

        anim.SetBool("isShotOut", true);

        yield return new WaitForSeconds(0.1f);
        isNormalAttack = false;
        isAimming = true;

    }

    void NormalAttack()
    {
        RaycastHit hit;
        anim.SetTrigger("doShot");
        
        if (Physics.SphereCast(attackPos.position, 0.05f, attackPos.forward, out hit, 20, LayerMask.GetMask("Player")))
        {
            if (!IsObstacleBetween(attackPos.position, hit.transform.position, LayerMask.GetMask("Enviroment")))
            {
                //Debug.Log("Player Attack");
                Player player = hit.transform.gameObject.GetComponent<Player>();
                StartCoroutine(player.OnDamage(shotDamage));
            }
            else
            {
                //Debug.Log("Attack");
            }
        }
        else
        {
            //Debug.Log("Attack");
        }
    }

    IEnumerator FlashAttackReady()
    {
        anim.SetTrigger("doFlash");
        isSpecialAttack = true;
        //공격경로 표시

        for(int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.2f);
            flashLens.SetActive(!flashLens.activeSelf);
        }

        yield return new WaitForSeconds(0.2f);
        isAimming = false;

        flashlight.intensity = 2.5f;
        yield return new WaitForSeconds(1f);

        FlashAttack();
        flashlight.intensity = 0;

        yield return new WaitForSeconds(1f);
        anim.SetTrigger("doFlashOut");

        yield return new WaitForSeconds(1.5f);
        isAimming = true;
        curNAttackCoolTime = 0;
        isSpecialAttack = false;
    }

    void FlashAttack()
    {
        // SphereCast로 원뿔 모양의 레이캐스트 수행
        RaycastHit[] hits = Physics.SphereCastAll(attackPos.position, flashDistance, transform.forward, 0, LayerMask.GetMask("Player"));

        if (hits.Length > 0)
        {
            Vector3 dirToTarget = (hits[0].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < flashAngle / 2 && !IsObstacleBetween(attackPos.position, hits[0].transform.position, LayerMask.GetMask("Enviroment")))
            {
                Debug.Log("player stun");
                flashEffect.FlashBanged();
                Player playerObject = hits[0].collider.gameObject.GetComponent<Player>();
                playerObject.Stun(3f);
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


    public void Stun()
    {
        if (isStun)
        {
            anim.SetBool("isShotOut", true);
            isAimming = false;

            //스킬 코루틴 멈추기
            if (cNormalAttack != null)
            {
                StopCoroutine(cNormalAttack);
                cNormalAttack = null;
            }
            if (cFlashAttack != null)
            {
                StopCoroutine(cFlashAttack);
                cFlashAttack = null;
            }


            isNormalAttack = false;
            isSpecialAttack = false;
            curNAttackCoolTime = 0;
            curSAttackCoolTime = 0;

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

    public void ShotAnimEnd()
    {

    }
   
}

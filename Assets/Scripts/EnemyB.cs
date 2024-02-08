using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    public float moveDistance;
    public float stopDistance;

    public float nAttackCoolTime;
    public float sAttack1CoolTime;
    public float sAttack2CoolTime;

    void Update()
    {
        LookAtTarget();

        HackingUI();

        HackingCoolDown();

        Targeting();

        nav.speed = runSpeed;

        anim.SetFloat("speed", nav.speed);
    }

    void Targeting()
    {
        Vector3 playerTargeting = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 enemyPos = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(enemyPos, playerTargeting);

        if (distance <= stopDistance) // 플레이어와 멀어짐
        {
            nav.isStopped = false;
            Vector3 oppositeDirection = (transform.position - target.transform.position).normalized;
            nav.SetDestination(transform.position + oppositeDirection * 3.5f);
        }

        else if (distance < moveDistance && distance > stopDistance) // 멈춤
        {
            nav.isStopped = true;
        }

        else if (distance >= moveDistance) //플레이어에게 이동
        {
            nav.isStopped = false;
            nav.SetDestination(target.transform.position);
        }

    }

    void LookAtTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPosition);
        }
    }
}

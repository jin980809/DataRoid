using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject target;

    public NavMeshAgent nav;
    public Rigidbody rigid;
    public BoxCollider attackCollider;
    public Animator anim;
    public CapsuleCollider damageCollider;
    public Material gLight;
    public Material oLight;
    public Material rLight;
    public SkinnedMeshRenderer headLight;

    [Space(10f)]
    public bool isAttack = false;
    public bool isChase = false;
    public bool isShotChase = false;
    public bool isDeath = false;
    public bool isHit = false;

    [Space(10f)]
    public float maxHealth;
    public float curHealth;

    [Space(10f)]
    public float walkSpeed;
    public float runSpeed;
    public float chasingDistance;
    public float attackDistance;

    [Space(10f)]
    public Vector3 playerShotPos;

    [Serializable]
    public struct dropItem
    {
        public GameObject item;
        public float percentage;
    }
    public dropItem[] dropItems;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
    }

    public void OnDamage(float damage, Vector3 playerShotPos)
    {
        curHealth -= damage;

        if (curHealth > 0)
        {
            isHit = true;
            //anim.SetTrigger("doHit");
            //nav.isStopped = true;
            this.playerShotPos = playerShotPos;
            isShotChase = true;
            StartCoroutine(OnDamageOut());
        }
        else
        {
            this.gameObject.layer = 10;
            isDeath = true;
            nav.isStopped = true;
            anim.SetTrigger("doDie");
            Invoke("DropItems", 4);
            Destroy(gameObject, 4);
        }
    }

    public void SwitchLight()
    {
        if (curHealth / maxHealth > 0.5f)
        {
            headLight.material = gLight;
        }
        else if (curHealth / maxHealth > 0.3f)
        {
            headLight.material = oLight;
        }
        else
        {
            headLight.material = rLight;
        }
    }

    public void DropItems()
    {
        foreach (dropItem i in dropItems)
        {
            float dropPosX = UnityEngine.Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
            float dropPosZ = UnityEngine.Random.Range(transform.position.z - 0.5f, transform.position.z + 0.5f);

            Vector3 dropPos = new Vector3(dropPosX, transform.position.y, dropPosZ);

            if (DropPercentage(i.percentage))
            {
                Instantiate(i.item, dropPos, transform.rotation);
            }
        }
    }

    public static bool DropPercentage(float Chance)
    {
        if (Chance < 0.0000001f)
        {
            Chance = 0.0000001f;
        }

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }

    IEnumerator OnDamageOut()
    {
        yield return new WaitForSeconds(1f);
        //nav.isStopped = false;
        isHit = false;
    }
}
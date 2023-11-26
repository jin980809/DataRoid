using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    public enum Type { A, B, C, D };
    public Type enemyType;

    [Space(10f)]
    public Transform target;
    public Transform[] aroundTarget;

    [Space(10f)]
    public NavMeshAgent nav;
    public Rigidbody rigid;
    public Light eleclight;
    public BoxCollider attackCollider;

    [Space(10f)]
    public int maxHealth;
    public int curHealth;
    public float maxElectric = 0;
    public float curElectric = 0;
    public int detectRange;
    public float decreaseRate;

    //bool isDodge = false;
    bool isAttack = false;
    public bool isChase = false;

    [Space(10f)]
    public int aroundTargetIndex = 0;

    [Space(10f)]
    public GameObject curModule;
    public List<GameObject> detectList;

    private Animator anim;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        eleclight = GetComponentInChildren<Light>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (nav.enabled)
        {
            if (Vector3.Distance(transform.position, target.position) <= detectRange && !isAttack)
            {
                Chase();
            }
            else
            {
                isChase = false;
            }

            nav.SetDestination(target.position);

            if (enemyType == Type.C)
            {
                Around();
            }
            else
            {
                nav.isStopped = !isChase;
            }
            
        }
        else
        { 
            //isDodge = false;
        }

        LightChange();

        if (enemyType == Type.C)
        {
            Around();
        }

        anim.SetFloat("speed", nav.speed);
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Around()
    {
        nav.SetDestination(aroundTarget[aroundTargetIndex].position);

        if ((transform.position.x == aroundTarget[aroundTargetIndex].position.x) &&
            (transform.position.z == aroundTarget[aroundTargetIndex].position.z))
        {

            Debug.Log("dectect" + aroundTarget[aroundTargetIndex].name);
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

    void Chase()
    {
        switch (enemyType)
        {
            case Type.A:
                isChase = true;
                break;

            case Type.B:
            case Type.C:
                isChase = true;
                //if (!isDodge)
                //{
                //    rigid.AddForce(target.forward * -50, ForceMode.Impulse);
                //    isDodge = true;
                //}
                break;
        }
    }

    void LightChange()
    {
        float elePerc = curElectric / maxElectric;
        curElectric -= decreaseRate * Time.deltaTime;

        if (elePerc > 0.15f)
        {
            eleclight.color = Color.green;
        }
        else if (elePerc <= 0.15f && elePerc > 0.05f)
        {
            eleclight.color = new Color(1, 0.5f, 0);
        }
        else
        {
            eleclight.color = Color.red;
        }

        //Debug.Log(elePerc);
    }

    void Targeting()
    {
        if (/*!isDead &&*/ enemyType != Type.D)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 0.25f;
                    targetRange = 0.5f;

                    break;

                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;

                    break;

                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        //anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(1f);
                attackCollider.enabled = true;

                yield return new WaitForSeconds(1f);
                attackCollider.enabled = false;

                yield return new WaitForSeconds(1f);
                break;

            case Type.B:
                break;

            case Type.C:
                break;
        }

        isChase = true;
        isAttack = false;
        //anim.SetBool("isAttack", false);
        yield return null;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Device")
        {
            detectList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Device")
        {
            detectList.Remove(other.gameObject);
        }

        if(other.gameObject == curModule)
        {
            StartCoroutine("ReSearch");
        }
    }


    IEnumerator ReSearch()
    {
        isChase = false;

        yield return new WaitForSeconds(2f);

        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in detectList)
        {
            if (obj == null)
            {
                continue;
            }

            Vector3 objectPosition = obj.transform.position;
            float distance = Vector3.Distance(transform.position, objectPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                curModule = obj;
            }
        }

        isChase = true;

    }
}

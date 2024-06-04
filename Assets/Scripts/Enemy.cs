using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject target;

    public NavMeshAgent nav;
    public Rigidbody rigid;
    public BoxCollider L_attackCollider;
    public BoxCollider R_attackCollider;
    public Animator anim;
    public CapsuleCollider damageCollider;

    [Space(10f)]
    public bool isAttack = false;
    public bool isChase = false;
    public bool isShotChase = false;
    public bool isDeath = false;
    public bool isHit = false;
    public bool isHacking = false;
    public bool isStun = false;

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

    [Space(10f)]
    public string modelName;
    public string iMEI;
    public int ID;
    public float sheild;
    public float maxSheild;

    [Serializable]
    public struct dropItem
    {
        public GameObject item;
        public float percentage;
    }
    public dropItem[] dropItems;

    //public GameObject enemyPrefab; // ���������� ����� �� UI
    //public Canvas canvas; // UI�� ���� ĵ����
    //public GameObject enemyUI; // ������ �� UI
    public float rotateRate = 1;

    [Space(10f)]
    [Header("Parts")]
    //public Transform[] parts;
    private bool[] isHitStackDone = new bool[6];
    private int[] partsHitStack = new int[6];
    public GameObject[] partsEffect;
    //private Slider sheildUI;
    //private Slider hackingDurationUI;
    //private float curHackingDuration;
    //public float hackingDuration;

    [Space(10f)]
    public Coroutine attackCoroutine;
    public ParticleSystem cps;
    public bool isMeleeDamage;

    [Space(10f)]
    [Header("Subdue")]
    public bool isPlayerSubdue = false;
    public BoxCollider subdueCol;
    protected bool isParryingPossible = false;
    public float parryingTime;
    protected bool isSubdueReady = true;

    [Space(10f)]
    [Header("Data")]
    public bool getData = false;

    [Space(10f)]
    [Header("Death to OnOff")]
    public GameObject[] onObject;
    public GameObject[] offObject;

    public Coroutine DoSubdue = null;
    public float subdueCoolTime;
    float curSubdueCoolTime;
    public float subdueProgress;
    public float curSubdueProgress;
    public float speedDiscountRate = 1;

    public Coroutine bombCor = null;
    public bool isA = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public enum Type
    {
        Head = 0,
        Body = 1,
        LeftArm = 2,
        RightArm = 3,
        LeftLeg = 4,
        RightLeg = 5
    }

    public Type hitAreaType;

    void Start()
    {
        //curHealth = maxHealth;
        //rectTransform = enemyPrefab.GetComponent<RectTransform>();
        EnemyInitialization();
        curSubdueProgress = subdueProgress;
    }

    public void Stun(bool parrying, float time)
    {
        //�ִϸ��̼� ����
        isStun = true;
        if(parrying)
        {
            anim.SetTrigger("doParryingDown");
        }
        else
        {
            anim.SetBool("isStun", true);
        }
        
        anim.SetBool("isAttack", false);
        nav.isStopped = true;
        isAttack = false;
        isChase = false;
        isShotChase = false;
        L_attackCollider.enabled = false;
        R_attackCollider.enabled = false;
        StartCoroutine(StunOut(parrying, time));
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        //if(isSkill)
        //{
        //    Vector3 enemyDir = new Vector3(transform.position.x, 0, transform.position.z);
        //    Vector3 playerDir = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        //    Vector3 KnockBackPos = transform.position + (enemyDir - playerDir) * 5f;
        //    transform.position = Vector3.Lerp(transform.position, KnockBackPos, 10 * Time.deltaTime);
        //}
    }

    IEnumerator StunOut(bool parrying, float time)
    {
        if (parrying)
        {
            anim.SetTrigger("doParryingDownOut");
        }
        //�ִϸ��̼� ����
        yield return new WaitForSeconds(time);
        if (!parrying)
        {
            anim.SetBool("isStun", false);
        }
        nav.isStopped = false;
        isStun = false;
    }

    void EnemyInitialization()
    {
        if (int.Parse(EnemyManager.Instance.enemyInfo[ID]["isDead"] + "") == 1)
        {
            this.gameObject.SetActive(false);
            isDeath = true;
        }

        maxHealth = float.Parse(EnemyManager.Instance.enemyInfo[ID]["HP"] + "");
        curHealth = maxHealth;
        maxSheild = float.Parse(EnemyManager.Instance.enemyInfo[ID]["Sheild"] + "");
        sheild = maxSheild;
        L_attackCollider.gameObject.GetComponent<Attack>().damage = int.Parse(EnemyManager.Instance.enemyInfo[ID]["Damage"] + "");
        R_attackCollider.gameObject.GetComponent<Attack>().damage = int.Parse(EnemyManager.Instance.enemyInfo[ID]["Damage"] + "");
    }

    public void HitEffect(Vector3 spawnPosition)
    {
        //cps.transform.position = spawnPosition;
        //cps.Emit(1);
    }

    void StackHitArea(int hitArea, float damage)
    {
        if (!isDeath)
        {
            switch (hitArea)
            {
                case -1:
                    break;

                case 0:
                    curHealth /= 2f;
                    partsEffect[0].SetActive(true);
                    Debug.Log("Head 5");
                    break;
                case 1:
                    curHealth -= damage * 4;
                    partsEffect[1].SetActive(true);
                    Debug.Log("Body 5");
                    break;
                case 2:
                    L_attackCollider.gameObject.GetComponent<Attack>().curDamage -= GetComponentInChildren<Attack>().damage * 0.25f;
                    partsEffect[2].SetActive(true);
                    Debug.Log("L Arm 5");
                    break;
                case 3:
                    R_attackCollider.gameObject.GetComponent<Attack>().curDamage -= GetComponentInChildren<Attack>().damage * 0.25f;
                    partsEffect[3].SetActive(true);
                    Debug.Log("R Arm 5");
                    break;
                case 4:
                    speedDiscountRate -= 0.25f;
                    partsEffect[4].SetActive(true);
                    Debug.Log("L Leg 5");
                    break;
                case 5:
                    speedDiscountRate -= 0.25f;
                    partsEffect[5].SetActive(true);
                    Debug.Log("R Leg 5");
                    break;
            }
        }
    }

    public void OnDamage(float damage, Vector3 playerShotPos, int hitArea)
    {
        Debug.Log(hitArea);

        //if (isHacking && hitArea == (int)hitAreaType)
        //{
        //    Debug.Log("Critical Hit");
        //    curHealth -= damage;
        //    sheild -= sheildDamage;

        //    if (sheild <= 0)
        //    {
        //        Destroy(enemyUI);
        //        Stun(false, 3f);
        //        isHacking = false;
        //    }
        //}
        //else
        //{
        //    curHealth -= damage;
        //}
        curHealth -= damage;

        if (hitArea != -1)
        {
            if (!isHitStackDone[hitArea])
            {
                partsHitStack[hitArea]++;

                if (partsHitStack[hitArea] >= 5)
                {
                    isHitStackDone[hitArea] = true;
                    StackHitArea(hitArea, damage);
                }
            }
        }

        if (curHealth > 0)
        {
            //isHit = true;
            //anim.SetTrigger("doHit");
            //nav.isStopped = true;

            this.playerShotPos = playerShotPos;
            isShotChase = true;

            StartCoroutine(OnDamageOut());
        }
        else
        {
            EnemyManager.Instance.enemyInfo[ID]["isDead"] = "1";

            //if (hasData)
            //    ProgressManager.Instance.curData += getDataAmount;

            this.gameObject.layer = 10;
            isDeath = true;
            nav.isStopped = true;
            nav.speed = 0;
            anim.SetTrigger("doDie");
            DropItems();
            OnOffObject();
            StartCoroutine(Dead());
        }
    }

    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }

    IEnumerator OnDamageOut()
    {
        yield return new WaitForSeconds(0.01f);
        //nav.isStopped = false;
        isHit = false;
    }


    public void DropItems()
    {
        for(int i = 0; i < dropItems.Length; i++)
        {
            Vector3 dropPos = transform.position;

            if (RandPercentage(dropItems[i].percentage))
            {
                Instantiate(dropItems[i].item, dropPos, transform.rotation);
            }
        }
    }

    public static bool RandPercentage(float Chance)
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

    //public void HackingUI()
    //{
    //    if (isHacking)
    //    {
    //        //Debug.Log(IsEnemyVisible());
    //        if (IsEnemyVisible())
    //        {
    //            UpdateUIPosition();
    //            UpdateUIScale();
    //            if (enemyUI != null)
    //                enemyUI.SetActive(true);
    //        }
    //        else
    //        {
    //            enemyUI.SetActive(false);
    //        }

    //    }
    //    else
    //    {
    //        if (enemyUI != null)
    //        {
    //            enemyUI.SetActive(false);
    //        }
    //    }
    //}

    //public void HackingCoolDown()
    //{
    //    if (isHacking)
    //    {
    //        curHackingDuration -= Time.deltaTime;
    //        hackingDurationUI.value = curHackingDuration / hackingDuration;
    //        sheildUI.value = sheild / maxSheild;
    //    }

    //    if (curHackingDuration <= 0)
    //    {
    //        isHacking = false;
    //        curHackingDuration = hackingDuration;
    //    }
    //}

    //public void CreateEnemyUI()
    //{
    //    if (enemyUI == null)
    //    {
    //        enemyUI = Instantiate(enemyPrefab, canvas.transform);
    //        sheildUI = enemyUI.transform.GetChild(0).GetComponent<Slider>();
    //        hackingDurationUI = enemyUI.transform.GetChild(1).GetComponent<Slider>();
    //    }
    //    else
    //    {
    //        enemyUI.SetActive(true);
    //    }
    //}

    //bool IsEnemyVisible()
    //{
    //    Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);
    //    //Debug.Log(screenPos);
    //    return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
    //}

    //void UpdateUIPosition()
    //{
    //    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

    //    switch (hitAreaType)
    //    {
    //        case Type.Head:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[0].position);
    //            break;
    //        case Type.Body:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[1].position);
    //            break;
    //        case Type.LeftArm:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[2].position);
    //            break;
    //        case Type.RightArm:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[3].position);
    //            break;
    //        case Type.LeftLeg:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[4].position);
    //            break;
    //        case Type.RightLeg:
    //            screenPos = Camera.main.WorldToScreenPoint(parts[5].position);
    //            break;
    //    }

    //    //if (enemyUI != null)
    //    //{
    //    //    Vector3 canvasPos = screenPos / canvas.scaleFactor;
    //    //    enemyUI.transform.position = canvasPos;
    //    //}

    //}

    //void UpdateUIScale()
    //{
    //    float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
    //    float scaleFactor = Mathf.Clamp(10f / distance, 0.5f, 2f);

    //    if (enemyUI != null)
    //        enemyUI.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    //}

    public void RotationSpeedUp()
    {
        Vector3 lookrotation = nav.steeringTarget - transform.position;
        if (lookrotation != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime * rotateRate);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            if (!isMeleeDamage && !target.GetComponent<Player>().isSubdue)
            {
                //UIManager.Instance.SubDueSlider.gameObject.SetActive(true);
                isMeleeDamage = true;
                Attack attack = other.GetComponent<Attack>();
                OnDamage(attack.curDamage, other.transform.position, -1);
                StartCoroutine(MeleeDamageOut());
                //anim.SetTrigger("doSubdueOut");
            }
        }
    }

    protected IEnumerator MeleeDamageOut()
    {
        yield return new WaitForSeconds(0.3f);
        isMeleeDamage = false;
    }

    public void DoSubdueCor()
    {
        DoSubdue = StartCoroutine(SubdueReady());
    }

    IEnumerator SubdueReady()
    {
        anim.SetTrigger("doSubdueReady");
        isSubdueReady = false;
        isParryingPossible = true;
        nav.isStopped = true;

        yield return new WaitForSeconds(parryingTime);
        subdueCol.enabled = true;
        //Debug.Log("SubdueAttack");
        isParryingPossible = false;

        yield return new WaitForSeconds(0.2f);
        //Debug.Log("AA");
        anim.SetTrigger("doSubdueOut");
        subdueCol.enabled = false;
        yield return new WaitForSeconds(0.7f);
        isSubdueReady = true;
        nav.isStopped = false;
    }

    public void Subdue()
    {
        if (isPlayerSubdue)
        {
            subdueCol.enabled = false;
            target.GetComponent<Player>().SubdueObject = transform.gameObject;
            isShotChase = false;
            anim.SetTrigger("doSubdue");
            if (DoSubdue != null)
            {
                StopCoroutine(DoSubdue);
                DoSubdue = null;
            }
            //isPlayerSubdue = true;

            transform.LookAt(target.transform.position);
            //player.transform.LookAt(transform.position);
            //isSubdueReady = true;

            nav.isStopped = true;
            nav.velocity = Vector3.zero;

            //curSubdueCoolTime += Time.deltaTime;
            //curSubdueProgress += Time.deltaTime;
            UIManager.Instance.SubDueSlider.value = curSubdueProgress / subdueProgress;

            //if (curSubdueCoolTime > subdueCoolTime)
            //{
            //    //���� ���н� (�ð��� �� ����������)
            //    target.GetComponent<Player>().isSubdue = false;
            //    //UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
            //    //ProgressManager.Instance.curProgress -= 1;
            //    anim.SetTrigger("doSubdueOut");
            //    curSubdueCoolTime = 0;
            //    curSubdueProgress = 0;
            //    Invoke("SubdueOut", 1.5f);
            //    isPlayerSubdue = false;
            //}

            if (curSubdueProgress <= 0)
            {
                anim.SetTrigger("doSubdueOut");
                //UIManager.Instance.SubDueSlider.gameObject.SetActive(false);
                //ProgressManager.Instance.curProgress -= 1;
                target.GetComponent<Player>().isSubdue = false;
                //curSubdueCoolTime = 0;
                Invoke("SubdueOut", 1.5f);
                isPlayerSubdue = false;
                curSubdueProgress = subdueProgress;
            }
        }
    }

    public void Parrying()
    {
        if (isParryingPossible && isMeleeDamage)
        {
            Debug.Log("Parrying");
            StopCoroutine(DoSubdue);
            isParryingPossible = false;
            nav.isStopped = false;
            isSubdueReady = true;
            Stun(true, 5f);
        }
    }

    public void SubdueOut()
    {
        //UIManager.Instance.SubDueSlider.gameObject.SetActive(false);

        UIManager.Instance.SubDueSlider.value = 0;
        nav.isStopped = false;
        isSubdueReady = true;
    }

    public void OnOffObject()
    {
        for(int i = 0; i < onObject.Length; i++)
        {
            onObject[i].SetActive(true);
        }
        for(int i = 0; i < offObject.Length; i++)
        {
            onObject[i].SetActive(false);
        }
    }
}
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


    [Space(10f)]
    public GameObject enemyPrefab; // 프리팹으로 사용할 적 UI
    public Canvas canvas; // UI가 속한 캔버스
    public GameObject enemyUI; // 생성된 적 UI
    RectTransform rectTransform; // RectTransform 컴포넌트
    public Camera mainCamera;
    public Transform[] parts;
    private Slider sheildUI;
    private Slider hackingDurationUI;
    private float curHackingDuration;
    public float hackingDuration;
    public Coroutine attackCoroutine;
    public ParticleSystem cps;
    public bool hasData;
    public bool isMeleeDamage;
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
        rectTransform = enemyPrefab.GetComponent<RectTransform>();
        EnemyInitialization();
    }

    public void Stun(bool parrying)
    {
        //애니메이션 실행
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
        attackCollider.enabled = false;
        StartCoroutine(StunOut(parrying));
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

    IEnumerator StunOut(bool parrying)
    {
        if (parrying)
        {
            anim.SetTrigger("doParryingDownOut");
        }
        //애니메이션 실행
        yield return new WaitForSeconds(5f);
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
        attackCollider.gameObject.GetComponent<Attack>().damage = int.Parse(EnemyManager.Instance.enemyInfo[ID]["Damage"] + "");
    }

    public void HitEffect(Vector3 spawnPosition)
    {
        cps.transform.position = spawnPosition;
        cps.Emit(1);
    }

    public void OnDamage(float damage, Vector3 playerShotPos, int hitArea, float sheildDamage)
    {
        if (isHacking && hitArea == (int)hitAreaType)
        {
            Debug.Log("Critical Hit");
            curHealth -= damage;
            sheild -= sheildDamage;

            if (sheild == 0)
            {
                Destroy(enemyUI);
                Stun(false);
                isHacking = false;
            }
        }
        else
        {
            curHealth -= damage;
        }

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
            EnemyManager.Instance.enemyInfo[ID]["isDead"] = "1";
            this.gameObject.layer = 10;
            isDeath = true;
            nav.isStopped = true;
            anim.SetTrigger("doDie");
            //Invoke("DropItems", 4);
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
        if (hasData)
            MaterialManager.Instance.UFSData += 1;
    }

    IEnumerator OnDamageOut()
    {
        yield return new WaitForSeconds(0.01f);
        //nav.isStopped = false;
        isHit = false;
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

            if (RandPercentage(i.percentage))
            {
                Instantiate(i.item, dropPos, transform.rotation);
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

    public void HackingUI()
    {
        if (isHacking)
        {
            //Debug.Log(IsEnemyVisible());
            if (IsEnemyVisible())
            {
                UpdateUIPosition();
                UpdateUIScale();
                if (enemyUI != null)
                    enemyUI.SetActive(true);
            }
            else
            {
                enemyUI.SetActive(false);
            }

        }
        else
        {
            if (enemyUI != null)
            {
                enemyUI.SetActive(false);
            }
        }
    }

    public void HackingCoolDown()
    {
        if (isHacking)
        {
            curHackingDuration -= Time.deltaTime;
            hackingDurationUI.value = curHackingDuration / hackingDuration;
            sheildUI.value = sheild / maxSheild;
        }

        if (curHackingDuration <= 0)
        {
            isHacking = false;
            curHackingDuration = hackingDuration;
        }
    }

    public void CreateEnemyUI()
    {
        if (enemyUI == null)
        {
            enemyUI = Instantiate(enemyPrefab, canvas.transform);
            sheildUI = enemyUI.transform.GetChild(0).GetComponent<Slider>();
            hackingDurationUI = enemyUI.transform.GetChild(1).GetComponent<Slider>();
        }
        else
        {
            enemyUI.SetActive(true);
        }
    }

    bool IsEnemyVisible()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);
        //Debug.Log(screenPos);
        return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
    }

    void UpdateUIPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        switch (hitAreaType)
        {
            case Type.Head:
                screenPos = Camera.main.WorldToScreenPoint(parts[0].position);
                break;
            case Type.Body:
                screenPos = Camera.main.WorldToScreenPoint(parts[1].position);
                break;
            case Type.LeftArm:
                screenPos = Camera.main.WorldToScreenPoint(parts[2].position);
                break;
            case Type.RightArm:
                screenPos = Camera.main.WorldToScreenPoint(parts[3].position);
                break;
            case Type.LeftLeg:
                screenPos = Camera.main.WorldToScreenPoint(parts[4].position);
                break;
            case Type.RightLeg:
                screenPos = Camera.main.WorldToScreenPoint(parts[5].position);
                break;
        }

        if (enemyUI != null)
        {
            Vector3 canvasPos = screenPos / canvas.scaleFactor;
            enemyUI.transform.position = canvasPos;
        }

    }

    void UpdateUIScale()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        float scaleFactor = Mathf.Clamp(10f / distance, 0.5f, 2f);

        if (enemyUI != null)
            enemyUI.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

    public void RotationSpeedUp()
    {
        Vector3 lookrotation = nav.steeringTarget - transform.position;
        if (lookrotation != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 5 * Time.deltaTime);
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
                OnDamage(attack.damage, other.transform.position, -1, other.GetComponentInParent<Player>().sheildDamage);
                StartCoroutine(MeleeDamageOut());
                anim.SetTrigger("doSubdueOut");
            }
        }
    }

    IEnumerator MeleeDamageOut()
    {
        yield return new WaitForSeconds(0.3f);
        isMeleeDamage = false;
    }
}
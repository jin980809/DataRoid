using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform ShootPos;
    public Weapon weapon;
    public Transform borderPos;
    public ParticleSystem flashEffect;
    public GameObject hitEffect;
    public GameObject flashLight;

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public int equipWeaponIndex;
    private int lastSwapWeaponIndex = 0;

    [Space(10)]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float dodgeSpeed;
    public float gunWalkSpeed;
    public float gunRunSpeed;

    [Space(10)]
    public float maxHp;
    public float curHp;

    [Space(10)]
    public float dodgeCoolTime;
    public float hackingTime;
    private float targetSpeed;
    private float culDodgeTime;

    float hAxis;
    float vAxis;
    bool rDown; // 뛰기
    bool sDown; // 앉기
    bool fDown; // 공격
    bool f2Down; // 해킹
    bool dDown; // 구르기
    bool gDown; // 총 꺼내고 넣기
    bool lDown; // 재장전
    bool aDown; // 상호작용
    bool iDown; // 인벤토리
    bool eDown; // 손전등
    bool qDown; // 취소
    bool skDown1; // 스킬1
    bool mDown; // 스킬1

    bool sDown1;
    bool sDown2;
    bool sDown3;

    [Space(10)]
    public bool isDamage = false;
    public bool isWalk;
    public bool isRun;
    bool isFireReady = true;
    bool isBorder;
    public bool isDodge = false;
    private bool isDodgeReady = true;
    private bool isGunOn = false;
    public bool isShot = false;
    public bool isReload = false;
    public bool isSwap = false;
    public bool isInteraction = false;
    public bool isInventoryOpen = false;
    public bool isFlashLightOn = false;
    public bool isCreaingUIOpen = false;
    public bool isHacking = false;
    public bool isZoom = false;
    bool isSemiReady = true;
    bool isMapOpen = false;
    public bool isShotEnd = true;

    [SerializeField] private float shotDeley;
    private float rotationVelocity;
    private float _animationBlend;
    private float verBlend;
    private float horBlend;
    private float fireDelay;
    private float interactionTime;
    private float curHackingTime = 0f;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 gunOnVec;
    Vector3 targetDirection;

    Animator anim;

    public GameObject _mainCamera;
    public CameraMove cameraArm;
    Rigidbody rigid;
    RectTransform miniMap;
    [SerializeField] private GameObject hackingEnemyInfo;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        miniMap = UIManager.Instance.MiniMap.GetComponent<RectTransform>();
    }

    void Update()
    {
        GetInput();

        Move();

        GunOnMove();

        Shot();

        Dodge();

        Reload();

        Interaction();

        InteractionCheck();

        OCInventory();

        FlashLightOnOff();

        EnemyHacking();

        Swap();

        ZoomInOut();

        OCMap();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        sDown = Input.GetButton("Crouch");
        fDown = Input.GetButton("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
        dDown = Input.GetButton("Dodge");
        gDown = Input.GetButtonDown("GunOn");
        lDown = Input.GetButton("Reload");
        aDown = Input.GetButton("Interaction");
        iDown = Input.GetButtonDown("Inventory");
        eDown = Input.GetButtonDown("FlashLight");
        qDown = Input.GetButtonDown("Cancel");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        skDown1 = Input.GetButton("Skill1");
        mDown = Input.GetButtonDown("Map");
    }

    void Move()
    {
        if (!isDodge && !isInteraction)
        {
            if(isGunOn)
            {
                if (rDown && !isReload && isShotEnd ) targetSpeed = gunRunSpeed;
                else targetSpeed = gunWalkSpeed;
            }
            else
            {
                if (sDown) targetSpeed = crouchSpeed;
                else if (rDown) targetSpeed = runSpeed;
                else targetSpeed = walkSpeed;
            }

            isRun = (rDown && !sDown);
        }
        if (isDodge && !isInteraction)
        {
            targetSpeed = dodgeSpeed;
            moveVec = dodgeVec;
        }

        moveVec = new Vector3(hAxis, 0f, vAxis).normalized;
        if (moveVec == Vector3.zero && !isDodge) { targetSpeed = 0.0f;/* crouchSpeed = 0;*/ }

        float targetRotation = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

        if (moveVec != Vector3.zero)
        {
            isWalk = true;

            if (!isDodge && !isInteraction)
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, 0.12f);

                transform.rotation = Quaternion.Euler(0.0f, isGunOn ? _mainCamera.transform.eulerAngles.y : rotation, 0.0f);
            }
            else
            {
                if (isDodgeReady && !isInteraction)
                    transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f); //도약할때 바로 캐릭터가 회전하게
            }
        }
        else
        {
            isWalk = false;

            if (isGunOn && !isDodge && !isInteraction)
                transform.rotation = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f);
        }


        if (!isDodge)
            targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        //애니메이션 자연스럽게 이어주기 위함
        _animationBlend = Mathf.Lerp(_animationBlend, /*sDown ? crouchSpeed :*/ targetSpeed, Time.deltaTime * 5);
        verBlend = Mathf.Lerp(verBlend, vAxis, Time.deltaTime * 3);
        horBlend = Mathf.Lerp(horBlend, hAxis, Time.deltaTime * 3);

        if (_animationBlend < 0.01f) _animationBlend = 0f;
        if (moveVec == Vector3.zero)
        {
            if (verBlend < 0.01f) verBlend = 0f;
            if (horBlend < 0.01f) horBlend = 0f;
        }

        if (!isBorder)
            transform.position += targetDirection.normalized * (/*sDown ? crouchSpeed : */targetSpeed) * Time.deltaTime;

        anim.SetBool("isCrouch", sDown);
        anim.SetFloat("speed", _animationBlend);
        anim.SetBool("isGunOn", isGunOn);
        anim.SetFloat("Vertical", verBlend);
        anim.SetFloat("Horizental", horBlend);
    }

    void Dodge()
    {
        culDodgeTime += Time.deltaTime;
        isDodgeReady = dodgeCoolTime < culDodgeTime;

        if (!isDodge && dDown && isDodgeReady && !isShot && isWalk && !isReload && !isInteraction && !isInventoryOpen && !isHacking)
        {
            isDodge = true;
            culDodgeTime = 0f;
            dodgeVec = moveVec;
            //targetSpeed *= 4;
            anim.SetTrigger("doDodge");
            Invoke("DodgeOut", 1.3f);

        }
    }

    void DodgeOut()
    {
        //targetSpeed *= 0.25f;
        isDodge = false;
    }

    void GunOnMove()
    {
        UIManager.Instance.aim.SetActive(isGunOn);
        if (gDown && !isReload && !isInteraction && !isInventoryOpen && !isDodge && !isInteraction && !isInventoryOpen && !isHacking)
        {
            isGunOn = !isGunOn;
            weapon.gameObject.SetActive(isGunOn);

            if (!isGunOn)
            {
                isZoom = false;
                lastSwapWeaponIndex = equipWeaponIndex;
                equipWeaponIndex = -1;
                
            }
            else
            {
                equipWeaponIndex = lastSwapWeaponIndex;
            }

        }

        anim.SetInteger("weaponIndex", equipWeaponIndex);
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        if ((sDown1 || sDown2 || sDown3) && !isSwap  && !isShot && !isDodge && !isReload && !isInteraction && !isHacking && !isInventoryOpen)
        {
            isGunOn = true;
            isZoom = false;
            int weaponIndex = -1;

            if (sDown1) { weaponIndex = 0; lastSwapWeaponIndex = 0; }
            if (sDown2) { weaponIndex = 1; lastSwapWeaponIndex = 1; }
            if (sDown3) { weaponIndex = 2; lastSwapWeaponIndex = 2; }

            if (weapon != null)
                weapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            weapon = weapons[weaponIndex].GetComponent<Weapon>();
            weapon.gameObject.SetActive(true);

            //anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Shot()
    {
        fireDelay += Time.deltaTime;
        isFireReady = (weapon.delay < fireDelay) && (weapon.isSemiauto ? isSemiReady : true);

        isShot = fDown && !isDodge && isGunOn && weapon.curAmmo > 0 && !isReload && !isInteraction && !isInventoryOpen && !isHacking;

        if (fDown && isFireReady && !isDodge && isGunOn && weapon.curAmmo > 0 && !isReload && !isInteraction && !isInventoryOpen && !isHacking)
        {
            isShotEnd = false;
            fireDelay = 0;
            RaycastHit hit;
            flashEffect.Play();
            anim.SetTrigger("doShot");
            weapon.curAmmo -= 1;
            Vector3 playerShotPos = ShootPos.position;

            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 20, ~LayerMask.GetMask("PhysicsEnemy")))
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                SpawnHitEffect(hit.point);

                if (isEnemyHitArea(hit.collider.transform.gameObject) && !enemy.isDeath)
                {
                    Debug.Log(hit.collider.transform.tag);

                    enemy.OnDamage(weapon.damage, playerShotPos, hitArea(hit.collider.transform.gameObject));
                }
            }
            else if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 20, LayerMask.GetMask("Enviroment")))
            {
                SpawnHitEffect(hit.point);
            }

            isSemiReady = false;
        }

        if(!isShot)
        {
            isSemiReady = true;
        }
        
        anim.SetBool("isShotOut", !isShotEnd && !isDodge);
    }

    void ZoomInOut()
    {
        if(f2Down && !isInteraction && !isReload && !isInventoryOpen && !isHacking && !isCreaingUIOpen && !isSwap && isGunOn)
        {
            isZoom = !isZoom;
        }
    }

    bool isEnemyHitArea(GameObject obj)
    {
        if(obj.CompareTag("Enemy") || obj.CompareTag("EnemyHead") || obj.CompareTag("EnemyBody") || obj.CompareTag("EnemyLeftArm") ||
            obj.CompareTag("EnemyRightArm") || obj.CompareTag("EnemyLeftLeg") || obj.CompareTag("EnemyRightLeg"))
        {
            return true;
        }

        return false;
    }

    int hitArea(GameObject obj)
    {
        switch(obj.tag)
        {
            case "EnemyHead":
                return 0;
            case "EnemyBody":
                return 1;
            case "EnemyLeftArm":
                return 2;
            case "EnemyRightArm":
                return 3;
            case "EnemyLeftLeg":
                return 4;
            case "EnemyRightLeg":
                return 5;
        }

        return -1;
    }

    void SpawnHitEffect(Vector3 spawnPosition)
    {
        Quaternion enemyRotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
        GameObject hitObj = Instantiate(hitEffect, spawnPosition, enemyRotation);
        hitObj.GetComponent<ParticleSystem>().Play();
    }

    void Reload()
    {
        if (lDown && weapon.curAmmo < weapon.maxAmmo && isGunOn && !isShot && !isReload && !isDodge && !isInteraction && MaterialManager.Instance.Ammo > 0 && !isInventoryOpen && !isHacking) // 가지고 있는 총알 개수가 0 이하가 아니면 추가
        {
            isReload = true;
            isZoom = false;
            //isZoom = false;
            anim.SetTrigger("doReload");
            StartCoroutine(ReloadOut());
        }
    }

    IEnumerator ReloadOut()
    {
        yield return new WaitForSeconds(2.5f);

        if (MaterialManager.Instance.Ammo < weapon.maxAmmo - weapon.curAmmo)
        {
            weapon.curAmmo += MaterialManager.Instance.Ammo;
            MaterialManager.Instance.Ammo = 0;
        }
        else
        {
            MaterialManager.Instance.Ammo -= (weapon.maxAmmo - weapon.curAmmo);
            weapon.curAmmo = weapon.maxAmmo;

        }

        //가지고 있는 총알개수가 maxAmmo보다 적으면 가지고 있는 총알 개수만 curAmmo에 장전
        yield return new WaitForSeconds(0.5f);
        isReload = false;
    }

    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        //Debug.DrawLine(ShootPos.position, transform.forward , Color.green);
        int layerMask = (1 << LayerMask.NameToLayer("Enviroment")) + (1 << LayerMask.NameToLayer("Door")) + (1 << LayerMask.NameToLayer("Enemy"));

        if (Physics.Raycast(borderPos.position, targetDirection, 0.7f, layerMask))
        {
            isBorder = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            isBorder = false;
            GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            if (!isDamage)
            {
                Attack attack = other.GetComponent<Attack>();
                curHp -= attack.damage;
                //Debug.Log(curHp);

                StartCoroutine(OnDamage(new Vector3(other.transform.position.x, 0.5f, other.transform.position.z)));
            }
        }
    }

    IEnumerator OnDamage(Vector3 enemyPos)
    {
        isDamage = true;
        Vector3 playerDir = new Vector3(transform.position.x, 0.5f, transform.position.z);


        yield return new WaitForSeconds(0.5f);
        //rigid.AddForce((playerDir - enemyPos).normalized * 50, ForceMode.Impulse);
        anim.SetTrigger("Hit");

        yield return new WaitForSeconds(0.5f);
        isDamage = false;
        yield return null;
    }

    void Interaction()
    {
        RaycastHit hit;
        Interaction interactionObj;

        if (Physics.Raycast(ShootPos.position, transform.forward, out hit, 1, LayerMask.GetMask("Interaction")))
        {
            interactionObj = hit.transform.GetComponent<Interaction>();

            if (aDown && !isShot && !isDodge && !isReload && !interactionObj.isCoolTime && !isHacking && !isInventoryOpen)
            {
                isInteraction = true;
                isZoom = false;
            }
            else
            {
                interactionTime = 0;
                isInteraction = false;

                //if (interactionObj.isCoolTime)
                //    Debug.Log("CoolTime");
            }

            if (isInteraction)
            {
                interactionTime += Time.deltaTime;

                moveVec = Vector3.zero;
                //Debug.Log(interactionObj.name + " " + interactionTime);

                if (interactionTime >= interactionObj.interactionTime)
                {
                    interactionObj.InteractionResult();
                    interactionTime = 0;
                    interactionObj.isCoolTime = true;
                    isInteraction = false;
                    //Debug.Log("end");
                }

                if (isDamage)
                {
                    interactionTime = 0;
                    isInteraction = false;
                }
            }

            UIManager.Instance.InteractionGauge.value = interactionTime / interactionObj.interactionTime;
        }
    }

    void InteractionCheck()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0f, LayerMask.GetMask("Interaction"));
        int count = 0;
        foreach (RaycastHit i in hit)
        {
            if (!IsObstacleBetween(transform.position, i.transform.position, LayerMask.GetMask("Enviroment")))
            {
                count++;
            }
        }
        UIManager.Instance.detectCount.text = "Interaction Detect : " + count;
    }

    void EnemyHacking()
    {
        RaycastHit hit1;
        //RaycastHit hit2;

        if (skDown1 && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen)
        {
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit1, 20) && hackingEnemyInfo == null && isEnemyHitArea(hit1.transform.gameObject))
            {
                hackingEnemyInfo = hit1.transform.gameObject;
                isHacking = true;
            }

            //if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit2, 20) && hit1.transform.CompareTag("Enemy"))
            //{
            //    if (hit2.transform.gameObject == hackingEnemyInfo)
            //    {
            //        isHacking = true;
            //    }
            //    else
            //    {
            //        isHacking = false;
            //        hackingEnemyInfo = null;
            //    }
            //}
            //else
            //{
            //    isHacking = false;
            //    hackingEnemyInfo = null;
            //}

            if (isHacking)
            {
                curHackingTime += Time.deltaTime;

                if (curHackingTime >= hackingTime)
                {
                    curHackingTime = 0;
                    UIManager.Instance.HackingUI.GetComponent<HackingPanel>().EnemyInfo = hackingEnemyInfo;
                    UIManager.Instance.HackingUI.SetActive(true);
                    hackingEnemyInfo.GetComponent<Enemy>().isHacking = true;
                    isHacking = false;
                }

                if (isDamage)
                {
                    if (isDamage)
                    {
                        curHackingTime = 0;
                        hackingEnemyInfo = null;
                        isHacking = false;
                    }
                }
            }
            else
            {
                curHackingTime = 0;
                hackingEnemyInfo = null;
                isHacking = false;
            }
        }
        else
        {
            curHackingTime = 0;
            hackingEnemyInfo = null;
            isHacking = false;
        }

        UIManager.Instance.HackingGauge.value = curHackingTime / hackingTime;
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

    void OCInventory()
    {
        if ((iDown || qDown) && !isShot && !isDamage && !isReload && !isDodge && !isInteraction && !CreateManager.Instance.isCreating)
        {

            if (iDown)
            {
                isInventoryOpen = !isInventoryOpen;
                Time.timeScale = isInventoryOpen ? 0f : 1.0f;
                cameraArm.enabled = !isInventoryOpen;
                UIManager.Instance.inventoryPanel.SetActive(isInventoryOpen);
            }

            if(qDown)
            {
                isInventoryOpen = false;
                Time.timeScale = 1.0f;
                cameraArm.enabled = !isInventoryOpen;
                UIManager.Instance.inventoryPanel.SetActive(isInventoryOpen);
            }
        }
    }

    void FlashLightOnOff()
    {
        if (eDown)
        {
            isFlashLightOn = !isFlashLightOn;
            flashLight.SetActive(isFlashLightOn);
        }
    }

    void OCMap()
    {
        if((mDown || qDown) && !isShot && !isDodge && !isHacking && !isReload && !isInteraction)
        {
            if(mDown)
            {
                isMapOpen = !isMapOpen;
                
                if(isMapOpen)
                {
                    miniMap.position = new Vector3(350, 730, 0);
                    miniMap.sizeDelta = new Vector2(600, 600);
                    UIManager.Instance.miniMapCamera.orthographicSize = 20;
                }
                else
                {
                    miniMap.position = new Vector3(200, 880, 0);
                    miniMap.sizeDelta = new Vector2(300, 300);
                    UIManager.Instance.miniMapCamera.orthographicSize = 10;
                }
            }

            if(qDown)
            {
                isMapOpen = false;
                miniMap.position = new Vector3(200, -200, 0);
                miniMap.sizeDelta = new Vector2(300, 300);
                UIManager.Instance.miniMapCamera.orthographicSize = 10;
            }
        }
    }

    void OnFootstep(AnimationEvent animationEvent)
    {

    }

    public void ShotAnimEnd()
    {
        isShotEnd = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);

        Gizmos.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * 20);
        
    }
}
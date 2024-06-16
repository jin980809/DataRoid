using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Reflection;

public class Player : MonoBehaviour
{ 
    public Transform borderPos;

    [Space(10)]
    [Header("Player Weapon")]
    [SerializeField]
    private Transform ShootPos;
    public GameObject bullet;
    public Weapon weapon; //초기 총기 설정(권총)
    public GameObject muzzleEffect;
    public GameObject[] hitEffect;
    public GameObject damageEffect;
    public GameObject flashLight;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public int equipWeaponIndex;
    public int lastSwapWeaponIndex;
    public float sheildDamage;
    public float sheildDiscountRate;
    [SerializeField] private float shotDeley;

    [Space(10)]
    [Header("Player State Speed")]
    public float walkSpeed;
    public float zoomSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float dodgeSpeed;
    public float gunWalkSpeed;
    public float gunRunSpeed;

    [Space(10)]
    [Header("Player Hp")]
    public float maxHp;
    public float curHp;
    public float decreaseHpRate;

    [Space(10)]
    [Header("Player CoolTime")]
    public float dodgeCoolTime;

    public float targetSpeed;
    private float culDodgeTime;

    float hAxis;
    float vAxis;
    bool rDown; // 뛰기
    bool sDown; // 앉기
    bool fDown; // 공격
    bool f2Down; // 조준
    bool dDown; // 구르기 space
    bool gDown; // 총 꺼내고 넣기 x
    bool lDown; // 재장전 r
    bool aDown; // 상호작용 f
    bool iDown; // 인벤토리 i
    bool eDown; // 손전등 t
    bool qDown; // esc
    bool skDown1; // 스킬1 Q
    bool skDown2; // 인벤토리
    bool mDown; // 맵 크기
    bool vDown; // 오브젝트 찾기

    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;

    [Space(10)]
    [Header("Player State")]
    public bool isDamage = false;
    public bool isWalk;
    public bool isRun;
    bool isFireReady = true;
    bool isBorder;
    public bool isDodge = false;
    private bool isDodgeReady = true;
    public bool isGunOn = false;
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
    public bool qToggle = false;
    public bool qSkillOn = false;
    bool eSkillOn = false;
    public bool isSubdue = false;
    public bool isStun;
    public bool isCommunicate = false;
    public bool isMeleeAttack = false;
    public bool isSettingOn = false;

    private float rotationVelocity;
    private float _animationBlend;
    private float verBlend;
    private float horBlend;
    private float fireDelay;
    private float interactionTime;
    //private float curHackingTime = 0f;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 gunOnVec;
    Vector3 targetDirection;

    [Space(10)]
    [Header("Camera")]
    public GameObject _mainCamera;
    public CameraMove cameraArm;
    Rigidbody rigid;
    RectTransform miniMap;

    //[Space(10)]
    //[Header("Hacking Skill")]
    //public float hackingCoolTime;
    //public int hackingNum;
    //public float curHackingCoolTime;
    //public float curHackingNum;
    //public GameObject hackingEnemyInfo;
    //public float hackingTime;

    [Space(10)]
    [Header("Granade")]
    public GameObject granade;
    GameObject instGranade;
    public float granadeCoolTime;
    float curGranadeCoolTime = 0;
    bool isGranade;

    [Space(10)]
    [Header("EMP Skill")]
    public float stunDistance;
    public float stunCoolTime;
    public float curStunCoolTime;
    public float stunTime;

    [Space(10)]
    [Header("MeleeAttack")]
    public BoxCollider meleeAttackCol;
    bool isMeleeAttackReady = true;

    [Space(10)]
    [Header("Search")]
    private bool isScan = false;
    public bool droneOn;
    public bool deviceOn;
    public float searchDistance;
    public float nameTagDisappearSeconds;
    public GameObject scanEffect;
    ObjectNameUI nameTag;

    [Space(10)]
    [Header("Else")]
    private LayerMask enemyLayer;
    public GameObject SubdueObject;
    List<ScriptableRendererFeature> rendererFeatures;
    ScriptableRendererData data;
    Animator anim;
    CrosshairDemoPlayerRecoil recoil;
    Coroutine meleeAttackCor;
    public Hovl_DemoLasers Lazers;
    bool isLazerAttack = false;
    //public AimMovement aimMove;
    bool isShotAnimStart = false;
    bool[] autoReload = new bool[4];
    bool isTriggerInteraction = false;
    public ChangeHPMat cHpMat;
    private Coroutine changeDamageLightCor;
    public ObjectNameUI elecChargeNameTag = null;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        miniMap = UIManager.Instance.MiniMap.GetComponent<RectTransform>();
        //recoil = GetComponent<CrosshairDemoPlayerRecoil>();
        //cHpMat = GetComponentInChildren<ChangeHPMat>();
        //curHackingNum = 0;
    }

    void Start()
    {
        //curHp = maxHp;
    }

    void Update()
    {
        GetInput();

        OCSettingUI();

        Move();

        GunOnMove();

        Shot();

        //MeleeAttack();

        Dodge();

        Reload();

        Interaction();

        //OCInventory();
        newOCInventory();

        FlashLightOnOff();

        //EnemyHacking();
        Granade();

        Swap();

        ZoomInOut();

        //OCMap();

        //HackingCoolDown();

        //EnemysStun();

        SubdueCancel();

        WeaponEnable();

        ObjectNameTag();

        if(deviceOn)
        {
            UIManager.Instance.aim.SetActive(true);
        }
        else
        {
            UIManager.Instance.aim.SetActive(false);
        }

        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        sDown = Input.GetButton("Crouch");
        fDown = (isGunOn && (equipWeaponIndex == 1 || equipWeaponIndex == 3)) ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
        f2Down = Input.GetButton("Fire2");
        dDown = Input.GetButton("Dodge");
        gDown = Input.GetButtonDown("GunOn");
        lDown = Input.GetButton("Reload");
        aDown = /*(isSubdue || isMeleeAttackReady) ? Input.GetButtonDown("Interaction") :*/ Input.GetButton("Interaction");
        iDown = Input.GetButtonDown("Inventory");
        eDown = Input.GetButtonDown("FlashLight");
        qDown = Input.GetButtonDown("Cancel");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        sDown4 = Input.GetButtonDown("Swap4");
        skDown1 = Input.GetButtonDown("Skill1");
        skDown2 = Input.GetButtonDown("Skill2");
        mDown = Input.GetButtonDown("Map");
        vDown = Input.GetButtonDown("Search");
    }

    void WeaponEnable()
    {
        if(MaterialManager.Instance.HandgunAmmo > 0 || weapons[0].GetComponent<Weapon>().curAmmo > 0)
        {
            hasWeapons[0] = true;
        }
        else
        {
            hasWeapons[0] = false;
        }

        if (MaterialManager.Instance.RifleAmmo > 0 || weapons[1].GetComponent<Weapon>().curAmmo > 0)
        {
            hasWeapons[1] = true;
        }
        else
        {
            hasWeapons[1] = false;
            autoReload[1]= true;
            if(equipWeaponIndex == 1)
            {
                equipWeaponIndex = 0;
                weapon = weapons[0].GetComponent<Weapon>();
                weapons[1].SetActive(false);
                weapons[0].SetActive(true);
                muzzleEffect = weapon.muzzleFlash;

                isSwap = true;

                Invoke("SwapOut", 0.4f);
            }
        }

        if (MaterialManager.Instance.ShotgunAmmo > 0 || weapons[2].GetComponent<Weapon>().curAmmo > 0)
        {
            hasWeapons[2] = true; 
        }
        else
        {
            hasWeapons[2] = false;
            autoReload[2]= true;
            if (equipWeaponIndex == 2)
            {
                equipWeaponIndex = 0;
                weapon = weapons[0].GetComponent<Weapon>();
                weapons[2].SetActive(false);
                weapons[0].SetActive(true);
                muzzleEffect = weapon.muzzleFlash;

                isSwap = true;

                Invoke("SwapOut", 0.4f);
            }
        }

        if (MaterialManager.Instance.LazerAmmo > 0 || weapons[3].GetComponent<Weapon>().curAmmo > 0)
        {
            hasWeapons[3] = true;
        }
        else
        {
            hasWeapons[3] = false;
            autoReload[3] = true;
            if (equipWeaponIndex == 3)
            {
                equipWeaponIndex = 0;
                weapon = weapons[0].GetComponent<Weapon>();
                weapons[3].SetActive(false);
                weapons[0].SetActive(true);
                muzzleEffect = weapon.muzzleFlash;
                SoundManager.Instance.StopLoopSound("Laser_Loop");

                isSwap = true;

                Invoke("SwapOut", 0.4f);
            }
        }
    }

    void Move()
    {
        if (!isDodge && !isSubdue && !isInteraction && !isCommunicate && !isInventoryOpen && !isSettingOn)
        {
            if(isGunOn)
            {
                if (rDown && !isReload && isShotEnd && !isZoom && !isHacking && !isShot && !isStun) targetSpeed = gunRunSpeed;
                else if (isZoom && isStun) targetSpeed = zoomSpeed;
                else targetSpeed = gunWalkSpeed;
            }
            else
            {
                if (sDown && !isStun) targetSpeed = crouchSpeed;
                else if (rDown && !isStun) targetSpeed = runSpeed;
                else targetSpeed = walkSpeed;
            }

            isRun = (rDown && !sDown && !aDown);
        }
        if (isDodge && !isInteraction)
        {
            targetSpeed = dodgeSpeed;
            moveVec = dodgeVec;
        }

        moveVec = new Vector3(hAxis, 0f, vAxis).normalized;
        if (moveVec == Vector3.zero && !isDodge || (isInteraction || isSubdue || isMeleeAttack || isCommunicate || isInventoryOpen || isSettingOn)) { targetSpeed = 0.0f;/* crouchSpeed = 0;*/ }

        float targetRotation = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

        if (moveVec != Vector3.zero && !isSubdue && !isInteraction)
        {
            isWalk = true;

            if (!isDodge && !isInteraction && !isCommunicate && !isInventoryOpen && !isMeleeAttack && !isSettingOn)
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, 0.17f);

                transform.rotation = Quaternion.Euler(0.0f, (!isShotEnd || isZoom || isHacking || isSubdue || isShot) ? _mainCamera.transform.eulerAngles.y : rotation, 0.0f);
            }
            else
            {
                if (isDodgeReady && !isInteraction && !isCommunicate && !isInventoryOpen && !isMeleeAttack && !isSettingOn)
                    transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f); //도약할때 바로 캐릭터가 회전하게
            }
        }
        else
        {
            isWalk = false;

            if ((!isShotEnd || isZoom || isHacking || isShot) && !isDodge && !isInteraction && !isMeleeAttack && !isSettingOn)
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

        if (!isBorder && !isInteraction && !isCommunicate && !isSettingOn)
            transform.position += targetDirection.normalized * (/*sDown ? crouchSpeed : */targetSpeed) * Time.deltaTime;

        float aimAngle = _mainCamera.transform.eulerAngles.x + 35f;

        anim.SetBool("isCrouch", sDown);
        anim.SetFloat("speed", _animationBlend);
        anim.SetFloat("UpDown", (float) (aimAngle >= 359.1f ? aimAngle - 359.9f : aimAngle) / 105f);
        anim.SetBool("isGunOn", isGunOn);
        if (isZoom || !isShotEnd || isShot)
        {
            anim.SetFloat("Vertical", verBlend);
            anim.SetFloat("Horizental", horBlend);
        }
        else
        {
            anim.SetFloat("Vertical", 1);
            anim.SetFloat("Horizental", 0);
        }
    }

    void Dodge()
    {
        culDodgeTime += Time.deltaTime;
        isDodgeReady = dodgeCoolTime < culDodgeTime;

        if (!isDodge && dDown && isDodgeReady && !isShot && isWalk && !isInteraction && !isInventoryOpen && !isHacking && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && !isSettingOn)
        {
            isShotEnd = true;
            isDodge = true;
            
            if (isReload)
            {
                StopCoroutine(ReloadOut());
                isReload = false;
                anim.SetTrigger("doReloadOut");
            }


            culDodgeTime = 0f;
            dodgeVec = moveVec;
            //targetSpeed *= 4;
            anim.SetTrigger("doDodge");
            Invoke("DodgeOut", 0.7f);

        }
    }

    

    void DodgeOut()
    {
        //targetSpeed *= 0.25f;
        isDodge = false;
        anim.SetTrigger("DodgeOut");
    }

    void GunOnMove()
    {
        if (gDown && !isReload && !isInteraction && !isInventoryOpen && !isDodge && !isInteraction && !isInventoryOpen && !isHacking && isShotEnd && !isSubdue && !isStun && hasWeapons[lastSwapWeaponIndex] && !isMeleeAttack && !isCommunicate && !isSettingOn)
        {
            isGunOn = !isGunOn;
            weapon.gameObject.SetActive(isGunOn);
            muzzleEffect = weapon.muzzleFlash;

            if (!isGunOn)
            {
                isZoom = false;
                lastSwapWeaponIndex = equipWeaponIndex;
                equipWeaponIndex = -1;
                
            }
            else
            {
                //if()
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
        if (sDown4 && (!hasWeapons[3] || equipWeaponIndex == 3))
            return;

        if ((sDown1 || sDown2 || sDown3 || sDown4) && !isSwap  && !isShot && !isDodge && !isReload && !isInteraction && !isHacking && !isInventoryOpen && isShotEnd && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && !isSettingOn)
        {
            isGunOn = true;
            isZoom = false;
            int weaponIndex = -1;

            if (sDown1) { weaponIndex = 0; lastSwapWeaponIndex = 0; }
            if (sDown2) { weaponIndex = 1; lastSwapWeaponIndex = 1; }
            if (sDown3) { weaponIndex = 2; lastSwapWeaponIndex = 2; }
            if (sDown4) { weaponIndex = 3; lastSwapWeaponIndex = 3; }

            if (weapon != null)
                weapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            weapon = weapons[weaponIndex].GetComponent<Weapon>();

            weapon.gameObject.SetActive(true);
            if (weapon.isLazer && Lazers == null)
                Lazers = weapon.lazerEffect;

            muzzleEffect = weapon.muzzleFlash;

            if(weaponIndex != -1)
            {
                if (autoReload[weaponIndex])
                {
                    if (weaponIndex == 1)
                    {
                        if(MaterialManager.Instance.RifleAmmo > weapon.maxAmmo)
                        {
                            weapon.curAmmo = weapon.maxAmmo;
                            MaterialManager.Instance.RifleAmmo -= weapon.maxAmmo;
                        }
                        else
                        {
                            weapon.curAmmo = MaterialManager.Instance.RifleAmmo;
                            MaterialManager.Instance.RifleAmmo = 0;
                        }


                    }
                    else if (weaponIndex == 2)
                    {
                        if (MaterialManager.Instance.ShotgunAmmo > weapon.maxAmmo)
                        {
                            weapon.curAmmo = weapon.maxAmmo;
                            MaterialManager.Instance.ShotgunAmmo -= weapon.maxAmmo;
                        }
                        else
                        {
                            weapon.curAmmo = MaterialManager.Instance.ShotgunAmmo;
                            MaterialManager.Instance.ShotgunAmmo = 0;
                        }
                    }
                    else if (weaponIndex == 3)
                    {
                        if (MaterialManager.Instance.LazerAmmo > weapon.maxAmmo)
                        {
                            weapon.curAmmo = weapon.maxAmmo;
                            MaterialManager.Instance.LazerAmmo -= weapon.maxAmmo;
                        }
                        else
                        {
                            weapon.curAmmo = MaterialManager.Instance.LazerAmmo;
                            MaterialManager.Instance.LazerAmmo = 0;
                        }
                    }

                    autoReload[weaponIndex] = false;
                }
            }

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
        if (isFireReady && muzzleEffect != null && !weapon.isLazer)
        {
            muzzleEffect.SetActive(false);
        }

        isShot = fDown && !isDodge && isGunOn && weapon.curAmmo > 0 && !isReload && !isInteraction && !isInventoryOpen && !isHacking && !qToggle && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack;

        if (fDown && !qToggle && isFireReady && !isDodge && isGunOn && weapon.curAmmo > 0 && !isReload && !isInteraction && !isInventoryOpen && !isHacking && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && !isSettingOn)
        {
            //recoil.Shot();
            isShotEnd = false;
            //aimMove.MoveAim();
            fireDelay = 0;
            RaycastHit hit;
            //muzzleEffect.SetActive(true);
            anim.SetTrigger("doShot");
            weapon.curAmmo -= 1;

            switch(equipWeaponIndex)
            {
                case 0:
                    SoundManager.Instance.PlaySound2D("Pistol");
                    break;

                case 1:
                    SoundManager.Instance.PlaySound2D("Riffle");
                    break;

                case 2:
                    SoundManager.Instance.PlaySound2D("ShotGun");
                    break;

                case 3:
                    //SoundManager.Instance.PlaySound2D("Laser_Start");
                    break;
            }
            

            Vector3 playerShotPos = ShootPos.position;
            if (!weapon.isShotGun)
            {
                if(!weapon.isLazer)
                {
                    if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, 40, ~LayerMask.GetMask("PhysicsEnemy", "TextBox") | LayerMask.GetMask("Enemy")))
                    {
                        StartCoroutine(ShotEffect(hit.point));
                        Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                        //BulletInstance((hit.point - muzzleEffect.transform.position).normalized);

                        /*switch (hit.transform.gameObject.layer)
                        {
                            case 9://Enemy
                                SpawnHitEffect(hit.point, 1);
                                //enemy.HitEffect(hit.point);
                                break;

                            case 6: //Enviroment
                                SpawnHitEffect(hit.point, 0);
                                break;

                            case 16: // Glass
                                SpawnHitEffect(hit.point, 2);
                                break;
                        }*/

                        if (isEnemyHitArea(hit.collider.transform.gameObject) && !enemy.isDeath && hit.transform.gameObject.layer == 9)
                        {
                            enemy.OnDamage(weapon.damage, playerShotPos, hitArea(hit.collider.transform.gameObject));
                        }
                    }
                    else
                    {
                         StartCoroutine(ShotEffect(playerShotPos + (_mainCamera.transform.forward * 40)));
                    }
                }
                else //레이저건
                {
                    if (Physics.SphereCast(_mainCamera.transform.position, 0.1f, _mainCamera.transform.forward, out hit, 100f, ~LayerMask.GetMask("PhysicsEnemy", "TextBox") | LayerMask.GetMask("Enemy")))
                    {
                        muzzleEffect.SetActive(true);

                        StartCoroutine(LazerEffect(hit.point));

                        Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                        if (isEnemyHitArea(hit.collider.transform.gameObject) && !enemy.isDeath && hit.transform.gameObject.layer == 9)
                        {
                            enemy.OnDamage(weapon.damage, playerShotPos, hitArea(hit.collider.transform.gameObject));
                        }
                    }
                    else
                    {
                        weapon.muzzleFlash.transform.rotation = Quaternion.LookRotation(((_mainCamera.transform.position + _mainCamera.transform.forward * 20) - muzzleEffect.transform.position).normalized);
                        if (!isLazerAttack)
                        {
                            Lazers.LazerOn();
                            isLazerAttack = true;
                        }
                    }
                }
            }
            else
            {
                for(int i = 0; i < weapon.shotGunSpreadAmount; i++)
                {
                    if (Physics.Raycast(_mainCamera.transform.position, GetShotgunDirecting(), out hit, 40, ~LayerMask.GetMask("PhysicsEnemy", "TextBox") | LayerMask.GetMask("Enemy")))
                    {
                        Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                        //BulletInstance((hit.point - muzzleEffect.transform.position).normalized);
                        StartCoroutine(ShotEffect(hit.point));
                        //switch (hit.transform.gameObject.layer)
                        //{
                        //    case 9://Enemy
                        //        SpawnHitEffect(hit.point, 1);
                        //        enemy.HitEffect(hit.point);
                        //        break;

                        //    case 6: //Enviroment
                        //        SpawnHitEffect(hit.point, 0);
                        //        break;

                        //    case 16: // Glass
                        //        SpawnHitEffect(hit.point, 2);
                        //        break;
                        //}

                        if (isEnemyHitArea(hit.collider.transform.gameObject) && !enemy.isDeath)
                        {
                            //Debug.Log(hitArea(hit.collider.transform.gameObject));

                            enemy.OnDamage(weapon.damage, playerShotPos, hitArea(hit.collider.transform.gameObject));


                            //Debug.Log(isEnemyHitArea(hit.collider.transform.gameObject));
                        }
                    }
                    else
                    {
                        StartCoroutine(ShotEffect(playerShotPos + (_mainCamera.transform.forward * 40)));
                    }
                }
            }

            isSemiReady = false;
        }

        if(!isShot)
        {
            isSemiReady = true;
            if (weapon.isLazer)
            {
                if (isLazerAttack)
                {
                    SoundManager.Instance.StopLoopSound("Laser_Loop");
                    Lazers.LazerOff();
                }
                isLazerAttack = false;

            }
        }
        
        anim.SetBool("isShotOut", !isShotEnd && !isDodge);
    }

    IEnumerator ShotEffect(Vector3 hitPos)
    {
        yield return new WaitForSeconds(0.1f);
        muzzleEffect.SetActive(true);
        BulletInstance((hitPos - muzzleEffect.transform.position).normalized);
    }

    IEnumerator LazerEffect(Vector3 hitPos)
    {
        yield return new WaitForSeconds(0.1f);
        if (!isLazerAttack)
        {
            SoundManager.Instance.PlaySound2D("Laser_Loop", 0, true);
            Lazers.LazerOn();
            isLazerAttack = true;
        }

        weapon.muzzleFlash.transform.rotation = Quaternion.LookRotation((hitPos - Lazers.FirePoint.transform.position).normalized);
    }

    void MeleeAttack()
    {
        if(aDown && isGunOn && !isMeleeAttack && !isInteraction && !isSwap && !isShot && !isDodge && !isReload && !isHacking && !isInventoryOpen && !isSubdue && !isStun && !isCommunicate && !isZoom && !isSubdue)
        {
            //Debug.Log("meleeAttack");
            //isMeleeAttack = true;
            anim.SetTrigger("doMeleeAttack");
            meleeAttackCor = StartCoroutine(DoMeleeAttack());
        }
    }

    IEnumerator DoMeleeAttack()
    {
        isMeleeAttack = true;
        yield return new WaitForSeconds(0.2f);
        meleeAttackCol.enabled = true;
        yield return new WaitForSeconds(0.2f);
        meleeAttackCol.enabled = false;
        anim.SetTrigger("doMeleeAttackOut");
        yield return new WaitForSeconds(0.4f);
        isMeleeAttack = false;

    }

    void ObjectNameTag()
    {
        //RaycastHit hit;
        if (vDown && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen && !isHacking && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && droneOn && !isScan && !isSettingOn)
        {
            scanEffect.SetActive(true);
            RaycastHit[] hits = Physics.SphereCastAll(ShootPos.position, searchDistance, Vector3.up, 0f, LayerMask.GetMask("Interaction"));
            Debug.Log("aa");

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.GetComponent<ObjectNameUI>() != null)
                {
                    if (!IsObstacleBetween(transform.position, hits[i].transform.position, LayerMask.GetMask("Enviroment")))
                    {
                        nameTag = hits[i].transform.GetComponent<ObjectNameUI>();
                        nameTag.isNameTagOpen = true;
                        StartCoroutine(NameTagOut());
                    }
                }
            }

            StartCoroutine(ScanEffectOut());
        }
    }

    IEnumerator NameTagOut()
    {

        yield return new WaitForSeconds(nameTagDisappearSeconds);
        nameTag.isNameTagOpen = false;
    }

    IEnumerator ScanEffectOut()
    {
        isScan = true;
        yield return new WaitForSeconds(nameTagDisappearSeconds + 1);
        scanEffect.SetActive(false);
        isScan = false;
    }

    Vector3 GetShotgunDirecting()
    {
        Vector3 targetPos = _mainCamera.transform.position + _mainCamera.transform.forward * weapon.shotGunRange;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-weapon.inaccuracyDistance, weapon.inaccuracyDistance),
            targetPos.y + Random.Range(-weapon.inaccuracyDistance, weapon.inaccuracyDistance),
            targetPos.z + Random.Range(-weapon.inaccuracyDistance, weapon.inaccuracyDistance)
            );

        Vector3 direction = targetPos - _mainCamera.transform.position;

        return direction.normalized;
    }

    void ZoomInOut()
    {
        if(f2Down && !isInteraction && !isReload && !isInventoryOpen && !isHacking && !isCreaingUIOpen && !isSwap && isGunOn && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && !isSettingOn)
        {
            isZoom = true;
        }
        else
        {
            isZoom = false;
        }
        anim.SetBool("isAim", isZoom && !isShot && isShotEnd);
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

    void SpawnHitEffect(Vector3 spawnPosition, int eIndex)
    {
        Quaternion enemyRotation = Quaternion.LookRotation(_mainCamera.transform.forward, Vector3.up);
        GameObject hitObj = Instantiate(hitEffect[eIndex], spawnPosition, enemyRotation);

        //hitObj.GetComponent<>().Play();
    }

    void BulletInstance(Vector3 direction)
    {
        GameObject inst_bullet = Instantiate(bullet);
        inst_bullet.transform.position = muzzleEffect.transform.position;
        inst_bullet.GetComponent<Bullet>().direction = direction;
    }

    void Reload()
    {
        if (lDown && weapon.curAmmo < weapon.maxAmmo && isGunOn && !isShot && !isReload && !isDodge && !isInteraction 
            && /*MaterialManager.Instance.Ammo > 0 &&*/ !isInventoryOpen && !isHacking && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && !isSettingOn) // 가지고 있는 총알 개수가 0 이하가 아니면 추가
        {
            //if (equipWeaponIndex == 0)
            //    return;
            if (equipWeaponIndex == 1 && MaterialManager.Instance.RifleAmmo <= 0)
                return;
            if (equipWeaponIndex == 2 && MaterialManager.Instance.ShotgunAmmo <= 0)
                return;            
            if (equipWeaponIndex == 3 && MaterialManager.Instance.LazerAmmo <= 0)
                return;

            switch (equipWeaponIndex)
            {
                case 0:
                    SoundManager.Instance.PlaySound2D("Gun_Reload");
                    break;

                case 1:
                    SoundManager.Instance.PlaySound2D("RiffleReload");
                    break;

                case 2:
                    SoundManager.Instance.PlaySound2D("Gun_Reload");
                    break;

                case 3:
                    SoundManager.Instance.PlaySound2D("RiffleReload");
                    break;
            }

            isReload = true;
            isZoom = false;
            isShotEnd = true;
            //isZoom = false;
            anim.SetTrigger("doReload");
            StartCoroutine(ReloadOut());
        }
    }

    IEnumerator ReloadOut()
    {
        yield return new WaitForSeconds(1.5f);

        if (isReload)
        {
            //curHp -= weapon.reloadCost;
            //weapon.curAmmo = weapon.maxAmmo;
            if (equipWeaponIndex == 0)
            {  
                float loseHp = weapon.reloadCost * (weapon.maxAmmo - weapon.curAmmo);
                //Debug.Log(loseHp);
                curHp -= loseHp;
                UIManager.Instance.LoseBatteryUI(loseHp);
                weapon.curAmmo = weapon.maxAmmo;
            }
            else if (equipWeaponIndex == 1)
            {
                if (MaterialManager.Instance.RifleAmmo < (weapon.maxAmmo - weapon.curAmmo))
                {
                    weapon.curAmmo += MaterialManager.Instance.RifleAmmo;
                    MaterialManager.Instance.RifleAmmo = 0;
                }
                else
                {
                    MaterialManager.Instance.RifleAmmo -= (weapon.maxAmmo - weapon.curAmmo);
                    weapon.curAmmo = weapon.maxAmmo;

                }
            }
            else if (equipWeaponIndex == 2)
            {
                if (MaterialManager.Instance.ShotgunAmmo < (weapon.maxAmmo - weapon.curAmmo))
                {
                    weapon.curAmmo += MaterialManager.Instance.ShotgunAmmo;
                    MaterialManager.Instance.ShotgunAmmo = 0;
                }
                else
                {
                    MaterialManager.Instance.ShotgunAmmo -= (weapon.maxAmmo - weapon.curAmmo);
                    weapon.curAmmo = weapon.maxAmmo;

                }
            }
            else if (equipWeaponIndex == 3)
            {
                if (MaterialManager.Instance.LazerAmmo < (weapon.maxAmmo - weapon.curAmmo))
                {
                    weapon.curAmmo += MaterialManager.Instance.LazerAmmo;
                    MaterialManager.Instance.LazerAmmo = 0;
                }
                else
                {
                    MaterialManager.Instance.LazerAmmo -= (weapon.maxAmmo - weapon.curAmmo);
                    weapon.curAmmo = weapon.maxAmmo;

                }
            }
        }

        //가지고 있는 총알개수가 maxAmmo보다 적으면 가지고 있는 총알 개수만 curAmmo에 장전
        yield return new WaitForSeconds(0.5f);
        if (isReload)
        {
            anim.SetTrigger("doReloadOut");
        }
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
        int layerMask = (1 << LayerMask.NameToLayer("Enviroment")) + (1 << LayerMask.NameToLayer("Door")) + (1 << LayerMask.NameToLayer("PhysicsEnemy")) + (1 << LayerMask.NameToLayer("Glass")) + (1 << LayerMask.NameToLayer("Object"));
        RaycastHit hit;
        if (Physics.SphereCast(borderPos.position, 0.15f, targetDirection, out hit, 0.5f, layerMask))
        {
            isBorder = true;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            isBorder = false;
            //GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezeAll;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            if (!isDamage && !isSubdue && !IsObstacleBetween(transform.position, other.transform.position, LayerMask.GetMask("Enviroment")))
            {
                Attack attack = other.transform.GetComponent<Attack>();
                UIManager.Instance.DamageImage();

                StartCoroutine(OnDamage(attack.curDamage));
                if(other.transform.GetComponentInParent<EnemyB>() != null)
                {
                    //other.transform.GetComponentInParent<EnemyB>().isAimming = false;
                    //other.enabled = false;
                    //other.transform.GetComponentInParent<EnemyB>().isMeleeAttack = false;
                    //other.transform.GetComponentInParent<EnemyB>().isSCoolDown = true;
                    //other.transform.GetComponentInParent<EnemyB>().isSpecialAttack = false;

                    //other.transform.GetComponentInParent<EnemyB>().anim.SetTrigger("doMeleeAttack");
                    //isStun = false;
                }

                if(isSubdue)
                {
                    StopCoroutine(meleeAttackCor);
                    meleeAttackCol.enabled = false;
                    isMeleeAttack = false;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            isTriggerInteraction = true;

            Interaction interactionObj = other.transform.GetComponent<Interaction>();

            if (!UIManager.Instance.interactionUIOpen)
            {
                if (interactionObj.isGetData)
                {
                    UIManager.Instance.InteractionButtonImage(1, interactionObj.interactionName);
                }
                else if ((int)(interactionObj.interactionType) == 1)
                {
                    UIManager.Instance.InteractionButtonImage(2, interactionObj.interactionName);
                }
                else //보조배터리 나오면 추가하기
                {
                    UIManager.Instance.InteractionButtonImage(0, interactionObj.interactionName);
                }

                UIManager.Instance.interactionUIOpen = true;
            }

            if (aDown && !isShot && !isDodge && !isReload && !interactionObj.isCoolTime && !isHacking && !isInventoryOpen && !isSubdue && !isStun && interactionObj.enabled && !isCommunicate && !isMeleeAttack && !isSettingOn)
            {
                if (interactionObj.isNonCharging)
                {
                    interactionObj.InteractionResult();
                }
                else
                {
                    isInteraction = true;
                    isZoom = false;
                }
            }
            else
            {
                interactionTime = 0;
                isInteraction = false;

                //if (interactionObj.isCoolTime)
                //    Debug.Log("CoolTime");
            }

            if (isInteraction && !interactionObj.isNonCharging)
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

            if (!interactionObj.isNonCharging)
            {
                UIManager.Instance.InteractionGauge.fillAmount = interactionTime / interactionObj.interactionTime;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Interaction"))
        {
            if (UIManager.Instance.interactionUIOpen)
            {
                UIManager.Instance.InteractionButtonImage(-1, " ");
                UIManager.Instance.interactionUIOpen = false;
            }
            isTriggerInteraction = false;
        }
    }

    public void Damage(float damage)
    {
        if (!isDamage)
        {
            StartCoroutine(OnDamage(damage));
        }
    }

    public IEnumerator DamageLightChange()
    {
        cHpMat.isPlayerHit = true;
        //https://blog.naver.com/cdw0424/222239813775
        yield return new WaitForSeconds(1f);

        cHpMat.isPlayerHit = false;
    }

    public IEnumerator OnDamage(float damage)
    {
        isDamage = true;
        isStun = false;
        curHp -= damage;
        anim.SetTrigger("Hit");
        UIManager.Instance.deviceUIAnim.SetTrigger("Hited");
        if (changeDamageLightCor != null)
        {
            StopCoroutine(changeDamageLightCor);
            StartCoroutine(DamageLightChange());
        }
        else
        {
            StartCoroutine(DamageLightChange());
        }
        //Vector3 playerDir = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        //Vector3 EnemyDir = new Vector3(enemyPos.x, enemyPos.y + 1f, enemyPos.z);
        //damageEffect.transform.rotation = Quaternion.LookRotation((EnemyDir - playerDir).normalized);
        //damageEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        
        isDamage = false;
        //damageEffect.SetActive(false);
        yield return null;
    }

    void Interaction()
    {
        RaycastHit hit;
        Interaction interactionObj;

        if (!isTriggerInteraction)
        {
            if (Physics.Raycast(_mainCamera.transform.position + (_mainCamera.transform.forward * Vector3.Distance(ShootPos.transform.position, _mainCamera.transform.position)), _mainCamera.transform.forward, out hit, 2, LayerMask.GetMask("Interaction")))
            {
                interactionObj = hit.transform.GetComponent<Interaction>();
                if (!IsObstacleBetween(ShootPos.transform.position, hit.point, LayerMask.GetMask("Enviroment", "Door")) && !interactionObj.isTrigger)
                {
                    isMeleeAttackReady = false;

                    if (!UIManager.Instance.interactionUIOpen)
                    {
                        if (interactionObj.isGetData)
                        {
                            UIManager.Instance.InteractionButtonImage(1, interactionObj.interactionName);
                        }
                        else if ((int)(interactionObj.interactionType) == 1)
                        {
                            UIManager.Instance.InteractionButtonImage(2, interactionObj.interactionName);
                        }
                        else //보조배터리 나오면 추가하기
                        {
                            UIManager.Instance.InteractionButtonImage(0, interactionObj.interactionName);
                        }
                        UIManager.Instance.interactionUIOpen = true;
                    }

                    if((int)interactionObj.interactionType == 1 || interactionObj.getBattery)
                    {
                        //elecChargeNameTag = interactionObj.GetComponentInChildren<ObjectNameUI>();

                        if(interactionObj.getBattery)
                        {
                            elecChargeNameTag = interactionObj.batteryNameTag;
                        }
                        else
                        {
                            elecChargeNameTag = interactionObj.chargeNameTag;
                        }

                        elecChargeNameTag.isNameTagOpen = true;
                    }

                    if (aDown && !isShot && !isDodge && !isReload && !interactionObj.isCoolTime && !isHacking && !isInventoryOpen && !isSubdue && !isStun && interactionObj.enabled && !isCommunicate && !isMeleeAttack && !isSettingOn)
                    {
                        if (interactionObj.isNonCharging)
                        {
                            interactionObj.InteractionResult();
                        }
                        else
                        {
                            isInteraction = true;
                            isZoom = false;
                        }
                    }
                    else
                    {
                        interactionTime = 0;
                        isInteraction = false;

                        //if (interactionObj.isCoolTime)
                        //    Debug.Log("CoolTime");
                    }

                    if (isInteraction && !interactionObj.isNonCharging)
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

                    if (!interactionObj.isNonCharging)
                    {
                        UIManager.Instance.InteractionGauge.fillAmount = interactionTime / interactionObj.interactionTime;
                    }
                }
            }
            else
            {
                if (UIManager.Instance.interactionUIOpen)
                {
                    UIManager.Instance.InteractionButtonImage(-1, " ");
                    UIManager.Instance.interactionUIOpen = false;
                }

                if(elecChargeNameTag != null)
                {
                    ElecChargeNameTagFalse();
                }
            }
        }
    }

    public void ElecChargeNameTagFalse()
    {
        elecChargeNameTag.isNameTagOpen = false;
        elecChargeNameTag = null;
    }

    //void EnemyHacking()
    //{
    //    RaycastHit hit1;
    //    //RaycastHit hit2;

    //    if(skDown1 && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen && qSkillOn && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack)
    //    {
    //        qToggle = !qToggle;
    //        if (rendererFeatures == null || rendererFeatures.Count <= 0) return;
    //        rendererFeatures[0].SetActive(qToggle);
    //    }


    //    if (qToggle && fDown && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen && qSkillOn && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack)
    //    {
    //        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit1, 20) && hackingEnemyInfo == null && isEnemyHitArea(hit1.transform.gameObject))
    //        {
    //            if (hit1.transform.GetComponent<Enemy>().sheild > 0 && !hit1.transform.GetComponent<Enemy>().isHacking && curHackingNum > 0)
    //            {
    //                //Debug.Log(hit1.transform.tag);
    //                hackingEnemyInfo = hit1.transform.gameObject;
    //                isHacking = true;
    //            }
    //            else
    //            {
    //                //실드 이미 없다고 하는 이미지나 이팩트 택스트 나타내기
    //            }
    //        }

    //        if (isHacking)
    //        {
    //            curHackingTime += Time.deltaTime;

    //            if (curHackingTime >= hackingTime)
    //            {
    //                curHackingTime = 0;
    //                curHackingNum--;
    //                hackingEnemyInfo.GetComponent<Enemy>().isHacking = true;
    //                hackingEnemyInfo.GetComponent<Enemy>().CreateEnemyUI();
    //                isHacking = false;
    //            }

    //            if (isDamage)
    //            {
    //                if (isDamage)
    //                {
    //                    curHackingTime = 0;
    //                    hackingEnemyInfo = null;
    //                    isHacking = false;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            curHackingTime = 0;
    //            hackingEnemyInfo = null;
    //            isHacking = false;
    //        }
    //    }
    //    else
    //    {
    //        curHackingTime = 0;
    //        hackingEnemyInfo = null;
    //        isHacking = false;
    //    }

    //    UIManager.Instance.HackingGauge.value = curHackingTime / hackingTime;
    //}

    //void HackingCoolDown()
    //{
    //    if(curHackingNum < hackingNum)
    //    {
            
    //        curHackingCoolTime += Time.deltaTime;
    //        if(curHackingCoolTime > hackingCoolTime)
    //        {
    //            curHackingCoolTime = 0;
    //            curHackingNum++;
    //        }
    //    }
    //}

    void Granade()
    {
        curGranadeCoolTime += Time.deltaTime;

        if(curGranadeCoolTime >= granadeCoolTime)
        {
            isGranade = true;
            curGranadeCoolTime = granadeCoolTime;
        }

        if (isGranade && skDown1 && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen  && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack)
        {
            curGranadeCoolTime = 0;
            isGranade = false;
            instGranade = Instantiate(granade);
            instGranade.transform.position = ShootPos.position;
            Rigidbody granadeRig = instGranade.GetComponent<Rigidbody>();
            granadeRig.AddForce(transform.forward * 10, ForceMode.Impulse);
        }
    }

    void EnemysStun()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        StunCoolDown();

        if (skDown2 && !isShot && !isDodge && !isReload && !isInteraction && !isCreaingUIOpen && !isInventoryOpen && !isHacking && curStunCoolTime >= stunCoolTime && !isSubdue && !isStun && !isCommunicate && eSkillOn && !isMeleeAttack)
        {
            //Debug.Log("sk2");
            curStunCoolTime = 0;

            RaycastHit[] hits = Physics.SphereCastAll(ShootPos.position, stunDistance, Vector3.up, 0f, enemyLayer);

            foreach (RaycastHit hit in hits)
            {
                GameObject enemyObject = hit.transform.gameObject;
                enemyObject.GetComponentInParent<Enemy>().Stun(false, stunTime);
            }
        }
    }

    void StunCoolDown()
    {
        if(curStunCoolTime < stunCoolTime)
        {
            curStunCoolTime += Time.deltaTime;
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

    //void OCInventory()
    //{
    //    if ((iDown || qDown) && !isShot && !isDamage && !isReload && !isDodge && !isInteraction && !CreateManager.Instance.isCreating && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack)
    //    {

    //        if (iDown)
    //        {
    //            isInventoryOpen = !isInventoryOpen;
    //            Time.timeScale = isInventoryOpen ? 0f : 1.0f;
    //            cameraArm.enabled = !isInventoryOpen;
    //            UIManager.Instance.inventoryPanel.SetActive(isInventoryOpen);
    //        }

    //        if(qDown)
    //        {
    //            isInventoryOpen = false;
    //            Time.timeScale = 1.0f;
    //            cameraArm.enabled = !isInventoryOpen;
    //            UIManager.Instance.inventoryPanel.SetActive(isInventoryOpen);
    //            UIManager.Instance.skillButtonUIAnim.SetTrigger("PlayerSkill_Close");
    //        }
    //    }
    //}

    void newOCInventory()
    {
        if ((skDown2 || qDown) && !isShot && !isDamage && !isReload && !isDodge && !isInteraction /*&& !CreateManager.Instance.isCreating*/ && !isSubdue && !isStun && !isCommunicate && !isMeleeAttack && deviceOn && !isSettingOn)
        {
            if (!isInventoryOpen && skDown2)
            {
                isInventoryOpen = true;
                //Time.timeScale = 0f;
                cameraArm.enabled = !isInventoryOpen;
                UIManager.Instance.mainUIAnim.SetTrigger("Open_Menu");

                //for (int i = 0; i < UIManager.Instance.fade_Ins.Length; i++)
                //{
                //    UIManager.Instance.fade_Ins[i].StartFadeIn();
                //}

            }

            else if(isInventoryOpen)
            {
                isInventoryOpen = false;
                //Time.timeScale = 1.0f;
                cameraArm.enabled = !isInventoryOpen;
                UIManager.Instance.mainUIAnim.SetTrigger("Open_Main");
            }
        }
    }

    void OCSettingUI()
    {
        if(qDown && !isCommunicate && !isInventoryOpen && deviceOn)
        {
            isSettingOn = !isSettingOn;

            if (isSettingOn)
            {
                Time.timeScale = 0;
                UIManager.Instance.mainUIAnim.SetTrigger("Open_ESC");
            }
            else
            {
                Time.timeScale = 1;
                UIManager.Instance.mainUIAnim.SetTrigger("Open_Main");
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
        if((mDown || qDown) && !isShot && !isDodge && !isHacking && !isReload && !isInteraction && !isSubdue && !isCommunicate)
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
        }
    }

    void SubdueCancel()
    {
        if (isSubdue)
        {
            anim.SetTrigger("doSubdue");
            if (aDown)
            {
                if (SubdueObject.GetComponent<ESN08Phase1>() != null)
                {
                    ESN08Phase1 eSN08Phase1 = SubdueObject.GetComponent<ESN08Phase1>();
                    //if (eSN08Phase1.curSubdueProgress > 0.4f)
                        eSN08Phase1.curSubdueProgress -= 0.3f;
                }
                else if (SubdueObject.GetComponent<TutSubdueEnemy>() != null)
                {
                    TutSubdueEnemy tutSubdue = SubdueObject.GetComponent<TutSubdueEnemy>();
                    //if (tutSubdue.curSubdueProgress > 0.4f)
                    tutSubdue.curSubdueProgress -= 0.3f;
                }
                else if(SubdueObject.GetComponent<EnemyB>() != null)
                {
                    EnemyB enemy = SubdueObject.GetComponent<EnemyB>();
                    //if (enemyA.curSubdueProgress > 0.4f)
                    enemy.curSubdueProgress -= 0.3f;
                }

                else
                {
                    EnemyA enemy = SubdueObject.GetComponent<EnemyA>();
                    //if (enemyA.curSubdueProgress > 0.4f)
                        enemy.curSubdueProgress -= 0.3f;
                }
            }
        }
        else
        {
            anim.SetTrigger("doSubdueOut");
        }
    }

    public void Stun(float stunTime)
    {
        if (!isStun)
        {
            //Debug.Log("aaa");
            isStun = true;
            //기절 애니메이션 실행
            StartCoroutine(StunOut(stunTime));
            if (changeDamageLightCor != null)
            {
                StopCoroutine(changeDamageLightCor);
                StartCoroutine(DamageLightChange());
            }
            else
            {
                StartCoroutine(DamageLightChange());
            }
        }
    }

    IEnumerator StunOut(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStun = false;
    }

    public void OnFootstep()
    {
        if(isRun)
        {
            SoundManager.Instance.PlaySound2D("Player_Run");
        }
        else
        {
            SoundManager.Instance.PlaySound2D("Player_Walk");
        }
    }

    public void ShotAnimEnd()
    {
        isShotEnd = true;
        isShotAnimStart = false;
    }

    public void ShotAnimStart()
    {
        isShotAnimStart = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);

        Gizmos.DrawRay(_mainCamera.transform.position + (_mainCamera.transform.forward * Vector3.Distance(ShootPos.transform.position, _mainCamera.transform.position)), _mainCamera.transform.forward * 20);
    }

    public void SetActiveRendererFeature<T>(bool active) where T : ScriptableRendererFeature
    {
        // URP Asset의 Renderer List에서 0번 인덱스 RendererData 참조
        ScriptableRendererData rendererData = GetRendererData(0);
        if (rendererData == null) return;

        List<ScriptableRendererFeature> rendererFeatures = rendererData.rendererFeatures;
        if (rendererFeatures == null || rendererFeatures.Count <= 0) return;

        for (int i = 0; i < rendererFeatures.Count; i++)
        {
            ScriptableRendererFeature rendererFeature = rendererFeatures[i];
            if (!rendererFeature) continue;
            if (rendererFeature is T) rendererFeature.SetActive(active);
        }
#if UNITY_EDITOR
        rendererData.SetDirty();
#endif
    }

    public ScriptableRendererData GetRendererData(int rendererIndex = 0)
    {
        // 현재 Quality 옵션에 세팅된 URP Asset 참조
        UniversalRenderPipelineAsset pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (!pipelineAsset) return null;

        // URP Renderer List 리플렉션 참조 (Internal 변수라서 그냥 참조 불가능)
        FieldInfo propertyInfo = pipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData[] rendererDatas = (ScriptableRendererData[])propertyInfo.GetValue(pipelineAsset);
        if (rendererDatas == null || rendererDatas.Length <= 0) return null;
        if (rendererIndex < 0 || rendererDatas.Length <= rendererIndex) return null;

        return rendererDatas[rendererIndex];
    }
}
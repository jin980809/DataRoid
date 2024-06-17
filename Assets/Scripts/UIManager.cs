using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    private static UIManager m_instance;

    public Animator mainUIAnim;
    public Animator deviceUIAnim;
    public Animator textUIAnim;
    public Animator questUIAnim;
    public Animator LevelPointUIAnim;
    public Animator LevelUpAnim;
    public Animator weaponBoxAnim;
    public Player player;
    public Image hpGauge;
    public Text hpText;
    public GameObject aim;

    [Space(10)]
    [Header("Interaction")]
    public Image InteractionGauge;
    public Image InteractionGaugeGlow;
    public bool interactionUIOpen = false;
    //public Text detectCount;

    [Space(10)]
    [Header("Weapon Box Item UI")]
    public GameObject riffleIcon;
    public GameObject shotgunIcon;
    public GameObject lazerIcon;
    public GameObject chargerIcon;

    public Text getRiffleAmmoText;
    public Text getShotgunAmmoText;
    public Text getLazerAmmoText;
    public Text getChargerText;
    //[Space(10)]
    //[Header("Inventory")]
    //public GameObject inventoryPanel;
    //public Text steelAmount;
    //public Text gunPowderAmount;
    //public Text ExpPieceAmount;
    //public Text ammoAmount;
    //public Text maxAmmoAmount;
    //public Text specialAmmoAmount;
    //public Text ExpCapsuleAmount;

    [Space(10)]
    [Header("Get UI Text")]
    public Text getBatteryText;
    public Text getDataText;

    [Space(10)]
    [Header("Data")]
    public Image DataGauge;
    public Text Datatext;
    public Text DataName;

    [Space(10)]
    [Header("HackingUI")]
    public Slider HackingGauge;


    [Space(10)]
    [Header("MiniMap")]
    public GameObject MiniMap;
    public Camera miniMapCamera;

    [Space(10)]
    [Header("SubDue")]
    public Slider SubDueSlider;

    [Space(10)]
    [Header("Create TextBox")]
    public GameObject textBox;

    [Space(10)]
    [Header("Fade")]
    public Image fadeImage;


    [Space(10)]
    [Header("Gun State")]
    public Image handGunImage;
    public Image rifleImage;
    public Image shotGunImage;
    public Image LazerImage;
    public Text AmmoText;
    public Text maxAmmoText;

    [Space(10)]
    [Header("DroneTextBox")]
    public Text DroneText;
    public TextOnOff textOnOff;

    [Space(10)]
    [Header("PlayerDead UI")]
    public GameObject endPanel;

    [Space(10)]
    [Header("ObjectGet UI")]
    public GameObject objectGetText;

    [Space(10)]
    [Header("F Button UI")]
    public GameObject fButton;
    public GameObject fDataButton;
    public GameObject fElecButton;
    public Animator fButtonAnim;
    public Animator fDataButtonAnim;
    public Animator fElecButtonAnim;
    public Text fButtonText;
    public Text fDataButtonText;
    public Text fElecButtonText;

    [Space(10)]
    [Header("Quest Text")]
    public Text questText;
    public Text t_QuestText;

    [Space(10)]
    [Header("Main UI")]
    public GameObject mainUI;

    [Space(10)]
    [Header("Damage")]
    public Image damageImage;
    public float fadeDuration = 2f;
    private Coroutine DamegeImageCor;

    [Space(10)]
    [Header("Map Open UI")]
    public Animator[] mapOpenAnims;

    [Space(10)]
    [Header("Setting UI")]
    public GameObject settingUI;

    void Start()
    {
        mainUIAnim.SetTrigger("Open");

        if(!ObjectManager.Instance.saveObjects[1])
        {
            mainUI.SetActive(true);
            player.deviceOn = true;
        }
        else
        {
            mainUI.SetActive(false);
            player.deviceOn = false;
        }
        t_QuestText.text = GameManager.Instance.userName + "의 기억";
    }

    void Update()
    {
        InteractionGaugeGlow.fillAmount = InteractionGauge.fillAmount;

        LevelUpAnim.SetInteger("Level", ProgressManager.Instance.dataLevel);

        float hp = player.curHp / 100f;
        if (hp < 0) hp = 0;
        hpGauge.fillAmount = hp;
        hpText.text = ((player.curHp).ToString("F2")) + "%";

        float data = ProgressManager.Instance.curData / 100f;
        if (data < 0) data = 0;
        DataGauge.fillAmount = data;
        Datatext.text = ((int)ProgressManager.Instance.curData) + "%";

        //for(int i = 0; i < hpGauges.Length; i++)
        //{
        //    if((int)(player.curHp / 10) > i)
        //        hpGauges[i].SetActive(true);
        //    else
        //        hpGauges[i].SetActive(false);
        //}

        //float hackingCool = player.curHackingCoolTime / player.hackingCoolTime;
        //if (hackingCool <= 0) hackingCoolTime.fillAmount = 0;
        //else
        //    hackingCoolTime.fillAmount = 1 - hackingCool;

        GunImageChange(player.equipWeaponIndex);
    }


    public void CreateTextBox(string text)
    {
        GameObject box = Instantiate(textBox);
        box.GetComponentInChildren<Text>().text = text;
    }

    public void OpenObjectGetText(string s)
    {
        objectGetText.SetActive(true);
        objectGetText.GetComponentInChildren<Text>().text = s;
        StartCoroutine(ObjectTextOut());
    }

    IEnumerator ObjectTextOut()
    {
        yield return new WaitForSeconds(1f);
        objectGetText.SetActive(false);
    }

    void GunImageChange(int gunIndex)
    {
        if(gunIndex == -1)
        {
            handGunImage.gameObject.SetActive(false);
            rifleImage.gameObject.SetActive(false);
            shotGunImage.gameObject.SetActive(false);
            LazerImage.gameObject.SetActive(false);

            AmmoText.gameObject.SetActive(false);
            maxAmmoText.gameObject.SetActive(false);

        }
        else
        {
            AmmoText.gameObject.SetActive(true);
            maxAmmoText.gameObject.SetActive(true);
            AmmoText.text = player.weapon.curAmmo.ToString();
            

            switch (gunIndex)
            {
                case 0:
                    handGunImage.gameObject.SetActive(true);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(false);
                    maxAmmoText.text = "번개";
                    break;

                case 1:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(true);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(false);
                    maxAmmoText.text = MaterialManager.Instance.RifleAmmo.ToString();
                    break;

                case 2:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(true);
                    LazerImage.gameObject.SetActive(false);
                    maxAmmoText.text = MaterialManager.Instance.ShotgunAmmo.ToString();
                    break;

                case 3:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(true);
                    maxAmmoText.text = MaterialManager.Instance.LazerAmmo.ToString();
                    break;
            }
        }
        
    }

    public void InteractionButtonImage(int index, string name)
    {
        fButton.SetActive(false);
        fDataButton.SetActive(false);
        fElecButton.SetActive(false);

        if(index != -1)
        {
            switch(index)
            {
                case 0:
                    fButton.SetActive(true);
                    fButtonAnim.SetTrigger("Quest_In");
                    fButtonText.text = name;
                    break;

                case 1:
                    fDataButton.SetActive(true);
                    fDataButtonAnim.SetTrigger("Quest_In");
                    fDataButtonText.text = name;
                    break;

                case 2:
                    fElecButton.SetActive(true);
                    fElecButtonAnim.SetTrigger("Quest_In");
                    fElecButtonText.text = name;
                    break;
            }
        }
    }

    public void GetDataUI(int amount)
    {
        getDataText.text = "+" + amount + "%";
        deviceUIAnim.SetTrigger("Gain_Data");
    }

    public void GetBatteryUI(float amount)
    {
        getBatteryText.text = "+" + amount + "%";
        deviceUIAnim.SetTrigger("Gain_Battery");
    }

    public void LoseBatteryUI(float amount)
    {
        getBatteryText.text = "-" + amount + "%";
        deviceUIAnim.SetTrigger("Lose_Battery");
    }

    public void DamageImage()
    {
        if (DamegeImageCor != null)
        {
            StopCoroutine(DamegeImageCor);
        }
        DamegeImageCor = StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        Color originalColor = damageImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    public void MapOpenUI(int index)
    {
        mapOpenAnims[index].SetTrigger("UnLock");
        StartCoroutine(MapCloseUI(index));
    }
    IEnumerator MapCloseUI(int index)
    {
        yield return new WaitForSeconds(2f);
        mapOpenAnims[index].SetTrigger("Out");
    }

    public void WeaponBoxUI(int rA, int sA, int lA, int cA)
    {
        riffleIcon.SetActive(false);
        shotgunIcon.SetActive(false);
        lazerIcon.SetActive(false);
        chargerIcon.SetActive(false);

        if(rA > 0)
        {
            riffleIcon.SetActive(true);
            getRiffleAmmoText.text = rA.ToString();
        }

        if(sA > 0)
        {
            shotgunIcon.SetActive(true);
            getShotgunAmmoText.text = sA.ToString();
        }

        if(lA > 0)
        {
            lazerIcon.SetActive(true);
            getLazerAmmoText.text = lA.ToString();
        }

        if(cA > 0)
        {
            chargerIcon.SetActive(true);
            getChargerText.text = cA.ToString();
        }

        weaponBoxAnim.SetTrigger("Spawn");
    }

}

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
    public Animator GetUIAnim;
    public Animator textUIAnim;
    public Animator questUIAnim;
    public Animator LevelPointUIAnim;
    public Animator LevelUpAnim;
    public Player player;
    public Image hpGauge;
    public Text hpText;

    [Space(10)]
    [Header("Interaction")]
    public Image InteractionGauge;
    public Image InteractionGaugeGlow;
    public bool interactionUIOpen = false;
    //public Text detectCount;

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
        t_QuestText.text = GameManager.Instance.userName + "ÀÇ ±â¾ï";
    }

    void Update()
    {
        InteractionGaugeGlow.fillAmount = InteractionGauge.fillAmount;

        LevelUpAnim.SetInteger("Level", ProgressManager.Instance.dataLevel);

        float hp = player.curHp / 100f;
        if (hp < 0) hp = 0;
        hpGauge.fillAmount = hp;

        float data = ProgressManager.Instance.curData / 100f;
        if (data < 0) data = 0;
        DataGauge.fillAmount = data;

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
            maxAmmoText.text = player.weapon.maxAmmo.ToString();

            switch (gunIndex)
            {
                case 0:
                    handGunImage.gameObject.SetActive(true);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(false);
                    break;

                case 1:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(true);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(false);
                    break;

                case 2:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(true);
                    LazerImage.gameObject.SetActive(false);
                    break;

                case 3:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(false);
                    LazerImage.gameObject.SetActive(true);
                    break;
            }
        }
        
    }

    public void InteractionButtonImage(int index)
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
                    break;

                case 1:
                    fDataButton.SetActive(true);
                    break;

                case 2:
                    fElecButton.SetActive(true);
                    break;
            }
        }
    }

    public void GetDataUI(int amount)
    {
        getDataText.text = "+" + amount + "%";
        GetUIAnim.SetTrigger("GetData");
    }

    public void GetBatteryUI(int amount)
    {
        getBatteryText.text = "+" + amount + "%";
        GetUIAnim.SetTrigger("GetBattery");
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
}

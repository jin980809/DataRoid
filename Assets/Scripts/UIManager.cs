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
    public Animator menuUIAnim;
    public Animator textUIAnim;
    public Animator skillButtonUIAnim;
    public Player player;
    public Image hpGauge;
    public Text hpText;
    public Slider shaderVolumm;

    public UI_FadeIn[] fade_Ins;

    [Space(10)]
    [Header("Interaction")]
    public Image InteractionGauge;
    public Image InteractionGaugeGlow;
    public Text detectCount;

    [Space(10)]
    [Header("Inventory")]
    public GameObject inventoryPanel;
    public Text steelAmount;
    public Text gunPowderAmount;
    public Text ExpPieceAmount;
    public Text ammoAmount;
    public Text specialAmmoAmount;
    public Text ExpCapsuleAmount;

    [Space(10)]
    [Header("Creating")]
    public GameObject CreatingUI;
    public GameObject CreatingText;

    [Space(10)]
    [Header("Data")]
    public Image DataGauge;
    public Text Datatext;

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
    public Text AmmoText;

    [Space(10)]
    [Header("DroneTextBox")]
    public Text DroneText;

    [Space(10)]
    [Header("PlayerDead UI")]
    public GameObject endPanel;

    [Space(10)]
    [Header("ObjectGet UI")]
    public GameObject objectGetText;

    [Space(10)]
    [Header("F Button UI")]
    public Image fButton;

    void Start()
    {
        mainUIAnim.SetTrigger("Open");
    }

    void Update()
    {
        TextUpdate();
        InteractionGaugeGlow.fillAmount = InteractionGauge.fillAmount;

        float hp = player.curHp / player.maxHp;
        if (hp < 0) hp = 0;
        hpGauge.fillAmount = hp;

        float data = ProgressManager.Instance.dataAverage / 100f;
        if (data < 0) data = 0;
        DataGauge.fillAmount = 1 - data;

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

    void TextUpdate()
    {
        steelAmount.text = "Steel : " + MaterialManager.Instance.ShotgunAmmo;
        gunPowderAmount.text = "GunPowder : " + MaterialManager.Instance.GunPowder;
        ExpPieceAmount.text = "ExpPiece : " + MaterialManager.Instance.ExpPiece;
        ammoAmount.text = "Ammo : " + MaterialManager.Instance.HandgunAmmo;
        specialAmmoAmount.text = "SpecialAmmo : " + MaterialManager.Instance.RifleAmmo;
        ExpCapsuleAmount.text = "ExpCapsule : " + MaterialManager.Instance.ExpCapsule;
        hpText.text = (int)((player.curHp / player.maxHp) * 100) + "%";
        Datatext.text = (int)ProgressManager.Instance.dataAverage + "%";
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
            AmmoText.gameObject.SetActive(false);
        }
        else
        {
            AmmoText.gameObject.SetActive(true);
            AmmoText.text = player.weapon.curAmmo + " / " + player.weapon.maxAmmo;
            switch (gunIndex)
            {
                case 0:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(true);
                    shotGunImage.gameObject.SetActive(false);
                    break;

                case 1:
                    handGunImage.gameObject.SetActive(true);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(false);
                    break;

                case 2:
                    handGunImage.gameObject.SetActive(false);
                    rifleImage.gameObject.SetActive(false);
                    shotGunImage.gameObject.SetActive(true);
                    break;
            }
        }
        
    }

    public void InteractionButtonImage(bool isActive)
    {
        fButton.gameObject.SetActive(isActive);
    }
}

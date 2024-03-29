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

    public Animator uiAnim;
    public Player player;
    public GameObject aim;
    public Image hpGauge;
    public Text hpText;

    [Space(10)]
    [Header("Interaction")]
    public Slider InteractionGauge;
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
    [Header("EXP")]
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
    [Header("Skill CoolTime")]
    public Image hackingCoolTime;
    public Image stunCoolTime;

    [Space(10)]
    [Header("Gun State")]
    public Image handGunImage;
    public Image rifleImage;
    public Image shotGunImage;
    public Text curAmmoText;
    public Text maxAmmoText;

    [Space(10)]
    [Header("DroneTextBox")]
    public Text DroneText;



    void Start()
    {

    }

    void Update()
    {
        TextUpdate();
        
        float hp = player.curHp / player.maxHp;
        if (hp < 0) hp = 0;
        hpGauge.fillAmount = hp;

        float hackingCool = player.curHackingCoolTime / player.hackingCoolTime;
        if (hackingCool <= 0) hackingCoolTime.fillAmount = 0;
        else
            hackingCoolTime.fillAmount = 1 - hackingCool;


        float stunCool = player.curStunCoolTime / player.stunCoolTime;
        if (stunCool < 0) stunCool = 0;
        stunCoolTime.fillAmount = 1 - stunCool;

        GunImageChange(player.equipWeaponIndex);
    }

    void TextUpdate()
    {
        steelAmount.text = "Steel : " + MaterialManager.Instance.Steel;
        gunPowderAmount.text = "GunPowder : " + MaterialManager.Instance.GunPowder;
        ExpPieceAmount.text = "ExpPiece : " + MaterialManager.Instance.ExpPiece;
        ammoAmount.text = "Ammo : " + MaterialManager.Instance.Ammo;
        specialAmmoAmount.text = "SpecialAmmo : " + MaterialManager.Instance.SpecialAmmo;
        ExpCapsuleAmount.text = "ExpCapsule : " + MaterialManager.Instance.ExpCapsule;
        hpText.text = (int)((player.curHp / player.maxHp) * 100) + "%";
    }

    public void CreateTextBox(string text)
    {
        GameObject box = Instantiate(textBox);
        box.GetComponentInChildren<Text>().text = text;
    }

    void GunImageChange(int gunIndex)
    {
        if(gunIndex == -1)
        {
            handGunImage.gameObject.SetActive(false);
            rifleImage.gameObject.SetActive(false);
            shotGunImage.gameObject.SetActive(false);
            curAmmoText.gameObject.SetActive(false);
            maxAmmoText.gameObject.SetActive(false);
        }
        else
        {
            curAmmoText.gameObject.SetActive(true);
            maxAmmoText.gameObject.SetActive(true);
            curAmmoText.text = player.weapon.curAmmo + "";
            maxAmmoText.text = player.weapon.maxAmmo + "";
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
}

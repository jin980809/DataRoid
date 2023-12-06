using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Player player;
    public GameObject aim;
    public Slider hpGauge;

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
    public Slider ExpGauge;

    [Space(10)]
    [Header("HackingUI")]
    public GameObject HackingUI;
    public Slider HackingGauge;
    public Text EnemyModelName;
    public Text IMEI;
    public Text EnemyElec;
    public Text HitArea;


    [Space(10)]
    [Header("MiniMap")]
    public GameObject MiniMap;
    public Camera miniMapCamera;

    void Update()
    {
        TextUpdate();
        
        float hp = player.curHp / player.maxHp;
        if (hp < 0) hp = 0;
        hpGauge.value = hp;

    }

    void TextUpdate()
    {
        steelAmount.text = "Steel : " + MaterialManager.Instance.Steel;
        gunPowderAmount.text = "GunPowder : " + MaterialManager.Instance.GunPowder;
        ExpPieceAmount.text = "ExpPiece : " + MaterialManager.Instance.ExpPiece;
        ammoAmount.text = "Ammo : " + MaterialManager.Instance.Ammo;
        specialAmmoAmount.text = "SpecialAmmo : " + MaterialManager.Instance.SpecialAmmo;
        ExpCapsuleAmount.text = "ExpCapsule : " + MaterialManager.Instance.ExpCapsule;
    }
}

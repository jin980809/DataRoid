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

    [Space(10)]
    public Slider InteractionGauge;
    public Text detectCount;
    public GameObject aim;
    public GameObject inventoryPanel;
    public Text steelAmount;
    public Text gunPowderAmount;
    public Text ammoAmount;
    public Text specialAmmoAmount;
    public GameObject CreatingUI;
    public GameObject CreatingText;

    void Update()
    {
        TextUpdate();
    }

    void TextUpdate()
    {
        steelAmount.text = "Steel : " + MaterialManager.Instance.Steel;
        gunPowderAmount.text = "GunPowder : " + MaterialManager.Instance.GunPowder;
        ammoAmount.text = "Ammo : " + MaterialManager.Instance.Ammo;
        specialAmmoAmount.text = "SpecialAmmo : " + MaterialManager.Instance.SpecialAmmo;
    }
}

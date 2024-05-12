using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Range(0, 255)]
    public float equatorColor;
    [Range(0, 100)]
    public float volumm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        equatorColor = UIManager.Instance.shaderVolumm.value;
        SetEquatorColor();
    }

    void SetEquatorColor()
    {
        RenderSettings.ambientEquatorColor = new Color(equatorColor/255f, equatorColor/255f, equatorColor/255f);
    }
}

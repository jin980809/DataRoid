using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Flash : MonoBehaviour
{
    public Volume volume;
    public CanvasGroup AplhaController;

    private bool on = false;

    void Start()
    {
        
    }

    void Update()
    {

        if (on == true)
        {
            Time.timeScale = .1f;

            AplhaController.alpha = AplhaController.alpha - Time.deltaTime * 4;
            volume.GetComponent<Volume>().weight = volume.GetComponent<Volume>().weight - Time.deltaTime * 2;

            if(AplhaController.alpha <= 0)
            {
                volume.GetComponent<Volume>().weight = 0;
                AplhaController.alpha = 0;
                Time.timeScale = 1;
                on = false;
            }
        }
    }

    public void FlashBanged()
    {
        volume.GetComponent<Volume>().weight = 1;
        on = true;
        AplhaController.alpha = 1;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FlickeringLights : MonoBehaviour
{
    public Light spotLight;
    public Light pointLight;

    public GameObject Target;

    public float maxWait = 1;
    public float maxFlicker = 0.2f;

    float timer;
    float interval;
    float Curtime;

    void Update()
    {
        timer += Time.deltaTime;
        Curtime += Time.deltaTime;

        if (timer > interval)
        {
            if (Curtime < 5)
            {
                ToggleLightSetup();
            }

            else
            {
                LightSwitch();
            }
        }


    }

    void ToggleLightSetup()
    {
        spotLight.enabled = !spotLight.enabled;
        pointLight.enabled = !pointLight.enabled;

        if (spotLight.enabled)
        {
            interval = Random.Range(0, maxWait);

            Target.SetActive(false);
        }
        else if (pointLight.enabled)
        {
            interval = Random.Range(0, maxWait);
        }
        else if (!spotLight.enabled)
        {
            interval = Random.Range(0, maxFlicker);

            Target.SetActive(true);
        }
        else if (!pointLight.enabled)
        {
            interval = Random.Range(0, maxFlicker);
        }

        timer = 0;
    }

    void LightSwitch()
    {
        if (Curtime < 6.5)
            {
            spotLight.intensity = 0f;
            pointLight.intensity = 0f;
            Target.SetActive(true);
        }

        else 
        {
            spotLight.intensity = 5f;
            pointLight.intensity = 0.1f;
            Target.SetActive(false);
        }
    }
}

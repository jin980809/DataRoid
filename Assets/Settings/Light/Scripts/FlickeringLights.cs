using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public Light spotLight;
    public Light pointLight;

    public GameObject Target_1;
    public GameObject Target_2;

    public float maxWait = 1;
    public float maxFlicker = 0.2f;

    float timer;
    float interval;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            ToggleLightSetup();
        }
    }

    void ToggleLightSetup()
    {
        spotLight.enabled = !spotLight.enabled;
        pointLight.enabled = !pointLight.enabled;

        if (spotLight.enabled)
        {
            interval = Random.Range(0, maxWait);

            Target_1.SetActive(false);
            Target_2.SetActive(false);
        }
        else if (pointLight.enabled)
        {
            interval = Random.Range(0, maxWait);
        }
        else if (!spotLight.enabled)
        {
            interval = Random.Range(0 ,maxFlicker);

            Target_1.SetActive(true);
            Target_2.SetActive(true);
        }
        else if (!pointLight.enabled)
        {
            interval = Random.Range(0, maxFlicker);
        }

        timer = 0;
    }
}

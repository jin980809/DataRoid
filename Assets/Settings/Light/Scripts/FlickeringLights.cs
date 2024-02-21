using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public Light spotLight;
    public Light pointLight;

    public GameObject Target;

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
}

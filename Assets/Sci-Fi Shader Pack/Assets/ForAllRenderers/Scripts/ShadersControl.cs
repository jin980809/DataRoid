using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadersControl : MonoBehaviour 
{

    float timer = 1.2f;
    float timerSciFi = 0;

    bool back = true;

    void Update()
    {
        if (GameManager.Instance.isPlayerDead)
        {
            if (timer <= -0.2f)
                back = false;
            if (timer > 1.2f)
                back = true;

            //if (back == false)
            //{
            //    timer += Time.deltaTime / 2;
            //}
            if (back)
            {
                timer -= Time.deltaTime / 2;
            }

            Shader.SetGlobalFloat("_ShaderDisplacement", timer);
            Shader.SetGlobalFloat("_ShaderHologramDisplacement", 1 - timer);
            Shader.SetGlobalFloat("_ShaderDissolve", 1 - timer);

            if (timerSciFi > 1.2f)
                timerSciFi = 0;

            timerSciFi += Time.deltaTime / 3;

            Shader.SetGlobalFloat("_ShaderSciFi", timerSciFi);

        }
    }
}

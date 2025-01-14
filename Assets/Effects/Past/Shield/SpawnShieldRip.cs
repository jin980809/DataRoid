using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnShieldRip : MonoBehaviour
{
    public GameObject shieldRipples;
    private VisualEffect shieldRipplesVFX;

    private void OnCollisionEnter(Collision co)
    {
        if (co.gameObject.tag == "Damaged")
        {
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRipplesVFX = ripples.GetComponent<VisualEffect>();
            shieldRipplesVFX.SetVector3("SphereCenter", co.contacts[0].point);

            Destroy(ripples, 2);
        }
    }
}

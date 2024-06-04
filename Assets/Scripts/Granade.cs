using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float cookingTime;
    GameObject range;
    public GameObject effect;
    private Rigidbody rb;

    void Start()
    {
        range = transform.GetChild(0).transform.gameObject;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Damage());
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(cookingTime);
        range.SetActive(true);
        effect.SetActive(true);
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.1f);
        range.SetActive(false);

        yield return new WaitForSeconds(2.9f);
        effect.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }

}

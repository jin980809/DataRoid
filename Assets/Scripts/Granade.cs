using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float cookingTime;
    GameObject range;
    void Start()
    {
        range = transform.GetChild(0).transform.gameObject;
        StartCoroutine(Damage());
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(cookingTime);
        range.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }
}

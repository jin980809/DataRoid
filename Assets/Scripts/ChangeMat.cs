using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour
{
    public Material changeMat;
    SkinnedMeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        EndChangeMat();
    }

    public void EndChangeMat()
    {
        if(GameManager.Instance.isPlayerDead)
        {
            mesh.material = changeMat;
        }
    }
}

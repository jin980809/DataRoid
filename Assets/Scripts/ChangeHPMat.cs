using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHPMat : MonoBehaviour
{
    public Material[] changeMat;
    SkinnedMeshRenderer mesh;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isPlayerDead)
        {
            if(player.curHp > 66f)
            {
                mesh.material = changeMat[0];
            }
            else if(player.curHp <= 66f && player.curHp > 33f)
            {
                mesh.material = changeMat[1];
            }
            else if(player.curHp <= 33f && player.curHp > 15f)
            {
                mesh.material = changeMat[2];
            }
            else
            {
                mesh.material = changeMat[3];
            }
        }
    }
}

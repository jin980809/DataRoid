using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetObject : SetActiveObj
{
    public int targetEnemyID;

    void Update()
    {
        Debug.Log(EnemyManager.Instance.enemyInfo[targetEnemyID]["isDead"]);

        bool isEnemyDead;
        if (EnemyManager.Instance.enemyInfo[targetEnemyID]["isDead"].ToString() == "1")
            isEnemyDead = true;
        else
            isEnemyDead = false;


        if (isEnemyDead == targetObjActive && !isDone)
        {
            ObjectSet();
            isDone = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObj : MonoBehaviour
{
    public enum Type
    {
        Interaction = 1,
        Collider = 2,
    };
    public Type activeType;
    public bool ObjActive;
    public bool targetObjActive;
    public GameObject targetObj;

    [Space(10)]
    [Header("Interaction")]
    public Interaction interaction;

    [Space(10)]
    [Header("Collider")]
    public new Collider collider;

    // Update is called once per frame
    void Update()
    {
        if (targetObj.activeSelf == targetObjActive)
        { 
            switch (activeType)
            {
                case Type.Interaction:
                    interaction.enabled = ObjActive;
                    break;

                case Type.Collider:
                    collider.enabled = ObjActive;
                    break;
            }
        }
    }
}

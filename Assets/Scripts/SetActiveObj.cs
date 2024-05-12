using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObj : MonoBehaviour
{
    public enum Type
    {
        Interaction = 1,
        Collider = 2,
        Object = 3,
    };
    public Type activeType;
    public bool ObjActive;
    public bool targetObjActive;
    public GameObject targetObj;
    private bool isDone = false;
    [Space(10)]
    [Header("Interaction")]
    public Interaction interaction;

    [Space(10)]
    [Header("Collider")]
    public new Collider collider;

    [Space(10)]
    [Header("Object")]
    public GameObject gObj;

    // Update is called once per frame
    void Update()
    {
        if (targetObj.activeSelf == targetObjActive && !isDone)
        {
            ObjectSet();
            isDone = true;
        }
    }

    void ObjectSet()
    {
        switch (activeType)
        {
            case Type.Interaction:
                interaction.enabled = ObjActive;
                break;

            case Type.Collider:
                collider.enabled = ObjActive;
                break;

            case Type.Object:
                gObj.SetActive(ObjActive);
                break;
        }
    }
}

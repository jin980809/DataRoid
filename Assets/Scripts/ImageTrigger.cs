using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTrigger : MonoBehaviour
{
    public Player player;
    public GameObject canvas;
    public GameObject p_Obj;
    private GameObject inst_Obj;
    public bool isSave;
    public int objectID;

    bool qDown;

    void Start()
    {
        if (isSave)
        {
            transform.gameObject.SetActive(ObjectManager.Instance.saveObjects[objectID]);
        }
    }

    void Update()
    {
        if (inst_Obj != null)
        {
            if (!inst_Obj.activeSelf)
            {
                player.isCommunicate = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.isCommunicate = true;
            inst_Obj = Instantiate(p_Obj, canvas.transform);
            GetComponent<BoxCollider>().enabled = false;

            if (isSave)
            {
                ObjectManager.Instance.saveObjects[objectID] = false;
            }

            //gameObject.SetActive(false);
        }
    }
}

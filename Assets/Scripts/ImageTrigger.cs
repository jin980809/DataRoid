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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.isCommunicate = true;
            inst_Obj = Instantiate(p_Obj, canvas.transform);
            inst_Obj.GetComponent<DiscriptionImage>().player = other.GetComponent<Player>();
            GetComponent<BoxCollider>().enabled = false;

            if (isSave)
            {
                ObjectManager.Instance.saveObjects[objectID] = false;
            }

            //gameObject.SetActive(false);
        }
    }
}

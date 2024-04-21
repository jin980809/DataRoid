using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRotater : MonoBehaviour, IDragHandler
{
    public float rotateSpeed = 10;

    public Camera rotation_Camera;
    public Player player;
    bool qDown;

    public void OnDrag(PointerEventData eventData)
    {
        float x = eventData.delta.x * Time.deltaTime * rotateSpeed;
        float y = eventData.delta.y * Time.deltaTime * rotateSpeed;

        transform.Rotate(y, -x, 0, Space.World);

        //Debug.Log("Drag");
    }

    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");

        if (qDown)
        {
            ExitImage();
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = rotation_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Data")))
            {
                if (hit.transform.GetComponent<RotateObjectHasData>() != null)
                {
                    if (!hit.transform.GetComponent<RotateObjectHasData>().getData)
                    {
                        hit.transform.GetComponent<RotateObjectHasData>().getData = true;
                        MaterialManager.Instance.UFSData += 1;
                        UIManager.Instance.OpenObjectGetText("Get Data");
                    }
                }
            }
        }
    }

    void ExitImage()
    {
        player.isCommunicate = false;
        transform.gameObject.SetActive(false);
    }
}

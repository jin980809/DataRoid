using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRotater : MonoBehaviour, IDragHandler
{
    public float rotateSpeed = 10;

    public Camera rotation_Camera;
    public GameObject mainCamera;
    public Player player;
    public Interaction interaction;
    bool qDown;

    public void OnDrag(PointerEventData eventData)
    {
        float x = eventData.delta.x * Time.deltaTime * rotateSpeed;
        float y = eventData.delta.y * Time.deltaTime * rotateSpeed;


        if(rotation_Camera.transform.rotation.eulerAngles.y == 90)
            transform.Rotate(0, -x, -y, Space.World);
        else if(rotation_Camera.transform.rotation.eulerAngles.y == 180)
            transform.Rotate(-y, -x, 0, Space.World);
        else if (rotation_Camera.transform.rotation.eulerAngles.y == 0)
            transform.Rotate(y, -x, 0, Space.World);
        else if(rotation_Camera.transform.rotation.eulerAngles.y == 270)
            transform.Rotate(0, -x, y, Space.World);

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
                        hit.transform.gameObject.SetActive(false);
                        interaction.hasData = false;
                    }
                }
            }
        }
    }

    void ExitImage()
    {
        player.isCommunicate = false;
        mainCamera.SetActive(true);
        rotation_Camera.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }
}

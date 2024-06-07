using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectRotater : MonoBehaviour, IDragHandler
{
    public float rotateSpeed = 10;

    public Camera rotation_Camera;
    public GameObject mainCamera;
    public Player player;
    public Interaction interaction;
    bool qDown;
    bool isScanning = false;

    public bool isQuestUpdate;

    public string questText;
    public bool useUSFDataVariation;
    public string questTextPlus;

    private float startValue = -1f; // 초기 값
    private float endValue = 7.6f; // 목표 값
    public float scanIncreaseSpeed = 1f; // 증가 속도
    private float currentValue; // 현재 값

    public SpriteRenderer scanImage;

    Material material;

    void Start()
    {
        material = scanImage.material;
    }

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

        if (qDown && !isScanning)
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
                        if (isQuestUpdate)
                        {
                            UIManager.Instance.questUIAnim.SetTrigger("Quest_In");

                            if (useUSFDataVariation)
                            {
                                UIManager.Instance.questText.text = questText + MaterialManager.Instance.UFSData.ToString() + questTextPlus;
                            }
                            else
                            {
                                UIManager.Instance.questText.text = questText;
                            }
                        }
                        //UIManager.Instance.OpenObjectGetText("Get Data");
                        UIManager.Instance.deviceUIAnim.SetTrigger("Scan_Code");
                        isScanning = true;
                        StartCoroutine(ScanOut(hit.transform.gameObject));
                        //hit.transform.gameObject.SetActive(false);
                        
                    }
                }
            }
        }
    }

    IEnumerator ScanOut(GameObject gobj)
    {

        while (currentValue < endValue)
        {
            currentValue += scanIncreaseSpeed * Time.deltaTime;
            material.SetFloat("_DirectionalGlowFadeFade", currentValue);
            yield return null;
        }

        currentValue = endValue;

        yield return new WaitForSeconds(0.1f);
        gobj.SetActive(false);
        interaction.hasData = false;
        isScanning = false;
    }    

    void ExitImage()
    {
        player.isCommunicate = false;
        mainCamera.SetActive(true);
        rotation_Camera.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
    }
}

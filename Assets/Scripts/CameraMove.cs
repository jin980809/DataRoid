using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Transform cameraArm;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private GameObject cineCameraObj;

    [Space(10)]
    [SerializeField]
    private Vector3 walkCameraOffset;
    [SerializeField]
    private Vector3 runCameraOffset;    
    [SerializeField]
    private Vector3 shotCameraOffset;


    private Player player;

    [Space(10)]
    public float followSpeed;
    public float Sensitivity;
    private Vector3 smoothCamera;
    public float zoomSpeed;

    [Space(10)]
    public float walkHeight;
    public float runHeight;
    public float zoomHeight;
    float hoverOffset;
    public float walkHeightSpeed;
    public float runHeightSpeed;
    public float zoomHeightSpeed;

    Vector3 valo;
    void Awake()
    {
        player = target.gameObject.GetComponentInParent<Player>();
    }

    void Update()
    {
        LookAround();

        followCam();

        SwitchCamera();
    }

    void SwitchCamera()
    {
        CinemachineVirtualCamera cineVirCam = cineCameraObj.GetComponent<CinemachineVirtualCamera>();
        Cinemachine3rdPersonFollow cine3rd = cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        Vector3 currCameraOffset = cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset;

        if(player.isZoom)
        {
            cine3rd.ShoulderOffset = Vector3.SmoothDamp(currCameraOffset, shotCameraOffset, ref valo, zoomSpeed);
        }
        else
        {
            if(player.isRun && !player.isReload)
            {
                cine3rd.ShoulderOffset = Vector3.SmoothDamp(currCameraOffset, runCameraOffset, ref valo, 0.1f);
            }
            else
            {
                cine3rd.ShoulderOffset = Vector3.SmoothDamp(currCameraOffset, walkCameraOffset, ref valo, 0.1f);
            }
        }
    }

        

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * Sensitivity;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 325f, 361f);
        }

        if(!player.isInteraction)
            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void followCam()
    {
        if(player.isWalk)
        {
            hoverOffset = Mathf.Sin(Time.time * walkHeightSpeed) * walkHeight;
        }
        if(player.isRun)
        {
            hoverOffset = Mathf.Sin(Time.time * runHeightSpeed) * runHeight;
        }
        if(player.isZoom)
        {
            hoverOffset = Mathf.Sin(Time.time * zoomHeightSpeed) * zoomHeight;
        }

        if(!player.isWalk)
        {
            hoverOffset = Mathf.Sin(Time.time * 0) * 0;
        }


        float a = Mathf.Lerp(transform.position.y, target.position.y + hoverOffset, Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, target.position.z), followSpeed * Time.deltaTime);
    }
}
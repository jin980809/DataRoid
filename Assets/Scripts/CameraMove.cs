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

    Vector3 valo;
    void Awake()
    {
        player = target.gameObject.GetComponentInParent<Player>();
    }

    void Update()
    {
        LookAround();

        followCam();

        switchCamera();
    }

    void switchCamera()
    {
        CinemachineVirtualCamera cineVirCam = cineCameraObj.GetComponent<CinemachineVirtualCamera>();
        Cinemachine3rdPersonFollow cine3rd = cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        Vector3 currCameraOffset = cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset;

        if(player.isZoom)
        {
            cine3rd.ShoulderOffset = Vector3.SmoothDamp(cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset, shotCameraOffset, ref valo, 0.1f);
        }
        else
        {
            if(player.isRun && !player.isReload)
            {
                cine3rd.ShoulderOffset = Vector3.SmoothDamp(cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset,runCameraOffset, ref valo, 0.1f);
            }
            else
            {
                cine3rd.ShoulderOffset = Vector3.SmoothDamp(cineVirCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset, walkCameraOffset, ref valo, 0.1f);
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

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void followCam()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}
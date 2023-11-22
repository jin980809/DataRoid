using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVControl : MonoBehaviour
{
    float hAxis;
    bool iDown;

    public float rotateSpeed;
    public float rotateAngle;

    Vector3 moveVec;
    Vector3 defVec;
    Vector3 rotateVec;

    public Camera mainCamera;
    public Player player;
    public CameraMove cameraMove;

    void Start()
    {
        defVec = transform.rotation.eulerAngles;
        rotateVec = defVec;
        //Debug.Log(defVec);
    }

    void Update()
    {
        GetInput();
        Move();
        CCTVOut();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        iDown = Input.GetButton("Cancel");
    }

    void Move()
    {

        moveVec = new Vector3(0f, hAxis, 0f).normalized;

        if (defVec.y + rotateAngle / 2 < rotateVec.y && moveVec == Vector3.up)
        {
            moveVec = Vector3.zero;
        }

        if (defVec.y - rotateAngle / 2 > rotateVec.y &&  moveVec == Vector3.down)
        {
            moveVec = Vector3.zero;
        }


        rotateVec += moveVec * rotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rotateVec);

        //Debug.Log(rotateVec);
    }

    void CCTVOut()
    {
        if(iDown)
        {
            mainCamera.enabled = true;
            player.enabled = true;
            cameraMove.enabled = true;

            this.gameObject.SetActive(false);
        }
    }
}

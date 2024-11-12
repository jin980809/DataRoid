using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject objectA;   // 충돌할 오브젝트 A
    public GameObject objectB;   // 활성화할 오브젝트 B
    public GameObject objectC;   // 활성화할 오브젝트 C
    public GameObject objectD;   // 회전시킬 오브젝트 D

    private bool isNearObjectA = false;   // 플레이어가 오브젝트 A에 근접했는지 여부
    private bool objectsActivated = false; // 오브젝트 B와 C가 활성화된 상태인지 확인

    void Update()
    {
        // 상호작용 키 "F"를 눌렀을 때
        if (isNearObjectA && Input.GetKeyDown(KeyCode.F))
        {
            if (!objectsActivated)
            {
                // 첫 번째 상호작용: 오브젝트 B와 C 활성화
                objectB.SetActive(true);
                objectC.SetActive(true);
                objectsActivated = true;
            }
            else
            {
                // 두 번째 상호작용: 오브젝트 D의 Y 축 회전 값에 180도 추가
                RotateObjectD();
            }
        }

        // ESC 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Escape) && objectsActivated)
        {
            // 오브젝트 B와 C 비활성화
            objectB.SetActive(false);
            objectC.SetActive(false);
            objectsActivated = false;
        }
    }

    private void RotateObjectD()
    {
        // 오브젝트 D의 현재 회전값 가져오기
        Vector3 currentRotation = objectD.transform.rotation.eulerAngles;
        // Y 축 값에 180도 추가 후 다시 설정
        objectD.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + 180f, currentRotation.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 오브젝트 A와 충돌했는지 확인
        if (other.gameObject == objectA)
        {
            isNearObjectA = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 오브젝트 A와 거리가 멀어지면 상호작용 불가 상태로 변경
        if (other.gameObject == objectA)
        {
            isNearObjectA = false;
        }
    }
}
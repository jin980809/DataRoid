using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject objectA;   // �浹�� ������Ʈ A
    public GameObject objectB;   // Ȱ��ȭ�� ������Ʈ B
    public GameObject objectC;   // Ȱ��ȭ�� ������Ʈ C
    public GameObject objectD;   // ȸ����ų ������Ʈ D

    private bool isNearObjectA = false;   // �÷��̾ ������Ʈ A�� �����ߴ��� ����
    private bool objectsActivated = false; // ������Ʈ B�� C�� Ȱ��ȭ�� �������� Ȯ��

    void Update()
    {
        // ��ȣ�ۿ� Ű "F"�� ������ ��
        if (isNearObjectA && Input.GetKeyDown(KeyCode.F))
        {
            if (!objectsActivated)
            {
                // ù ��° ��ȣ�ۿ�: ������Ʈ B�� C Ȱ��ȭ
                objectB.SetActive(true);
                objectC.SetActive(true);
                objectsActivated = true;
            }
            else
            {
                // �� ��° ��ȣ�ۿ�: ������Ʈ D�� Y �� ȸ�� ���� 180�� �߰�
                RotateObjectD();
            }
        }

        // ESC Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.Escape) && objectsActivated)
        {
            // ������Ʈ B�� C ��Ȱ��ȭ
            objectB.SetActive(false);
            objectC.SetActive(false);
            objectsActivated = false;
        }
    }

    private void RotateObjectD()
    {
        // ������Ʈ D�� ���� ȸ���� ��������
        Vector3 currentRotation = objectD.transform.rotation.eulerAngles;
        // Y �� ���� 180�� �߰� �� �ٽ� ����
        objectD.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + 180f, currentRotation.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������Ʈ A�� �浹�ߴ��� Ȯ��
        if (other.gameObject == objectA)
        {
            isNearObjectA = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ������Ʈ A�� �Ÿ��� �־����� ��ȣ�ۿ� �Ұ� ���·� ����
        if (other.gameObject == objectA)
        {
            isNearObjectA = false;
        }
    }
}
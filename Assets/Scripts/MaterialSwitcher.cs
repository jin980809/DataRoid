using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material material1; // ù ��° ���͸���
    public Material material2; // �� ��° ���͸���
    public float duration = 3f; // �� ���� �ð�

    private SkinnedMeshRenderer objectRenderer;
    private bool isMaterial1Active = true;

    void Start()
    {
        objectRenderer = GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(SwitchMaterials());
    }

    IEnumerator SwitchMaterials()
    {
        float elapsedTime = 0f;
        float initialInterval = 0.3f; // �ʱ� ��ü �ֱ�
        float intervalDecreaseRate = initialInterval / duration;

        while (elapsedTime < duration)
        {
            // ���͸��� ��ü
            if (isMaterial1Active)
            {
                objectRenderer.material = material1;
            }
            else
            {
                objectRenderer.material = material2;
            }

            isMaterial1Active = !isMaterial1Active;

            // ��ü �ֱ� ����
            float currentInterval = Mathf.Max(0.05f, initialInterval - (intervalDecreaseRate * elapsedTime));
            yield return new WaitForSeconds(currentInterval);

            elapsedTime += currentInterval;
        }

        // 3�� �Ŀ� ���������� �ϳ��� ���͸���� ����
        objectRenderer.material = material1;
    }
}

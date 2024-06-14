using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material material1; // 첫 번째 머터리얼
    public Material material2; // 두 번째 머터리얼
    public float duration = 3f; // 총 지속 시간

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
        float initialInterval = 0.3f; // 초기 교체 주기
        float intervalDecreaseRate = initialInterval / duration;

        while (elapsedTime < duration)
        {
            // 머터리얼 교체
            if (isMaterial1Active)
            {
                objectRenderer.material = material1;
            }
            else
            {
                objectRenderer.material = material2;
            }

            isMaterial1Active = !isMaterial1Active;

            // 교체 주기 조절
            float currentInterval = Mathf.Max(0.05f, initialInterval - (intervalDecreaseRate * elapsedTime));
            yield return new WaitForSeconds(currentInterval);

            elapsedTime += currentInterval;
        }

        // 3초 후에 최종적으로 하나의 머터리얼로 설정
        objectRenderer.material = material1;
    }
}

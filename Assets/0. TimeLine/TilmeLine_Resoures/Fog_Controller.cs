using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Controller : MonoBehaviour
{
    // A�� B�� �Ȱ� �е� ��
    public float fogDensityA = 0.04f;
    public float fogDensityB = 0.01f;

    // ���� ���¸� ��Ÿ���� �÷���
    private bool isUsingFogDensityA = true;

    void Start()
    {
        // �Ȱ� Ȱ��ȭ
        RenderSettings.fog = true;

        // �ʱ� �е��� A�� ����
        SetFogDensity(fogDensityA);
    }

    void Update()
    {
        // ����� �Է��� ���� �е� ��ȯ (��: Space Ű ���)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFogDensity();
        }
    }

    // ���� �Ȱ� �е��� ��ȯ�ϴ� �Լ�
    public void ToggleFogDensity()
    {
        if (isUsingFogDensityA)
        {
            SetFogDensity(fogDensityB);
        }
        else
        {
            SetFogDensity(fogDensityA);
        }

        // ���� ��ȯ
        isUsingFogDensityA = !isUsingFogDensityA;
    }

    // �Ȱ� �е��� �����ϴ� �Լ�
    private void SetFogDensity(float density)
    {
        RenderSettings.fogDensity = density;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Controller : MonoBehaviour
{
    // A와 B의 안개 밀도 값
    public float fogDensityA = 0.04f;
    public float fogDensityB = 0.01f;

    // 현재 상태를 나타내는 플래그
    private bool isUsingFogDensityA = true;

    void Start()
    {
        // 안개 활성화
        RenderSettings.fog = true;

        // 초기 밀도를 A로 설정
        SetFogDensity(fogDensityA);
    }

    void Update()
    {
        // 사용자 입력을 통해 밀도 전환 (예: Space 키 사용)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFogDensity();
        }
    }

    // 현재 안개 밀도를 전환하는 함수
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

        // 상태 전환
        isUsingFogDensityA = !isUsingFogDensityA;
    }

    // 안개 밀도를 설정하는 함수
    private void SetFogDensity(float density)
    {
        RenderSettings.fogDensity = density;
    }
}
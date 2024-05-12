using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeIn : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup 오브젝트를 가리키는 변수
    public float fadeDuration = 0.5f; // 서서히 나타나는 데 걸리는 시간

    private float fadeTimer = 0f; // 페이드 타이머
    private bool isFading = false; // 페이드 중인지 여부

    void Start()
    {
        // 만약에 CanvasGroup이 할당되지 않았다면, 자신의 CanvasGroup을 찾아 할당
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // 초기화: Alpha 값을 0으로 설정
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        // 페이드 중이 아니라면 함수를 종료
        if (!isFading)
            return;

        // 페이드 타이머를 증가시키고, Alpha 값을 조절하여 서서히 나타나게 함
        fadeTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(fadeTimer / fadeDuration);
        canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);

        // 페이드가 완료되면 페이드 중인 상태를 false로 변경
        if (progress >= 1f)
        {
            isFading = false;
            fadeTimer = 0f;
        }
    }

    // 외부에서 호출하여 페이드를 시작하는 함수
    public void StartFadeIn()
    {
        isFading = true;
    }
}
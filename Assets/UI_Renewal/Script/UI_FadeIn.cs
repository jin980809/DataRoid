using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeIn : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup ������Ʈ�� ����Ű�� ����
    public float fadeDuration = 0.5f; // ������ ��Ÿ���� �� �ɸ��� �ð�

    private float fadeTimer = 0f; // ���̵� Ÿ�̸�
    private bool isFading = false; // ���̵� ������ ����

    void Start()
    {
        // ���࿡ CanvasGroup�� �Ҵ���� �ʾҴٸ�, �ڽ��� CanvasGroup�� ã�� �Ҵ�
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // �ʱ�ȭ: Alpha ���� 0���� ����
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        // ���̵� ���� �ƴ϶�� �Լ��� ����
        if (!isFading)
            return;

        // ���̵� Ÿ�̸Ӹ� ������Ű��, Alpha ���� �����Ͽ� ������ ��Ÿ���� ��
        fadeTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(fadeTimer / fadeDuration);
        canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);

        // ���̵尡 �Ϸ�Ǹ� ���̵� ���� ���¸� false�� ����
        if (progress >= 1f)
        {
            isFading = false;
            fadeTimer = 0f;
        }
    }

    // �ܺο��� ȣ���Ͽ� ���̵带 �����ϴ� �Լ�
    public void StartFadeIn()
    {
        isFading = true;
    }
}
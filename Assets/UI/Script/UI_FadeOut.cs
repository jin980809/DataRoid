using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup ������Ʈ�� ����Ű�� ����
    public float fadeDuration = 0.5f; // ������ ������� �� �ɸ��� �ð�

    private float fadeTimer = 0f; // ���̵� Ÿ�̸�
    private bool isFading = false; // ���̵� ������ ����

    void Start()
    {
        // ���࿡ CanvasGroup�� �Ҵ���� �ʾҴٸ�, �ڽ��� CanvasGroup�� ã�� �Ҵ�
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // �ʱ�ȭ: Alpha ���� 1�� ����
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        // ���̵� ���� �ƴ϶�� �Լ��� ����
        if (!isFading)
            return;

        // ���̵� Ÿ�̸Ӹ� ������Ű��, Alpha ���� �����Ͽ� ������ ������� ��
        fadeTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(fadeTimer / fadeDuration);
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);

        // ���̵尡 �Ϸ�Ǹ� ���̵� ���� ���¸� false�� ����
        if (progress >= 1f)
        {
            isFading = false;
            fadeTimer = 0f;
        }
    }

    // �ܺο��� ȣ���Ͽ� ���̵带 �����ϴ� �Լ�
    public void StartFadeOut()
    {
        isFading = true;
    }
}
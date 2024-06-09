using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNameUI : MonoBehaviour
{
    public Player player;
    public GameObject nameUI; // 积己等 利 UI
    public bool isNameTagOpen;
    public GameObject nameUIPrefab;
    public Canvas canvas;
    public Camera mainCamera;
    private float distance;
    private Text distText;
    public bool isBatteryTag;

    void Update()
    {
        if(mainCamera.gameObject.activeSelf)
            ObjectUI();
    }

    public void ObjectUI()
    {
        if (isNameTagOpen)
        {
            CreateEnemyUI();

            if(!isBatteryTag)
                distText.text = ((int)Vector3.Distance(player.transform.position, transform.position)) + "m";

            if (IsEnemyVisible())
            {
                UpdateUIPosition();
                //UpdateUIScale();
                if (nameUI != null)
                    nameUI.SetActive(true);
            }
            else
            {
                UpdateUIPositionOffScreen();
                //UpdateUIScale();
                if (nameUI != null)
                    nameUI.SetActive(true);
            }
        }
        else
        {
            if (nameUI != null)
            {
                nameUI.SetActive(false);
            }
        }
    }

    public void CreateEnemyUI()
    {
        if (nameUI == null)
        {
            nameUI = Instantiate(nameUIPrefab, canvas.transform);

            if(!isBatteryTag)
                distText = nameUI.GetComponentInChildren<Text>();
        }
        else
        {
            nameUI.SetActive(true);
        }
    }

    bool IsEnemyVisible()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);
        return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
    }

    void UpdateUIPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (nameUI != null)
        {
            Vector3 canvasPos = screenPos / canvas.scaleFactor;
            nameUI.transform.position = canvasPos;
        }
    }

    void UpdateUIPositionOffScreen()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        if (nameUI != null)
        {
            Vector3 canvasPos = screenPos / canvas.scaleFactor;
            nameUI.transform.position = canvasPos;
        }
    }

    void UpdateUIScale()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        float scaleFactor = Mathf.Clamp(10f / distance, 0.5f, 2f);

        if (nameUI != null)
            nameUI.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }
}
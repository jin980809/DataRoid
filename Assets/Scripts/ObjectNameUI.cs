using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNameUI : MonoBehaviour
{
    GameObject nameUI; // 积己等 利 UI
    public bool isNameTagOpen;
    public GameObject nameUIPrefab;
    public Canvas canvas;
    public Camera mainCamera;

    void Update()
    {
        HackingUI();
    }

    public void HackingUI()
    {
        if (isNameTagOpen)
        {
            CreateEnemyUI();
            //Debug.Log(IsEnemyVisible());
            if (IsEnemyVisible())
            {
                UpdateUIPosition();
                UpdateUIScale();
                if (nameUI != null)
                    nameUI.SetActive(true);
                
            }
            else
            {
                nameUI.SetActive(false);
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
            //sheildUI = enemyUI.transform.GetChild(0).GetComponent<Slider>();
            //hackingDurationUI = enemyUI.transform.GetChild(1).GetComponent<Slider>();
        }
        else
        {
            nameUI.SetActive(true);
        }
    }

    bool IsEnemyVisible()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);
        //Debug.Log(screenPos);
        return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;
    }

    void UpdateUIPosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //switch (hitAreaType)
        //{
        //    case Type.Head:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[0].position);
        //        break;
        //    case Type.Body:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[1].position);
        //        break;
        //    case Type.LeftArm:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[2].position);
        //        break;
        //    case Type.RightArm:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[3].position);
        //        break;
        //    case Type.LeftLeg:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[4].position);
        //        break;
        //    case Type.RightLeg:
        //        screenPos = Camera.main.WorldToScreenPoint(parts[5].position);
        //        break;
        //}

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

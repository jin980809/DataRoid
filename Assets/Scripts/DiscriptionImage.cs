using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscriptionImage : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public Button nextButton;
    public Button preButton;
    int curImageIndex = 0;
    bool qDown;

    void Start()
    {
        image.sprite = sprites[curImageIndex];
    }

    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");

        ButtonInteractable();

        if(qDown)
        {
            ExitImage();
        }
    }

    void ButtonInteractable()
    {
        if (curImageIndex == 0)
            preButton.interactable = false;
        else
            preButton.interactable = true;

        if (curImageIndex == sprites.Length - 1)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;
    }


    public void NextClick()
    {
        curImageIndex++;
        image.sprite = sprites[curImageIndex];
    }

    public void PreClick()
    {
        curImageIndex--;
        image.sprite = sprites[curImageIndex];
    }

    public void ExitImage()
    {
        transform.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextOnOff : MonoBehaviour
{
    public Player player;
    public TextBox[] textBox;
    public int curTextCount = 0;
    public bool isClick;
    bool isCommunicate = false;
    bool isNextReady = false;
    bool isText;

    [Serializable]
    public struct TextBox
    {
        [TextArea]
        public string droneText;
        public float duration;
    }

    public void InitTextBox(int i)
    {
        textBox = new TextBox[i];
    }

    public void TextOn()
    {
        if (!isText)
        {
            isText = true;

            StartCoroutine(StartTextBox(textBox[curTextCount].duration));
        }
    }

    IEnumerator StartTextBox(float durationTime)
    {
        UIManager.Instance.textUIAnim.SetTrigger("Open");
        UIManager.Instance.DroneText.text = textBox[curTextCount].droneText;

        if (!isClick)
        {
            yield return new WaitForSeconds(durationTime);
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }
        isNextReady = true;
        yield return null;
    }
    IEnumerator TextBoxOut(float durationTime)
    {
        UIManager.Instance.textUIAnim.SetTrigger("Close");

        yield return new WaitForSeconds(1f);
        if (curTextCount < textBox.Length - 1)
        {
            curTextCount += 1;

            StartCoroutine(StartTextBox(durationTime));
        }
        else
        {
            curTextCount = 0;
            isText = false;
            if (isClick)
            {
                player.isCommunicate = false;
                isCommunicate = false;
                isNextReady = false;
            }
        }
        yield return null;
    }
}

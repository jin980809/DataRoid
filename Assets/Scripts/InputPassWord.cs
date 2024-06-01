using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class InputPassWord : PassWord
{
    public TMP_InputField[] passWordInput;
    public string[] passWord;
    bool qDown;
    bool eDown;
    public bool isNameRegist;
    public bool isImageOnOnly;

    public TextBox[] textBox;
    [Serializable]
    public struct TextBox
    {
        [TextArea]
        public string droneText;
        public float duration;
    }

    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");
        eDown = Input.GetButton("Enter");

        if (qDown)
        {
            ExitPassWord();
        }

        if(eDown)
        {
            if(!isImageOnOnly)
                PassWordCheck();
        }
    }

    bool isPassWordCorrect()
    {
        for (int i = 0; i < passWord.Length; i++)
        {
            if (!string.Equals(passWord[i], passWordInput[i].text))
                return false;
        }

        return true;
    }

    public void PassWordCheck()
    {
        if (!isNameRegist)
        {
            if (isPassWordCorrect())
            {
                Debug.Log("unlock");
                isDone = true;
                PassWordResult();
                NameTagOnOff();
                ExitPassWord();
            }
            else
            {
                Debug.Log("false");
                UIManager.Instance.textOnOff.InitTextBox(textBox.Length);

                for (int i = 0; i < textBox.Length; i++)
                {
                    UIManager.Instance.textOnOff.textBox[i].droneText = textBox[i].droneText;
                    UIManager.Instance.textOnOff.textBox[i].duration = textBox[i].duration;
                }

                UIManager.Instance.textOnOff.TextOn();
            }
        }
        else
        {
            isDone = true;
            GameManager.Instance.userName = passWordInput[0].text;
            UIManager.Instance.t_QuestText.text = GameManager.Instance.userName + "ÀÇ ±â¾ï";
            PassWordResult();
            ExitPassWord();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputPassWord : PassWord
{
    public TMP_InputField[] passWordInput;
    public string[] passWord;
    bool qDown;

    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");

        if (isPassWordCorrect())
        {
            Debug.Log("unlock");
            isDone = true;
            PassWordResult();
            ExitPassWord();
        }

        if (qDown)
        {
            ExitPassWord();
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
}

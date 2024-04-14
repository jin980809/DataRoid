using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassWord : MonoBehaviour
{
    public Interaction interaction;
    public bool isDone = false;
    public TMP_InputField[] passWordInput;
    public string[] passWord;

    [Space(10)]
    [Header("Active")]
    public Collider[] a_col;
    public GameObject[] a_gameObj;
    public Interaction[] a_interaction;

    [Space(10)]
    [Header("Disable")]
    public Collider[] d_col;
    public GameObject[] d_gameObj;
    public Interaction[] d_interaction;

    bool qDown;

    void Start()
    {

    }

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

        if(qDown)
        {
            ExitPassWord();
        }
    }

    bool isPassWordCorrect()
    {
        for(int i = 0; i < passWord.Length; i++)
        {
            if (passWordInput[i].text != passWord[i])
                return false;
        }

        return true;
    }

    void PassWordResult()
    {
        for (int i = 0; i < a_col.Length; i++)
            a_col[i].enabled = true;

        for (int i = 0; i < a_gameObj.Length; i++)
            a_gameObj[i].SetActive(true);

        for (int i = 0; i < a_interaction.Length; i++)
            a_interaction[i].enabled = true;

        for (int i = 0; i < d_col.Length; i++)
            d_col[i].enabled = false;

        for (int i = 0; i < d_gameObj.Length; i++)
            d_gameObj[i].SetActive(false);

        for (int i = 0; i < d_interaction.Length; i++)
            d_interaction[i].enabled = false;

    }

    void ExitPassWord()
    {
        transform.gameObject.SetActive(false);
        interaction.isActive = false;
    }
}
